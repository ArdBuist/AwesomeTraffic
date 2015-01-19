using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    static class Methods
    {
        public static bool CheckValidConnections(SimControl s)
        {
            foreach(Tile t in s.tileList)
            {
                if(t!=null)
                    foreach(int direction in t.Directions)
                    {
                        Tile OtherTile = Methods.GetOtherTile(s,t, direction);
                        if (OtherTile == null || !OtherTile.doesConnect(direction))
                            return false;
                    }
            }
            return true;
        }

        public static Tile GetOtherTile(SimControl s,Tile startTile, int direction)
        {
            Tile tile = null;
            try
            {
                switch (direction)
                {
                    case 1: tile = s.tileList[startTile.listPlace - s.tilesHorizontal];
                        break;
                    case 2: tile = s.tileList[startTile.listPlace + 1];
                        break;
                    case 3: tile = s.tileList[startTile.listPlace + s.tilesHorizontal];
                        break;
                    case 4: tile = s.tileList[startTile.listPlace - 1];
                        break;
                }
            }
            catch(Exception e)
            {

            }
            return tile;
        }
        public static bool TileConnectionisValid(SimControl simcontrol, int listplace,Tile currentBuildTile)
        {
            currentBuildTile.listPlace = listplace;
            if (currentBuildTile.name == "Crossroad" || currentBuildTile.name == "Fork")
            {
                foreach (int direction in currentBuildTile.Directions)
                {
                    if (Methods.GetOtherTile(simcontrol, currentBuildTile, direction) != null && (Methods.GetOtherTile(simcontrol, currentBuildTile, direction).name == "Crossroad" || Methods.GetOtherTile(simcontrol,currentBuildTile, direction).name == "Fork"))
                        return false;
                }
            }
            return true;
        }
    }
}
