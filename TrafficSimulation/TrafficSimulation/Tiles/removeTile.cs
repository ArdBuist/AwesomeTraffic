using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class removeTile : Tile
    {
        public removeTile()
        {
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRemoveTile(Graphics.FromImage(image));
            return image;
        }

        //returnt de banen die er bij een bepaalde kant uitgaan.
        public override int GetLanesIn(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

        //returnt de banen die er bij een bepaald kant ingaan.
        public override int GetLanesOut(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
        }

        //controleert of aan de aangegeven zijkant ook echt een weg ligt.
        public override bool doesConnect(int side)
        {
            bool noLanes = false;
            return noLanes;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
        }
        public override void SetValues(SimControl s, Point position)
        {
        }
    }
}
