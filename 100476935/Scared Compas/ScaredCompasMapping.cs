
using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{

    public class ScaredCompasMap
    {
        public int mapWidth;
        public int mapHeight;
        public Point CritterLocation = new Point(0,0);
        public Point Goal = new Point (0,0);
        public Dictionary<string, List<Boolean>> ScaredCompasMapInfo = new Dictionary<string, List<Boolean>>();
        // avoid, food, escape, evaluated
        public bool goalAxisEnter = false;

        public ScaredCompasMap()
        {

        }
    
        public ScaredCompasMap(string _message)
        {
            _message = _message.Substring(_message.IndexOf('1') + 2);
            string[] holder = _message.Split(':');
            mapWidth = int.Parse(holder[0]);
            mapHeight = int.Parse(holder[1]);
            Boolean[] PointConditions = new Boolean[4] { false, false, false, false };
            int[] count = new int[2] { 0, mapWidth * mapHeight };
            goalAxisEnter = false;

            for (int i = 0; i != mapWidth + 1; i++)
            {
                for (int i2 = 0; i2 != mapHeight + 1; i2++)
                {
                    ScaredCompasMapInfo.Add(PointToString(new Point(i, i2)), new List<Boolean>(PointConditions));
                }
            }

            for (int i = 0; i != mapWidth; i++)
            {
                ScaredCompasMapInfo[PointToString(new Point(i, 0))][0] = true;
                ScaredCompasMapInfo[PointToString(new Point(i, 0))][3] = true;
            }
            for (int i = 0; i != mapWidth; i++)
            {

                ScaredCompasMapInfo[PointToString(new Point(i, mapHeight))][0] = true;
                ScaredCompasMapInfo[PointToString(new Point(i, mapHeight))][3] = true;
            }
            for (int i = 0; i != mapHeight; i++)
            {
                ScaredCompasMapInfo[PointToString(new Point(0, i))][0] = true;
                ScaredCompasMapInfo[PointToString(new Point(0, i))][3] = true;
            }
            for (int i = 0; i != mapHeight; i++)
            {
                ScaredCompasMapInfo[PointToString(new Point(mapWidth, i))][0] = true;
                ScaredCompasMapInfo[PointToString(new Point(mapWidth, i))][3] = true;
            }

        }

        public void UpdateMap(string _message)
        {

            string[] holder = _message.Split('\t');
            Point target = new Point(0, 0);
            if (!holder[0].Contains("Nothing"))
            {
                holder[0] = holder[0].Substring(holder[0].IndexOf('\n') + 1, holder[0].IndexOf('}') - holder[0].IndexOf('\n'));
                Array.ForEach(holder, messageSegment =>
                {
                    if (messageSegment.Contains("EscapeHatch"))
                    {
                        target = GeneratePoint(messageSegment);
                        ScaredCompasMapInfo[PointToString(target)][2] = true;
                        ScaredCompasMapInfo[PointToString(target)][3] = true;
                        Goal = target;
                    }

                    if (messageSegment.Contains("Terrain"))
                    {
                        target = GeneratePoint(messageSegment);
                        ScaredCompasMapInfo[PointToString(target)][2] = true;
                        ScaredCompasMapInfo[PointToString(target)][3] = true;
                        for (int i = -4; i != 5; i++)
                        {
                            for (int i2 = -4; i2 != 5; i2++)
                            {
                                ScaredCompasMapInfo[PointToString(new Point(target.X + i, target.Y + i2))][2] = true;
                                ScaredCompasMapInfo[PointToString(new Point(target.X + i, target.Y + i2))][3] = true;

                            }
                        }

                    }

                    else if (messageSegment.Contains("Bomb") || messageSegment.Contains("Terrain"))
                    {
                        target = GeneratePoint(messageSegment);
                        ScaredCompasMapInfo[PointToString(target)][0] = true;
                        ScaredCompasMapInfo[PointToString(target)][3] = true;
                    }
                    else if (messageSegment.Contains("Food") || messageSegment.Contains("Gift"))
                    {
                        target = GeneratePoint(messageSegment);
                        ScaredCompasMapInfo[PointToString(target)][1] = true;
                        ScaredCompasMapInfo[PointToString(target)][3] = true;
                    }

                });
            }
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
            _message = _message.Substring(_message.IndexOf("3") + 5);
            string holder = "";
            holder = _message.Substring(0, _message.IndexOf(','));
            int num = (_message.IndexOf('}')) - (_message.IndexOf(',') + 3);
            _message = _message.Substring(_message.IndexOf(',') + 3, num);
            CritterLocation = new Point(int.Parse(holder), int.Parse(_message));
        }
    }
}