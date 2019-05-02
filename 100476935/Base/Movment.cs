using CritterController;
using CritterWorld;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{
    public class Movment
    {
        static string[] options = new string[5] { "Exit", "Terrain", "Bomb", "Food", "Gift" };
        private Dictionary<string, List<Point>> destinations; // all locations that can be 
        Dictionary<string, string> behavior;
        Random rand = new Random();
        Point previousDestination;


        public Movment()
        {

        }

        public Movment(Dictionary<string, string> _behavior)
        {
            behavior = _behavior;
            SetupDestinations();
        }

        public void SetupDestinations()
        {
            destinations = new Dictionary<string, List<Point>>();
            destinations.Add("Exit", new List<Point>());
            destinations.Add("Terrain", new List<Point>());
            destinations.Add("Bomb", new List<Point>());
            destinations.Add("Food", new List<Point>());
            destinations.Add("Gift", new List<Point>());
        }

        public string MoveCritter(Map _map, int _eatSpeed)
        {
            Boolean goFurther = false; // variable that defines if the "test location" has reached its limit distance
            Point testLocation = new Point(0, 0); // location that is compared to the generated map in V3 Mapping
            string[] axisTarget = new string[4] { "Y", "X", "Y", "X" };
            int[] distanceTraveled = new int[4] { 0, 0, 0, 0 };
            int[] incrementDecrement = new int[4] { 1, -1, -1, 1 };
            int[] finishingLocation = new int[4] { 0, 0, 0, 0 };
            Point returnValue;

            for (int index = 0; index != 4; index++) // for each direction
            {
                goFurther = true;
                while (goFurther)
                {
                    distanceTraveled[index] += incrementDecrement[index];

                    if (axisTarget[index] == "X")
                    {
                        testLocation = new Point(_map.CritterLocation.X + distanceTraveled[index], _map.CritterLocation.Y);
                    }
                    else if (axisTarget[index] == "Y")
                    {
                        testLocation = new Point(_map.CritterLocation.X, _map.CritterLocation.Y + distanceTraveled[index]);
                    }
                    goFurther = CheckLocation(_map, testLocation, distanceTraveled[index]);
                }
            }
            returnValue = resultSelection(_map);

            return MoveMessageCheck(returnValue, _map, _eatSpeed); 
        }

        private Boolean CheckLocation(Map _map, Point _testLocation, int distanceTraveled)
        {
            string holder = "";
            Boolean wallHit = false;

            if (_map.MapInfo.ContainsKey(_map.PointToString(_testLocation))) // does the test location contain anything
            {
                holder = _map.MapInfo[_map.PointToString(_testLocation)]; // holder = the info found at the dictionary key _testlocation
                Array.ForEach(options, option =>
                {
                    if (option == holder)
                    {
                        if (behavior[option] == "Avoid")
                        {
                            wallHit = true;
                        }
                        else
                        {
                            if(distanceTraveled < 25)
                            {
                                destinations[option].Add(_testLocation);
                            }

                        }
                    }
                });

            }
            if (wallHit)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Point resultSelection(Map _map)
        {
            Point holder = _map.CritterLocation;
            int counter = 0;
            List<Point> Results = new List<Point>();

            Array.ForEach(options, option =>
            {
                counter = destinations[option].Count; // in the current destination key, check how many entrys there are
                if (counter > 0)
                {
                    if (behavior[option] == "GoTo")
                    {
                        destinations[option].ForEach(destination => // if the critter is specified to go to that option, add it to the results 
                        {
                            Results.Add(destination);
                        });
                    }
                }
            });
            if (Results.Count < 1)
            {
                return RandomDestination(_map);
            }
            else
            {
                do
                {
                     holder = Results[rand.Next(Results.Count)];
                }
                 while (holder.X == _map.CritterLocation.X && holder.Y == _map.CritterLocation.Y);
                return holder;
            }
        }

        public Point RandomDestination(Map _map)
        {
            int[] holder = new int[2] {_map.CritterLocation.X, _map.CritterLocation.Y};
            int[] alteration = GenerateRandomPoints();
            holder[0] += alteration[0];
            holder[1] += alteration[1];
            return new Point(holder[0], holder[1]);
        }


        public string MoveMessageCheck(Point _message, Map _map, int _eatSpeed)
        {
            while(_message == previousDestination)
            {
                _message = RandomDestination(_map);
            }
            previousDestination = _message;
            return "SET_DESTINATION:" + _message.X + ":" + _message.Y +":" + _eatSpeed;
        }

        private int[] GenerateRandomPoints()
        {
            List<int[]> returnValue = new List<int[]>();

            returnValue.Add(new int[2] {0, 100});
            returnValue.Add(new int[2] { 0, -100 });
            returnValue.Add(new int[2] { 100, 0 });
            returnValue.Add(new int[2] { 100, 0 });

            return returnValue[rand.Next(returnValue.Count)];
        }

    }
}
