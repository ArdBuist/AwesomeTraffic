using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Crossroad : Tile
    {
        int[] lanes;
        public TrafficlightControl control;

        public Crossroad(SimControl sim)
        {
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            control = new TrafficlightControl(sim, this, 4, 5, lanes);
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            int totalLanes = CountLanes(lanes);
            Initialize();
        }

        public override int GetLanesIn(int direction)
        {
            return lanes[direction * 2 - 2];
        }

        public override int GetLanesOut(int direction)
        {
            return lanes[direction * 2 - 1];
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            lanes[direction * 2 - 1] = lanesOut;
            lanes[direction * 2 - 2] = lanesIn;
            control = new TrafficlightControl(s, this, 4, 5, lanes, position);
        }

        public override TrafficlightControl GetControl()
        {
            return control;
        }

        public override bool doesConnect(int side)
        {
            return true;
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawCrossroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);
            control.ChangeValues(position);
        }
    }
}
