using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class CompasV3 : ICritterController
    {

        Point goal = new Point(-1, -1);

        System.Timers.Timer getInfoTimer;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        public bool arenaInitialized = false;

        public CompasV3Map map = new CompasV3Map();

        public CompasV3Movment move = new CompasV3Movment();

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

        private void LoadSettings()
        {
            string fileName = "CompasV3.cfg";
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
            string fileName = "CompasV3.cfg";
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

        public CompasV3(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            CompasV3Settings settings = new CompasV3Settings(this);
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
                    arenaInitialized = false;
                    LoadSettings();
                    Responder("STOP");
                    Responder("GET_ARENA_SIZE:1");
                    break;
                case "SEE":
                    if (arenaInitialized)
                    {
                        map.UpdateMap(message);
                    }
                    break;
                case "CRASHED":
                    System.IO.File.WriteAllText(@"C:\Users\jdoug\Desktop\General.txt", message);
                    break;
                case "ARENA_SIZE":
                    map = new CompasV3Map(message);
                    arenaInitialized = true;
                    Responder("SCAN:2");
                    break;
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("GET_LOCATION:3");
                    break;
                case "LOCATION":
                    map.UpdateCritterLocation(message);
                    Responder(move.MoveCritter(map) + ":" +EatSpeed);
                    break;

                case "REACHED_DESTINATION":
                    Responder("GET_LOCATION:3");

                    break;
                





            }
        }
    }
}
