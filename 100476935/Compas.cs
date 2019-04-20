using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class Compas : ICritterController
    {
        Map map;
        Moving movment;
        Point goal = new Point(-1, -1);
        System.Timers.Timer getInfoTimer;
        bool headingForGoal = false;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        private static Point PointFrom(string coordinate)
        {
            string[] coordinateParts = coordinate.Substring(1, coordinate.Length - 2).Split(',');
            string rawX = coordinateParts[0].Substring(2);
            string rawY = coordinateParts[1].Substring(2);
            int x = int.Parse(rawX);
            int y = int.Parse(rawY);
            return new Point(x, y);
        }

        private void Log(string message)
        {
            if (Logger == null)
            {
                Console.WriteLine(message);
            }
            else
            {
                Logger(message);
            }
        }

        private void SetDestination(Point coordinate, int speed)
        {
            Responder("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
        }

        private void Tick()
        {
            Responder("GET_LEVEL_TIME_REMAINING:1");
        }

        private void LoadSettings()
        {
            string fileName = "Compas.cfg";
            string fileSpec = Filepath + "/" + fileName;
            try
            {
                using (StreamReader reader = new StreamReader(fileSpec))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] lineParts = line.Split('=');
                        switch (lineParts[0])
                        {
                            case "EatSpeed":
                                EatSpeed = int.Parse(lineParts[1]);
                                break;
                            case "HeadForExitSpeed":
                                HeadForExitSpeed = int.Parse(lineParts[1]);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log("Reading configuration " + fileSpec + " failed due to " + e);
            }
        }

        public void SaveSettings()
        {
            string fileName = "Compas.cfg";
            string fileSpec = Filepath + "/" + fileName;
            try
            {
                using (StreamWriter writer = new StreamWriter(fileSpec, false))
                {
                    writer.WriteLine("EatSpeed=" + EatSpeed);
                    writer.WriteLine("HeadForExitSpeed=" + HeadForExitSpeed);
                }
            }
            catch (Exception e)
            {
                Log("Writing configuration " + fileSpec + " failed due to " + e);
            }
        }

        public Compas(string name)
        {
            Name = name;

        }

        public void LaunchUI()
        {
            CompasSettings settings = new CompasSettings(this);
            settings.Show();
            settings.Focus();
        }

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                    LoadSettings();
                    Responder("GET_ARENA_SIZE:2081");
                    break;
                case "ARENA_SIZE":
                    map = new Map(message.Substring(0, 16));
                    map.CreateMap();
                    Responder("GET_LOCATION:2081");
                    break;
                case "LOCATION":
                    map.UpdateCritterLocation(message);
                    break;
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("SEE:2081");
                    break;
                case "SEE":
                    map.UpdateMap(message);
                    Responder(movment.MoveCritter(map.critterLocation, map));
                    break;

                case "SHUTDOWN":
                    getInfoTimer.Stop();
                    break;
                case "REACHED_DESTINATION":
                    Responder(movment.MoveCritter(map.critterLocation, map));
                    break;
                case "LEVEL_TIME_REMAINING":
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        }
    }
}
