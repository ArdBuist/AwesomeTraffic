using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Fork : Tile
    {
        public int[] lanes;
        public TrafficlightControl control;
        public int notDirection;

        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            this.notDirection = notDirection;
            lanes[notDirection * 2 - 2] = 0;
            lanes[notDirection * 2 - 1] = 0;
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            directions.Remove(notDirection);
            control = new TrafficlightControl(sim, this, 3, notDirection, lanes);
            int totalLanes = CountLanes(lanes);
            Initialize();
        }


        //public int NotDirection { get { return NotDirection; } }

        public override int GetLanesIn(int direction)
        {
            int value;
            value = lanes[direction * 2 - 2];
            if (value != 0)
                return value;
            else
                return 1;
        }

        public override int GetLanesOut(int direction)
        {
            int value;
            value = lanes[direction * 2 - 1];
            if (value != 0)
                return value;
            else
                return 1;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (directions.Contains(direction))
            {
                lanes[direction * 2 - 1] = lanesOut;
                lanes[direction * 2 - 2] = lanesIn;
            }
            control = new TrafficlightControl(s, this, 3, NotDirection, lanes, position);
        }

        public override TrafficlightControl GetControl()
        {
            return control;
        }

        public override bool doesConnect(int side)
        {
            if ((side + 1) % 4 + 1 != NotDirection)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {

            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);

            control.ChangeValues(position);
        }
    }
}
