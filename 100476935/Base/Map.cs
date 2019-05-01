
using CritterController;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;

namespace _100476935
{

    public class Map
    {
        public int mapWidth;
        public int mapHeight;
        public Point CritterLocation = new Point(0, 0);
        public Point Goal = new Point(0, 0);
        public Dictionary<string, String> MapInfo = new Dictionary<string, string>();
        // avoid, food, escape, evaluated   

        public Map()
        {
        }

        public Map(string _message)
        {
            _message = _message.Substring(_message.IndexOf('1') + 2);
            string[] holder = _message.Split(':');
            mapWidth = int.Parse(holder[0]);
            mapHeight = int.Parse(holder[1]);

            generateMapBoundries();

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
                    target = GeneratePoint(messageSegment);

                    if (messageSegment.Contains("Escape"))
                    {
                        UpdateMap(target, "Exit");
                    }
                    else if (messageSegment.Contains("Terrain"))
                    {
                        for (int i = -4; i != 5; i++)
                        {
                            for (int i2 = -4; i2 != 5; i2++)
                            {
                                UpdateMap(new Point(target.X + i, target.Y + i2), "Terrain");
                            }
                        }
                    }
                    else if (messageSegment.Contains("Bomb"))
                    {
                                UpdateMap(target, "Bomb");
                    }
                    else if (messageSegment.Contains("Food"))
                    {
                         UpdateMap(target, "Food");
                    }
                    else if (messageSegment.Contains("Gift"))
                    {
                       
                        UpdateMap(target, "Gift");

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
            int x = int.Parse(_message.Substring(_message.IndexOf('X') + 2, _message.IndexOf(',') - _message.IndexOf('X') - 2));
            int y = int.Parse(_message.Substring(_message.IndexOf('Y') + 2, _message.IndexOf('}') - _message.IndexOf('Y') - 2));
            CritterLocation = new Point(x, y);
        }

        public void generateMapBoundries()
        {
            for (int i = 0; i != mapWidth; i++)
            {
                UpdateMap(new Point(i, 0), "Terrain");
            }
            for (int i = 0; i != mapWidth; i++)
            {
                UpdateMap(new Point(i, mapHeight), "Terrain");
            }
            for (int i = 0; i != mapHeight; i++)
            {
                UpdateMap(new Point(0, i), "Terrain");
            }
            for (int i = 0; i != mapHeight; i++)
            {
                UpdateMap(new Point(mapWidth, i), "Terrain");
            }
        }

        public void ArrivedAtPoint()
        {
            try
            {
                string locationInfo = MapInfo[PointToString(CritterLocation)];
                if (locationInfo == "Food" || locationInfo == "Gift" || locationInfo == "Bomb")
                {
                    MapInfo.Remove(PointToString(CritterLocation));
                }
            }
            catch (KeyNotFoundException) { };
        }

        public void UpdateMap(Point _location, string type)
        {
            try
            {
                MapInfo.Add(PointToString(_location), type);
            }
            catch (ArgumentException)
            {
                if (type == "Terrain" && MapInfo[PointToString(_location)] != "Terrain")
                {
                    MapInfo[PointToString(_location)] = "Terrain";
                }
            }
        }
    }
}
