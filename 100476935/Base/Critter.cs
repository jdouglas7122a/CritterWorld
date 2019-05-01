using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{
    public abstract class BaseCritter : ICritterController
    {
        Point goal = new Point(-1, -1);
        protected Dictionary<string, string> behavior = new Dictionary<string, string>();

        Map map;

        Movment move = new Movment();

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        public bool arenaInitialized = false;

        protected void Log(string message)
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

        protected void SetDestination(Point coordinate, int speed)
        {
            Responder("SET_DESTINATION:" + coordinate.X + ":" + coordinate.Y + ":" + speed);
        }
        public void LoadSettings()
        {
            string fileName = Name + ".cfg";
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
            string fileName = Name + ".cfg";
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

        public abstract void LaunchUI();

        public void Receive(string message)
        {
            string[] dividedMessage = DivideMessage(message);

            switch (dividedMessage[0])
            {
                case "LAUNCH":
                    move = new Movment(behavior);
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
                case "ARENA_SIZE":
                    map = new Map(message);
                    arenaInitialized = true;
                    Responder("SCAN:2");
                    break;
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("GET_LOCATION:3");
                    break;
                case "LOCATION":
                    Responder("STOP");
                    map.UpdateCritterLocation(message);
                    Responder(move.MoveCritter(map, EatSpeed));
                    break;
                case "BUMP":
                    Responder("STOP");
                    map.UpdateCritterLocation(message);
                    Responder(move.RandomDestination(map, EatSpeed));
                    break;
                case "FIGHT":
                    Responder("STOP");
                    map.UpdateCritterLocation(message);
                    Responder(move.RandomDestination(map, EatSpeed));
                    break;
                case "REACHED_DESTINATION":
                    Responder("STOP");
                    Responder("GET_LOCATION:3");
                    break;
                case "CRASHED":
                    System.IO.File.WriteAllText(@"C:\Users\jdoug\Desktop\CrashReport.txt", message);
                    break;
            }
        }

        public string[] DivideMessage(string _message)
        {
            string[] returnValue;

            Log("Message from body for " + Name + ": " + _message);
            returnValue = _message.Split(':');

            return returnValue;
        }



    }
}
