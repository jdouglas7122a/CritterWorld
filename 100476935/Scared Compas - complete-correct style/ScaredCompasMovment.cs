using CritterController;
using CritterWorld;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{
    public class ScaredCompasMovment
    {
        private List<List<Point>> results = new List<List<Point>>(); // goal, goal axis , objective, avoid

        public ScaredCompasMovment()
        {
        }

        public string MoveCritter(ScaredCompasMap _map, int _eatSpeed, int _headForExitSpeed )
        {
            Boolean goFurther = false; // variable that defines if the "test location" has reached its limit distance
            Random rand = new Random();
            Point testLocation = new Point(0, 0); // location that is compared to the generated map in V3 Mapping
            results = new List<List<Point>>();

            for (int i = 0; i != 4; i++)
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
                while (goFurther) // while the line has not come into contact with obstruction
                {
                    movementInfo[index, 1] += movementInfo[index, 2]; //increment of decrement the disance the line has traveled
                    if (movementInfo[index, 0] == 0) // if it targets X
                    {
                        testLocation = new Point(_map.CritterLocation.X + movementInfo[index, 1], _map.CritterLocation.Y); // x -1, y
                    }
                    else if (movementInfo[index, 0] == 1) // if it targets Y
                    {
                        testLocation = new Point(_map.CritterLocation.X, _map.CritterLocation.Y + movementInfo[index, 1]);
                    }
                    goFurther = CheckForAvoid(_map, testLocation, movementInfo, index); // checks what the 
                    if (goFurther == true)
                    {
                        CheckForExit(_map, testLocation, movementInfo, index);
                        CheckForExitAxis(_map, testLocation, movementInfo, index);
                    }

                }
            }

            return resultSelection(rand, _map, _eatSpeed,_headForExitSpeed);
        }

        private string resultSelection(Random _rand, ScaredCompasMap _map, int _eatSpeed, int _headForExitSpeed ) //takes generated list of coordinates and decides which will become the new destination
        {
            string returnValue = "SET_DESTINATION:";
            Point holder = new Point(0, 0);
            List<int> compareDistanceTraveled = new List<int>();
            List<int> compareDistanceTraveledPositive = new List<int>();
            Boolean exit = false;

            if(results[0].Count > 0)
            {
                holder = results[0][0];
                exit = true;
            }
            else if(results[1].Count > 0)
            {
                holder = results[1][_rand.Next(results[1].Count)];
            }
            if(exit == true)
            {
                returnValue += holder.X + ":" + holder.Y + ":" + _headForExitSpeed;
            }
            else
            {
                returnValue += holder.X + ":" + holder.Y + ":" + _eatSpeed;
            }

            return returnValue;
        }

        private Boolean CheckForAvoid(ScaredCompasMap _map, Point _testLocation, int[,] _movementInfo, int _index) //checks if a coordinate needs to be avoided 
        {
            if (_map.ScaredCompasMapInfo[_map.PointToString(_testLocation)][0])
            {
                int multiplacationHolder = _movementInfo[_index, 2] * 5;

                if (_movementInfo[_index, 0] == 0)
                {
                    results[2].Add(new Point(_testLocation.X - multiplacationHolder, _testLocation.Y));
                }
                else if (_movementInfo[_index, 0] == 1)
                {
                    results[2].Add(new Point(_testLocation.X, _testLocation.Y - multiplacationHolder));
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        private void CheckForExit(ScaredCompasMap _map, Point _testLocation, int[,] _movementInfo, int _index)
        {
            if (_map.ScaredCompasMapInfo[_map.PointToString(_testLocation)][2])
            {
                results[0].Add(_testLocation);
            }
        }

        private void CheckForExitAxis(ScaredCompasMap _map, Point _testLocation, int[,] _movementInfo, int _index)
        {

            if (_map.Goal.X == _testLocation.X || _map.Goal.Y == _testLocation.Y)
            {
                results[1].Add(_testLocation);
            }
        }

    }
}
