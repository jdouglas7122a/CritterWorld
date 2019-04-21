using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class CompasV2 : ICritterController
    {
        public CompasV2Movment move;

        public CompasV2Map map;
    
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

        private void LoadSettings()
        {
            string fileName = "CompasV2.cfg";
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
            string fileName = "CompasV2.cfg";
            string fileSpec = Filepath + "/" + fileName;
            try
            {
                using (StreamWriter writer = new StreamWriter(fileSpec, false)) {
                    writer.WriteLine("EatSpeed=" + EatSpeed);
                    writer.WriteLine("HeadForExitSpeed=" + HeadForExitSpeed);
                }
            }
            catch (Exception e)
            {
                Log("Writing configuration " + fileSpec + " failed due to " + e);
            }
        }

        public CompasV2(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            CompasV2Settings settings = new CompasV2Settings(this);
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
                    Responder("GET_ARENA_SIZE:2082");
                    break;
                case "ARENA_SIZE":
                    map = new CompasV2Map(message);
                    Responder("SCAN:2082");
                    break;
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("GET_LOCATION:2082");
                    break;

                case "LOCATION":
                    map.UpdateCritterLocation(message);
                    Responder("SEE:2082");
                    break;
                case "SEE":
                    map.UpdateMap(message);
                    Responder(move.MoveCritter(map) + ":5");
                    break;
                case "REACHED_DESTINATION":
                    Responder("GET_LOCATION:2082");
                    break;
                case "ERROR":
                    break;
            }
        }
    }
}
