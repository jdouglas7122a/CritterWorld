using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class CompasV2 : ICritterController
    {
        public CompasV2Movment move = new CompasV2Movment();

        public CompasV2Map map = new CompasV2Map();
    
        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

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
                    break;//doesnt crash
                case "ARENA_SIZE":
                    map = new CompasV2Map(message);
                    Responder("SCAN:2082");
                    break; // doesnt crash
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("GET_LOCATION:2082");
                    break;//doesnt crash

                case "LOCATION":
                    map.UpdateCritterLocation(message);
                    Responder("SEE:2082");
                    break; //doesnt crash

                    /* quarantine zone


                case "SEE":
                    map.UpdateMap(message); //doesnt crash
                    Responder(move.MoveCritter(map) + ":5"); //crash
                    break;

                    */
                    
                case "CRASHED":
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
