using CritterController;
using CritterWorld;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{
    public class CompasV3Movment
    {
        public List<List<Point>> results = new List<List<Point>>(); // goal, objective, avoid

        public CompasV3Movment()
        {

        }

        public string MoveCritter(CompasV3Map _map)
        {
            Boolean goFurther = false;
            string returnValue = "SET_DESTINATION:";
            Random rand = new Random();
            Point testLocation = new Point(0, 0);

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


                    goFurther = CheckForAvoid(_map, testLocation, movementInfo, index);

                    if (movementInfo[index, 2] == 1 && movementInfo[index, 1] > 150)
                    {
                        System.IO.File.WriteAllText(@"C:\Users\jdoug\Desktop\General.txt", testLocation.X + " " + testLocation.Y);
                        goFurther = false;
                        results[1].Add(testLocation);
                    }
                    else if (movementInfo[index, 2] == -1 && movementInfo[index, 1] < -150)
                    {
                        goFurther = false;
                        results[2].Add(testLocation);
                    }


                    if (goFurther == true)
                    {
                        CheckForExit(_map, testLocation, movementInfo, index);

                        CheckForObjective(_map, testLocation, movementInfo, index);
                    }

                }
            }

            Point holder = new Point(0, 0);

            if (results[0].Count == 0)
            {
                if (results[1].Count == 0)
                {
                    holder = results[2][rand.Next(results[2].Count)];
                }
                else
                {
                    holder = results[1][rand.Next(results[1].Count)];
                }
            }
            else
            {
                holder = results[0][0];
            }

            returnValue += holder.X + ":" + holder.Y;

            return returnValue;
        }




        public Boolean CheckForAvoid(CompasV3Map _map, Point _testLocation, int[,] _movementInfo, int _index)
        {
            if (_map.compasV3MapInfo[_map.PointToString(_testLocation)][0])
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

        public void CheckForExit(CompasV3Map _map, Point _testLocation, int[,] _movementInfo, int _index)
        {
            if (_map.compasV3MapInfo[_map.PointToString(_testLocation)][2])
            {
                results[0].Add(_testLocation);
            }
        }

        public void CheckForObjective(CompasV3Map _map, Point _testLocation, int[,] _movementInfo, int _index)
        {
            if (_map.compasV3MapInfo[_map.PointToString(_testLocation)][1])
            {
                results[1].Add(_testLocation);
            }
        }

    }
}
