using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TrafficSimulation
{
    public class SimulationMap
    {
        public Tile[,] map;
        
        private int smallX, smallY;
        private int largeX, largeY;
        private List<Tile> tileList;

        public SimulationMap()
        {
            tileList = new List<Tile>();
        }

        public void CreateMap()
        {
            CreateList(tileList);
        }

        public Tile[,] Map { get { return map; } }

        private void CreateList(List<Tile> tileList)
        {
            createArray(tileList);

            foreach (Tile t in tileList)
            {
                if (t != null)
                {
                    map[ToGrid(t.position).X, ToGrid(t.position).Y] = t;
                }
            }
        }

        private void createArray(List<Tile> tileList)
        {
            smallX = 0;
            largeX = 0;
            smallY = 0;
            largeY = 0;

            bool first = true;

            foreach (Tile t in tileList)
            {
                if (t != null && first == true)
                {
                    smallX = t.position.X;
                    largeX = t.position.X;
                    smallY = t.position.Y;
                    largeY = t.position.Y;

                    first = false;
                }
                else
                {
                    if (t != null)
                    {
                        if (t.position.X < smallX)
                        {
                            smallX = t.position.X;
                        }
                        else if (t.position.X > largeX)
                        {
                            largeX = t.position.X;
                        }

                        if (t.position.Y < smallY)
                        {
                            smallY = t.position.Y;
                        }
                        else if (t.position.Y > largeY)
                        {
                            largeY = t.position.Y;
                        }
                    }
                }
            }

            smallX = smallX / 100;
            smallY = smallY / 100;
            largeX = largeX / 100;
            largeY = largeY / 100;
            map = new Tile[largeX - smallX + 1, largeY - smallY + 1];
        }

        public List<Tile> GetMap()
        {
            return tileList;
        }

        public Tile GetTileAbove(Point position)
        {
            if (ToGrid(position).Y > 0)
            {
                return map[ToGrid(position).X, ToGrid(position).Y - 1];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileBelow(Point position)
        {
            if (ToGrid(position).Y < Map.GetLength(0) - 1)
            {
                int x = ToGrid(position).X;
                int y = ToGrid(position).Y;
                return map[ToGrid(position).X, ToGrid(position).Y + 1];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileLeft(Point position)
        {
            if (ToGrid(position).X > 0)
            {
                return map[ToGrid(position).X - 1, ToGrid(position).Y];
            }
            else
            {
                return null;
            }
        }

        public Tile GetTileRight(Point position)
        {
            if (ToGrid(position).X < Map.GetLength(0) - 1)
            {
                int x = ToGrid(position).X;
                int y = ToGrid(position).Y;

                return map[ToGrid(position).X + 1, ToGrid(position).Y];
            }
            else
            {
                return null;
            }
        }

        public Tile[] GetSurroundingTiles(Point pos)
        {
            Tile[] tileArray = { GetTileAbove(pos), GetTileRight(pos), GetTileBelow(pos), GetTileLeft(pos) };
            return tileArray;
        }

        public Point ToGrid(Point p)
        {
            int temp1 = p.X / 100;
            int temp2 = p.Y / 100;

            Point point = new Point((p.X / 100) - smallX, (p.Y / 100) - smallY);
            return point;
        }

        public Point GetPosition(Point p)
        {
            return new Point((p.X / 100) * 100, (p.Y / 100) * 100);
        }

        public void RemoveTile(Tile t)
        {
            tileList.Remove(t);
        }

        public Tile GetTileMea(int x, int y)
        {
            Point p = GetPosition(new Point(x, y));

            foreach (Tile t in tileList)
            {
                if (t.position == p)
                {
                    return t;
                }
            }
            return null;
        }

        public void AddTile(Tile t)
        {
            foreach (Tile tile in tileList)
            {
                if (t.position == tile.position)
                {
                    RemoveTile(tile);
                }
            }
            tileList.Add(t);
        }
    }
}
