
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
        public Dictionary<string, List<Boolean>> compasV2MapInfo = new Dictionary<string, List<Boolean>>();
        // avoid, food, escape, evaluated

        public CompasV2Map(string _message)
        {
            _message = _message.Substring(_message.IndexOf("2082") + 5);
            string[] holder = _message.Split(':');
            mapWidth = int.Parse(holder[0]);
            mapHeight = int.Parse(holder[1]);
            Boolean[] PointConditions = new Boolean[4] { false, false, false, false };
            int[] count = new int[2] { 0, mapWidth * mapHeight };

            for (int i = 0; i != mapWidth + 1; i++)
            {
                for (int i2 = 0; i2 != mapHeight + 1; i2++)
                {
                    compasV2MapInfo.Add(PointToString(new Point(i, i2)), new List<Boolean>());
                    compasV2MapInfo[PointToString(new Point(i, i2))].AddRange(PointConditions);
                }
            }

            for (int i = 0; i != mapWidth; i++)
            {
                compasV2MapInfo[PointToString(new Point(i, 0))][0] = true;
                compasV2MapInfo[PointToString(new Point(i, 0))][3] = true;
            }
            for (int i = 0; i != mapWidth; i++)
            {

                compasV2MapInfo[PointToString(new Point(i, mapHeight))][0] = true;
                compasV2MapInfo[PointToString(new Point(i, mapHeight))][3] = true;
            }
            for (int i = 0; i != mapHeight; i++)
            {
                compasV2MapInfo[PointToString(new Point(0, i))][0] = true;
                compasV2MapInfo[PointToString(new Point(0, i))][3] = true;
            }
            for (int i = 0; i != mapHeight; i++)
            {
                compasV2MapInfo[PointToString(new Point(mapWidth, i))][0] = true;
                compasV2MapInfo[PointToString(new Point(mapWidth, i))][3] = true;
            }

        }

        public void UpdateMap(string _message)
        {
            string[] holder = _message.Split('\t');
            Point target;

            Array.ForEach(holder, messageSegment =>
            {
                if (messageSegment.Contains("EscapeHatch"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[PointToString(target)][2] = true;
                    compasV2MapInfo[PointToString(target)][3] = true;
                    Goal = target;
                }
                else if (messageSegment.Contains("Bomb"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[PointToString(target)][0] = true;
                    compasV2MapInfo[PointToString(target)][3] = true;
                }
                else if (messageSegment.Contains("Terrain"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[PointToString(target)][0] = true;
                    compasV2MapInfo[PointToString(target)][3] = true;
                }
                else if (messageSegment.Contains("Food"))
                {
                    target = GeneratePoint(messageSegment);
                    compasV2MapInfo[PointToString(target)][1] = true;
                    compasV2MapInfo[PointToString(target)][3] = true;
                }
            });
        }

        public Point GeneratePoint(string _message)
        {
            string location1 = _message.Substring(_message.IndexOf('=') + 1, (_message.IndexOf(',')) - (_message.IndexOf('=') + 1));
            string location2 = _message.Substring(_message.IndexOf('=', _message.IndexOf('=') + 1) + 1, (_message.IndexOf('}')) - (_message.IndexOf('=', _message.IndexOf('=') + 1) + 1));

            return new Point(int.Parse(location1), int.Parse(location2));
        }

        public String PointToString(Point point)
        {
            string result = "";

            result = point.X.ToString() + ":" + point.Y.ToString();

            return result;
        }

        public void UpdateCritterLocation(string _message)
        {
            _message = _message.Substring(_message.IndexOf("2082") + 8);
            string holder = "";
            holder = _message.Substring(0, _message.IndexOf(','));
            int num = (_message.IndexOf('}')) - (_message.IndexOf(',') + 3);
            _message = _message.Substring(_message.IndexOf(',') + 3, num);
            CritterLocation = new Point(int.Parse(holder), int.Parse(_message));
        }
    }
}