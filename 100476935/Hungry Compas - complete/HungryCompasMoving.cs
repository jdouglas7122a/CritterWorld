using CritterController;
using CritterWorld;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{
    public class HungryCompasMovment
    {
        private List<List<Point>> results = new List<List<Point>>(); // food, avoid

        public HungryCompasMovment()
        {

        }

        public string MoveCritter(HungryCompasMap _map)
        {
            Boolean goFurther = false; // variable that defines if the "test location" has reached its limit distance
            Random rand = new Random();
            Point testLocation = new Point(0, 0); // location that is compared to the generated map in V3 Mapping

            results = new List<List<Point>>();

            for (int i = 0; i != 2; i++)
            {
                results.Add(new List<Point>());
            }

            results.Add(new List<Point>());
            int[,] movementInfo = new int[4, 4] { // if it targets x or y, how far its traveled, weather it increments or decrements, finishing location score
                                                   { 1 , 0 , 1 , 0 }, { 0 , 0 , -1 , 0 },
                                                   { 1 , 0 , -1 , 0 }, { 0 , 0 , 1 , 0 }
                                                   };

            for (int index = 0; index != 4; index++) // for each direction
            {

                goFurther = true;
                while (goFurther) 
                {
                    movementInfo[index, 1] += movementInfo[index, 2]; //increment of decrement the disance the line has traveled
                    if (movementInfo[index, 0] == 0) 
                    {
                        testLocation = new Point(_map.CritterLocation.X + movementInfo[index, 1], _map.CritterLocation.Y); 
                    }
                    else if (movementInfo[index, 0] == 1) 
                    {
                        testLocation = new Point(_map.CritterLocation.X, _map.CritterLocation.Y + movementInfo[index, 1]);
                    }
                    goFurther = CheckForAvoid(_map, testLocation, movementInfo, index); 

                    if (goFurther == true)
                    {
                        CheckForFood(_map, testLocation, movementInfo, index);
                    }

                }
            }
            return resultSelection(rand, _map);
        }


        private string resultSelection(Random _rand, HungryCompasMap _map ) 
        {
            string returnValue = "SET_DESTINATION:";
            Point holder = new Point(0, 0);
            List<Point> randomPointGeneration = new List<Point>();
            randomPointGeneration.Add(new Point(0, 50));
            randomPointGeneration.Add(new Point(0, -50));
            randomPointGeneration.Add(new Point(50, 0));
            randomPointGeneration.Add(new Point(-50, 0));

            if (results[0].Count > 0)
            {
                holder = results[0][_rand.Next(results[0].Count)];
            }
            else
            {
                holder = randomPointGeneration[_rand.Next(randomPointGeneration.Count)];
                holder = new Point(_map.CritterLocation.X + holder.X, _map.CritterLocation.Y + holder.Y);
            }
        
            returnValue += holder.X + ":" + holder.Y;

            return returnValue;
        }

        private Boolean CheckForAvoid(HungryCompasMap _map, Point _testLocation, int[,] _movementInfo, int _index) //checks if a coordinate needs to be avoided 
        {
            if (_map.HungryCompasMapInfo[_map.PointToString(_testLocation)][0])
            {
                int multiplacationHolder = _movementInfo[_index, 2] * 5;

                if (_movementInfo[_index, 0] == 0)
                {
                    results[1].Add(new Point(_testLocation.X - multiplacationHolder, _testLocation.Y));
                }
                else if (_movementInfo[_index, 0] == 1)
                {
                    results[1].Add(new Point(_testLocation.X, _testLocation.Y - multiplacationHolder));
                }
                return false;
            }
            else
            {
                return true;
            }

        }

        private void CheckForFood(HungryCompasMap _map, Point _testLocation, int[,] _movementInfo, int _index)
        {
            if (_map.HungryCompasMapInfo[_map.PointToString(_testLocation)][1])
            {
                results[0].Add(_testLocation);
            }
        }

    }
}
