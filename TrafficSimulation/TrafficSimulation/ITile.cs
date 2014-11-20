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
        List<List<Car>> numberOfVehicles;
        Point position;
        int maxSpeed;
        int lanes;
        bool[] acces;
        int TotalVehicleLength;
        Size size;
        Bitmap image;

        public virtual void RemoveVehicle()
        {
        }
        public virtual void AddVehicle()
        {
        }
        public virtual void Update()
        {
        }
        public Graphics BitmapGraphics()
        {
            Graphics gr = Graphics.FromImage(image);
            return gr;
        }
    }
    public class Crossroad : Roads
    {
        public override string ToString() { return "CrossRoad"; }
        TrafficlightControl tc;

        public override void RemoveVehicle()
        {
            base.RemoveVehicle();
        }
        public override void AddVehicle()
        {
            base.AddVehicle();
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
    public class Road: Roads
    {
        int startDirection;
        int eindDirection;
        public override void RemoveVehicle()
        {
            base.RemoveVehicle();
        }
        public override void AddVehicle()
        {
            base.AddVehicle();
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
