using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{

    public class CompasV2Map
    {
        public int mapWidth;
        public int mapHeight;
        public Point CritterLocation;
        public Point Goal;
        public Dictionary<Point, List<bool>> compasV2MapInfo = new Dictionary<Point, List<bool>>();
        // avoid, food, escape, evaluated

        public CompasV2Map(string message)
        {
            message = message.Substring(message.IndexOf("2082")+4);
            string[] holder = message.Split(':');
            mapWidth = int.Parse(holder[0]);
            mapHeight = int.Parse(holder[1]);
            int[] count = new int[2] {0 ,mapWidth * mapHeight };
            
            for(int i = 0; i != mapWidth; i++)
            {
                for(int i2 = 0; i2 != mapHeight; i2++)
                {
                    compasV2MapInfo.Add(new Point(i,i2), new List<Boolean>());
                    for (int i3 = 0; i3 != 4; i++) // assings all bools within the point
                    {
                        compasV2MapInfo[new Point(i, i2)].Add(false);
                    }
                }
            }
     
        }

        public void UpdatePoint(string _message)
        {
            string[] holder = _message.Split('\t');
            Point target;

            Array.ForEach(holder, messageSegment => 
            {
                if (messageSegment.Contains("EscapeHatch"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[target][2] = true;
                    compasV2MapInfo[target][3] = true;
                }
                else if (messageSegment.Contains("Bomb"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[target][0] = true;
                    compasV2MapInfo[target][3] = true;
                }
                else if (messageSegment.Contains("Terrain"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[target][0] = true;
                    compasV2MapInfo[target][3] = true;
                }
                else if (messageSegment.Contains("Food"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[target][1] = true;
                    compasV2MapInfo[target][3] = true;
                }
            });
        }

        public Point GeneratePoint(string _message)
        {
            string location1 = _message.Substring(_message.IndexOf('=') + 1, (_message.IndexOf(',')) - (_message.IndexOf('=') + 1));
            string location2 = _message.Substring(_message.IndexOf('=', _message.IndexOf('=') + 1) + 1, (_message.IndexOf('}')) - (_message.IndexOf('=', _message.IndexOf('=') + 1) + 1));

            return new Point(int.Parse(location1), int.Parse(location2));
        }


    }









}