using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Road : Tile
    {

        public Road(int start, int end)
        {
            this.name = "Road";

            if (start < end)
            {
                directions.Add(start);
                directions.Add(end);
            }
            else
            {
                directions.Add(end);
                directions.Add(start);
            }
            
            Initialize();
        }

        public override int GetLanesIn(int direction)
        {
            if (direction == StartDirection)
                return lanesLowToHigh;
            else if (direction == EndDirection)
                return lanesHighToLow;
            //else if (direction == startDirection)
            //    return lanesLowToHigh;
            //else if (direction == endDirection)
            //    return lanesHighToLow;
            else
                return 1;
        }

        public override int GetLanesOut(int direction)
        {
            //if ((direction + 1) % 4 + 1 == startDirection)
            //    return lanesLowToHigh;
            //else if ((direction + 1) % 4 + 1 == endDirection)
            //    return lanesHighToLow;
            if (direction == StartDirection)
                return lanesHighToLow;
            else if (direction == EndDirection)
                return lanesLowToHigh;
            else
                return 1;
        }

        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == StartDirection)
            {
                this.lanesLowToHigh = lanesIn;
                this.lanesHighToLow = lanesOut;
            }
            if (direction == EndDirection)
            {


                this.lanesLowToHigh = lanesOut;
                this.lanesHighToLow = lanesIn;
            }
        }

        //kijkt of de wegen aan elkaar verbonden zijn
        public override bool doesConnect(int side)
        {
            int direction = (side + 1) % 4 + 1;
            if (direction == StartDirection || direction == EndDirection)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRoad(Graphics.FromImage(image), lanesLowToHigh, lanesHighToLow, StartDirection, EndDirection);
            return image;
        }
    }
}
