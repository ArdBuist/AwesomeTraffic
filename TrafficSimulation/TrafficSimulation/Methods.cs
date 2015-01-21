using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    static class Methods
    {
        public static bool CheckValidConnections(SimControl s)
        {
            s.simulationMap.CreateMap();
            foreach (Tile t in s.simulationMap.GetMap())
            {
                foreach (int direction in t.Directions)
                {
                    if (s.simulationMap.GetSurroundingTiles(t.position)[direction - 1] == null)
                    {
                        Tile[] tile = s.simulationMap.GetSurroundingTiles(t.position);
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool TileConnectionisValid(SimControl simcontrol, Tile currentBuildTile,Point tilePosition)
        {
            if (currentBuildTile.name == "Crossroad" || currentBuildTile.name == "Fork")
            {
                foreach (Tile t in simcontrol.simulationMap.GetSurroundingTiles(new Point((tilePosition.X/100)*100,(tilePosition.Y/100)*100)))
                {
                    if (t != null && (t.name.Equals("Fork") || t.name.Equals("Crossroad")))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
