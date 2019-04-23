using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{
    public class CompasV3Movment
    {
        public CompasV3Movment()
        {

        }

        string returnValue = "SET_DESTINATION:";

        public string MoveCritter(CompasV3Map _map)
        {
            Random rand = new Random();
            Boolean goFurther = false;
            Point testLocation = new Point(0,0);
            List<Point> results = new List<Point>();
            int[,] movementInfo = new int[4, 4] { // if it targets x or y, how far its traveled, weather it increments or decrements, finishing location score
                                                   { 1 , 0 , 1 , 0 }, { 0 , 0 , -1 , 0 },
                                                   { 1 , 0 , -1 , 0 }, { 0 , 0 , 1 , 0 }
                                                   };

            for(int index = 0; index != 4; index++) // for each direction
            {
                goFurther = true;
                while (goFurther) // while the line has not come into contact with obstruction
                {
                    movementInfo[index, 1] += movementInfo[index, 2]; //increment of decrement the disance the line has traveled
                    if (movementInfo[index, 0] == 0) // if it targets X
                    {
                        testLocation = new Point(_map.CritterLocation.X + movementInfo[index, 1], _map.CritterLocation.Y);
                    }
                    else if (movementInfo[index, 0] == 1) // if it targets Y
                    {
                        testLocation = new Point(_map.CritterLocation.X, _map.CritterLocation.Y + movementInfo[index, 1]);
                    }   



                    if (_map.compasV3MapInfo[_map.PointToString(testLocation)][0]) //checks the location dictionary to see if the tester location is to be avoided
                    {
                        goFurther = false;

                        if(movementInfo[index, 0] == 0)
                        {
                            results.Add(new Point(testLocation.X - (5 * movementInfo[index, 2]), testLocation.Y));
                        }
                        else if(movementInfo[index, 0] == 1)
                        {
                            results.Add(new Point(testLocation.X, testLocation.Y - (5 * movementInfo[index, 2])));
                        }
                    }
                    else if (movementInfo[index, 1] >= 50)
                    {
                        goFurther = false;
                        results.Add(testLocation);
                    }
                }
            }
            results.ForEach(result => //is result on axis with goal 
            {
                if (result.X == _map.Goal.X || result.Y == _map.Goal.Y)
                {
                    returnValue += result.X + ":" + result.Y;
                }
            });
            if (returnValue == "SET_DESTINATION:") // is return value still empty
            {
                Point holder = results[rand.Next(3)];
                returnValue += holder.X + ":" + holder.Y;
            }
            return returnValue;
        }
    }
}
