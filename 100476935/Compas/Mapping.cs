using CritterController;
using System;
using System.Drawing;
using System.IO;

namespace _100476935
{
    public class MapSector
    {
        public bool avoid;
        public Point location;
        public bool goal;
        public bool evaluated;
        public int[] mappingCalculation = new int[3];


        public MapSector(Point _location)
        {
            location = _location;
        }
    }


    public class CompasMap
    {
        public int mapWidth;
        public int mapHeight;
        public MapSector[,] mapInfo;
        public Point critterLocation { get; set; }

        public CompasMap(string _dimensions) // recieves just the number part of the message, no splitting in method
        {
            string[] holder = _dimensions.Split(':');
            mapWidth = int.Parse(holder[0]);
            mapHeight = int.Parse(holder[1]);
            mapInfo = new MapSector[mapWidth, mapHeight];
        }

        public void CreateMap() // plots all locations on map
        {
            int[] count = new int[2] { 0, 0 };

            foreach (MapSector i in mapInfo)
            {
                i.goal = false;
                i.evaluated = false;
                i.avoid = false;
                Array.ForEach(i.mappingCalculation, calcTarget => calcTarget = 0);
                i.location = new Point(count[0], count[1]);
                if (count[1] == mapHeight)
                {
                    count[1] = 0;
                    count[0]++;
                }
                count[1]++;
            };
        }

        public void UpdateCritterLocation(string message)
        {
            critterLocation = GeneratePoint(message);
        }

        public void UpdateMap(string canSee) // updates map with escape hatch, terrain and bombs
        {
            Point updatePoint = new Point();
            string[] holder = canSee.Split('\t'); // indexer is each string in holder
            Array.ForEach(holder, indexer =>
            {
                if (indexer.Contains("EscapeHatch"))
                {
                    updatePoint = GeneratePoint(indexer);

                    foreach (MapSector i in mapInfo)
                    {
                        if (i.location == updatePoint)
                        {
                            i.goal = true;
                            i.evaluated = true;
                        }
                    }
                }
                else if (indexer.Contains("Bomb") || indexer.Contains("Terrain"))
                {
                    updatePoint = GeneratePoint(indexer);

                    foreach (MapSector i in mapInfo)
                    {
                        if (i.location == updatePoint)
                        {
                            i.avoid = true;
                            i.evaluated = true;
                        }
                    }
                }
            });
        }

        public Point GeneratePoint(string _seeMessage) // finds point of object to be mapped
        {
            string location1 = _seeMessage.Substring(_seeMessage.IndexOf('=') + 1, (_seeMessage.IndexOf(',')) - (_seeMessage.IndexOf('=') + 1));
            string location2 = _seeMessage.Substring(_seeMessage.IndexOf('=', _seeMessage.IndexOf('=') + 1) + 1, (_seeMessage.IndexOf('}')) - (_seeMessage.IndexOf('=', _seeMessage.IndexOf('=') + 1) + 1));

            return new Point(int.Parse(location1), int.Parse(location2));
        }
    }




}
