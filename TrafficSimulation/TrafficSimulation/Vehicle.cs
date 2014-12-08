using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TrafficSimulation
{
    public class Vehicle
    {
        private Point position;
        private Point destination;
        private int length;
        private int width;
        private int speed;
        private int direction;
        private int lane;

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

        public Point Destination
        {
            get { return destination; }
        }

        public int Direction
        {
            get { return direction; }
        }
        public int Lane
        {
            get { return lane; }
        }
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
        NormalCar(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {

        }
    }

    public class Truck : Vehicle
    {
        Truck(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {

        }
    }
}
