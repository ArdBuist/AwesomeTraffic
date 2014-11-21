using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    interface ITile
    {
        void RemoveVehicle();
        void AddVehicle();
        void Update();
    }

    public abstract class Roads : ITile
    {
        List<List<Vehicle>> Vehicles;
        Point position;
        int maxSpeed;
        int lanes;
        bool[] acces;
        int TotalVehicleLength;
        Size size;
        Bitmap image;

        public Roads(Point position)
        {
            Vehicles = new List<List<Vehicle>>();
            this.position = position;
        }

        public virtual void RemoveVehicle(Vehicle v)
        {

        }
        public virtual void AddVehicle(Vehicle v)
        {

        }
        public virtual void Update();
        public Graphics BitmapGraphics()
        {
            Graphics gr = Graphics.FromImage(image);
            return gr;
        }
    }
    public class Crossroad : Roads
    {
        public override string ToString() { return "CrossRoad"; }

        List<TrafficlightControl> trafficlightControlList;

        Crossroad(Point position):base(position){
            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
        }

        public override void RemoveVehicle(Vehicle v)
        {
            base.RemoveVehicle(v);
        }
        public override void AddVehicle(Vehicle v)
        {
            base.AddVehicle(v);
        }
        public override void Update()
        {
            base.Update();
        }
    }
    public class Fork : Crossroad
    {
        public override string ToString() { return "Fork"; }
        int notDirection;
    }
    public class Road : Roads
    {
        List<TrafficlightControl> trafficlightControlList;
        int startDirection;
        int eindDirection;

        Road(Point position, int start, int end):base(position){
            trafficlightControlList = new List<TrafficlightControl>();
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

            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
        }

        public override void RemoveVehicle(Vehicle v)
        {
            base.RemoveVehicle(v);
        }
        public override void AddVehicle(Vehicle v)
        {
            base.AddVehicle(v);
        }
        public override void Update()
        {
            base.Update();
        }
        private void makeLists()
        {
        }
        private void changeLane()
        {
        }
    }
    public class Spawner : Road
    {
        public override string ToString() { return "Road"; }
        double carsPerSec;
        double numberOfCars;

        private void createVehicle()
        {
        }
    }
    public class CurvedRoad : Road
    {
        public override string ToString() { return "Curve"; }
    }
    public class StraightRoad : Road
    {
        public override string ToString() { return "Straight"; }
    }
}
