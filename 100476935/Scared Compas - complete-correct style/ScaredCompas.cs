﻿using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class ScaredCompas : ICritterController
    {

        //scared wants to get out of the arena as quickly as possible, doesnt understand the concept of walls very well when on axis with the exit

        Point goal = new Point(-1, -1);

        System.Timers.Timer getInfoTimer;

        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

        public int EatSpeed { get; set; } = 5;

        public int HeadForExitSpeed { get; set; } = 5;

        public bool arenaInitialized = false;

        public ScaredCompasMap map = new ScaredCompasMap();

        public ScaredCompasMovment move = new ScaredCompasMovment();

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
            string fileName = "ScaredCompas.cfg";
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
            string fileName = "ScaredCompas.cfg";
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

        public ScaredCompas(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            ScaredCompasSettings settings = new ScaredCompasSettings(this);
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
                    break;
                case "ARENA_SIZE":
                    map = new ScaredCompasMap(message);
                    arenaInitialized = true;
                    Responder("SCAN:2");
                    break;
                case "SCAN":
                    map.UpdateMap(message);
                    Responder("GET_LOCATION:3");
                    break;
                case "LOCATION":
                    map.UpdateCritterLocation(message);
                    Responder(move.MoveCritter(map, EatSpeed, HeadForExitSpeed));
                    break;
                case "BUMP":
                    Responder("STOP");
                    Responder("RANDOM_DESTINATION");
                    break;
                case "REACHED_DESTINATION":
                    Responder("GET_LOCATION:3");
                    break;
            }
        }
    }
}
