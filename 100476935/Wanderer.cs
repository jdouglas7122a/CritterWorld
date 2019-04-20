﻿using CritterController;
using System;

namespace _100476935
{
    public class Wanderer : ICritterController
    {
        public string Name { get; set; }

        public Send Responder { get; set; }

        public Send Logger { get; set; }

        public string Filepath { get; set; }

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

        public Wanderer(string name)
        {
            Name = name;
        }

        public void LaunchUI()
        {
            // TODO - need to provide this.
        }

        public void Receive(string message)
        {
            Log("Message from body for " + Name + ": " + message);
            string[] msgParts = message.Split(':');
            string notification = msgParts[0];
            switch (notification)
            {
                case "LAUNCH":
                case "REACHED_DESTINATION":
                case "FIGHT":
                case "BUMP":
                    Responder("RANDOM_DESTINATION");
                    break;
                case "ERROR":
                    Log(message);
                    break;
            }
        }
    }
}
