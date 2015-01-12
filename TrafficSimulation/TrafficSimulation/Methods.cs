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
                        Tile OtherTile = t.GetOtherTile(s, direction);
                        if (OtherTile == null || !OtherTile.doesConnect(direction))
                            return false;
                    }
            }
            return true;
        }
        public static int CalculateListPlace(SimControl s,int mouseX, int mouseY)
        {
            return mouseY / 100 * s.tilesHorizontal + mouseX / 100;
        }
    }
}
