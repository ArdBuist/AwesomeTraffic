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
        List<List<IVehicles>> numberOfVehicles;
        Point position;
        int maxSpeed;
        int lanes;
        bool[] acces;
        int TotalVehicleLength;
        Size size;
        void RemoveVehicle();
        void AddVehicle();
        void Update();
    }
    public class Crossroad : ITile
    {
        public override string ToString() { return "CrossRoad"; }
        TrafficlightControl tc;
    }
    public class Fork : Crossroad
    {
        public override string ToString() { return "Fork"; }
        int notDirection;
    }
    public class Road: ITile
    {
        int startDirection;
        int eindDirection;
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
