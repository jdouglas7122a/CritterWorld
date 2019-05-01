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
                    goFurther = CheckLocation(_map, testLocation);
                }
            }
            return resultSelection(_map, _eatSpeed);
        }

        private string resultSelection(Map _map, int _eatSpeed)
        {
            Point holder = new Point(0, 0);
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
                return RandomDestination(_map, _eatSpeed);
            }
            else
            {
                do
                {
                    holder = Results[rand.Next(Results.Count)];
                    if(holder.X == _map.CritterLocation.X && holder.Y == _map.CritterLocation.Y && Results.Count == 1)
                    {
                        return RandomDestination(_map, _eatSpeed);
                    }
                }
                while (holder.X == _map.CritterLocation.X && holder.Y == _map.CritterLocation.Y);
                return "SET_DESTINATION:" + holder.X + ":" + holder.Y + ":" + _eatSpeed;
            }
        }

        public string RandomDestination(Map _map, int _eatSpeed)
        {
            Point holder = _map.CritterLocation;
            List<Point> locationAlteration = GenerateRandomPoints();
            do
            {
                do
                {
                    Point alteration = locationAlteration[rand.Next(locationAlteration.Count)];
                    holder = new Point(holder.X + alteration.X, holder.Y + alteration.Y);
                }
                while (holder.X >= _map.mapWidth || holder.X <= 0 || holder.Y >= _map.mapHeight || holder.Y <= 0);
            }
            while (holder.X == _map.CritterLocation.X && holder.Y == _map.CritterLocation.Y);
           

            return "SET_DESTINATION:" + holder.X + ":" + holder.Y + ":" + _eatSpeed;
        }

        private Boolean CheckLocation(Map _map, Point _testLocation)
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
                            destinations[option].Add(_testLocation);
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

        private List<Point> GenerateRandomPoints()
        {
            List<Point> returnValue = new List<Point>();

            returnValue.Add(new Point(50, 0));
            returnValue.Add(new Point(150, 0));
            returnValue.Add(new Point(0, 50));
            returnValue.Add(new Point(0, 150));
            returnValue.Add(new Point(0, 100));
            returnValue.Add(new Point(0, -100));
            returnValue.Add(new Point(100, 0));
            returnValue.Add(new Point(-100, 0));

            return returnValue;
        }

    }
}
