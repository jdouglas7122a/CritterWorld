using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;


namespace _100476935
{

    public class Moving
    {
        public string MoveCritter(Point _location, CompasMap _mapData) //recieves critter location and the general map data
        {
            Point goal = new Point();
            Dictionary<Point, Boolean> MapPoints = new Dictionary<Point, bool>();
            string returnValue = "SET_DESTINATION:";
            Random rand = new Random();

            foreach (MapSector target in _mapData.mapInfo)
            {
                MapPoints.Add(target.location, target.avoid);
                if (target.goal == true)
                {
                    goal = new Point(target.location.X, target.location.Y);
                }
            }


            Boolean canGo = false;
            int[,] indexIdentifier = new int[4, 4] { { 1, 0, 1, 0 }, { 0, 0, -1, 0 }, { 1, 0, -1, 0 }, { 0, 0, 1, 0 } }; // if it targets x or y, how far its traveled, weather it increments or decrements, how far away from the exit the destination is
            Point[] lastAccessiblePoint = new Point[4];
            Point tester;
            List<Point> results = new List<Point>();

            for (int index1 = 0; index1 != 4; index1++)
            {

                canGo = true;
                while (canGo == true)
                {
                    indexIdentifier[index1, 1] += indexIdentifier[index1, 2];
                    if (indexIdentifier[index1, 0] == 0)
                    {
                        tester = new Point(_location.X + indexIdentifier[index1, 1], _location.Y);
                    }
                    else
                    {
                        tester = new Point(_location.X, _location.Y + indexIdentifier[index1, 1]);
                    }
                    if (MapPoints[tester])//if the point targeted is no entry------
                    {
                        canGo = false;
                        if (indexIdentifier[index1, 0] == 0)
                        {
                            results.Add(new Point(tester.X - (10 * indexIdentifier[index1, 2]), tester.Y));
                        }
                        else
                        {
                            results.Add(new Point(tester.X, tester.Y - (10 * indexIdentifier[index1, 2])));
                        }
                    }
                }
                results.ForEach(result => // if any result is on axis with the exit choose that one
                {
                    if (result.X == goal.X || result.Y == goal.Y)
                    {
                        returnValue += result.X + ":" + result.Y;
                    }
                });
                if (returnValue == "SET_DESTINATION:")// if no results are on axis with the exit, then a random destination is chosen
                {
                    Point holder = results[rand.Next(3)];

                    returnValue += holder.X + ":" + holder.Y;
                }

            }
            return returnValue;
        }
    }


}
