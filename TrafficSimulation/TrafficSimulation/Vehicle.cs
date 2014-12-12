using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;

namespace TrafficSimulation
{
    public abstract class Vehicle
    {
        public Point position;
        protected Point destination;
        protected Bitmap bitmap;
        protected int length;
        protected int width;
        protected int speed;
        protected int direction;
        protected int lane;

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            position = pos;
            destination = dest;
            length = len;
            width = 10;
            this.speed = speed;
            this.direction = direction;
            this.lane = lane;


        }

        public Point Destination { get { return destination; } }
        public int Direction { get { return direction; } }
        public int Lane { get { return lane; } }
        public Bitmap Bitmap { get { return bitmap; } }

        public void Update()
        {
            if (speed != 0)
            {
                switch (direction)
                {
                    case 1:
                        position.Y -= speed;
                        break;
                    case 2:
                        position.X += speed;
                        break;
                    case 3:
                        position.Y += speed;
                        break;
                    case 4:
                        position.X -= speed;
                        break;
                }
            }
        }
    }

    public class NormalCar : Vehicle
    {
        public NormalCar(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {
            /*ResourceManager rm = Properties.Resources.ResourceManager;
            this.bitmap = (Bitmap)rm.GetObject("car1");*/
            this.bitmap = new Bitmap(10, 14);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    bitmap.SetPixel(i, j, Color.Red);
                }
            }
        }
    }

    public class Truck : Vehicle
    {
        Truck(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {
            /*ResourceManager rm = Properties.Resources.ResourceManager;
            this.bitmap = (Bitmap)rm.GetObject("truck1");*/
        }
    }
}
