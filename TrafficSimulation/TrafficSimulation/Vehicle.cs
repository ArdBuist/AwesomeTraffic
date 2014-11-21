using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Vehicle
    {
        Point position;
        Point destination;
        int length;
        int speed;
        int direction;
        int lane;

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            position = pos;
            destination = dest;
            length = len;
            this.speed = speed;
            this.direction = direction;
            this.lane = lane;
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
            : base(pos, dest, len, speed, direction,lane)
        {
        }
    }
}
