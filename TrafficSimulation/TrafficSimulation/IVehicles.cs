using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public interface IVehicles
    {
        Point Position;
        Point Destination;
        int Length;
        int Direction;
        
    }
    public class Car : IVehicles
    {

    }
    public class Truck : IVehicles
    {

    }
}
