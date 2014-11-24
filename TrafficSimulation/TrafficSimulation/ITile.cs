using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TrafficSimulation
{
    public abstract class Tile
    {
        protected List<Vehicle>[] vehicles;
        protected Tile[] adjacentTiles;
        protected Point position;
        protected int maxSpeed;
        protected int lanes;
        protected bool[] acces;
        protected int TotalVehicleLength;
        protected Size size;
        protected Bitmap image;

        public Tile(Point position, int maxSpeed, int lanes)
        {
            vehicles = new List<Vehicle>[lanes * 2];
            adjacentTiles = new Tile[4];

            this.position = position;
            this.maxSpeed = maxSpeed;
            this.lanes = lanes;
        }

        public void RemoveVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Remove(v);
        }

        public void AddVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Add(v);
        }

        public void Update()
        {

        }

        public void changeLane(Vehicle v, int begin, int eind)
        {
            RemoveVehicle(v, begin);
            AddVehicle(v, eind);
        }

        public void addAdjacent(Tile t)
        {
            for (int i = 0; i < adjacentTiles.Length; i++)
            {
                if (adjacentTiles[i] == null)
                {
                    adjacentTiles[i] = t;
                    break;
                }
            }
        }

        public void removeAdjacent(int direction)
        {
            adjacentTiles[direction] = null;
        }

        public Graphics BitmapGraphics()
        {
            Graphics gr = Graphics.FromImage(image);
            return gr;
        }
    }
    public class Crossroad : Tile
    {
        public override string ToString() { return "CrossRoad"; }

        List<TrafficlightControl> trafficlightControlList;

        public Crossroad(Point position, int maxSpeed, int lanes)
            : base(position, maxSpeed, lanes)
        {
            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 4; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
        }
    }
    public class Fork : Tile
    {
        private int notDirection;
        List<TrafficlightControl> trafficlightControlList;

        public Fork(Point position, int maxSpeed, int lanes, int notDirection)
            : base(position, maxSpeed, lanes)
        {
            this.notDirection = notDirection;

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
        }


        public override string ToString() { return "Fork"; }
    }
    public class Road : Tile
    {
        private int startDirection;
        private int eindDirection;

        public Road(Point position, int maxSpeed, int lanes, int start, int end)
            : base(position, maxSpeed, lanes)
        {
            for (int i = 0; i < lanes * 2; i++)
            {
                vehicles[i] = new List<Vehicle>();
            }

            if (start < end)
            {
                startDirection = start;
                eindDirection = end;
            }
            else
            {
                startDirection = end;
                eindDirection = start;
            }
        }
    }
    public class Spawner : Tile
    {
        public override string ToString() { return "Road"; }

        int direction;
        double carsPerSec;
        double numberOfCars;

        public Spawner(Point position, int maxSpeed, int lanes, int direction)
            : base(position, maxSpeed, lanes)
        {
            this.direction = direction;
        }

        private void createVehicle()
        {

        }
    }
}
