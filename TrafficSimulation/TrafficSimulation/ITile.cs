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
        Bitmap image = new Bitmap(100,100);

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
        public override string ToString() { return "crossroad"; }
        TrafficlightControl tc;

        public Crossroad()
        {
            tc = new TrafficlightControl();
        }
        
        //Hier wordt het kruispunt getekend m.b.v. parameters die aangeven hoeveel wegen er in en uit gaan bij elke zijde.
        public Graphics drawCrossroad(int upIn, int upOut, int rightIn, int rightOut, int downIn, int downOut, int leftIn, int leftOut)
        {
            Graphics crossRoad = BitmapGraphics();

            // Er worden vier lijnen getekend: lineRU hoort bij de lijn rechtsboven, lineRD bij rechtsonder, lineLD bij linksonder, lineLU bij linksboven
             int lineRUx      = 50 + (10 * upOut);
             int lineRUy      = -1 * (50 - (10 * rightIn));
             int lineRUheight = 2 * (50 - (10 * rightIn));
             int lineRUwidth  = 2 * (50 - (10 * upOut));
             
             int lineRDx      = 50 + (10 * downIn);
             int lineRDy      = 50 + (10 * rightOut);
             int lineRDheight = 2 * (50 - (10 * rightOut));
             int lineRDwidth  = 2 * (50 - (10 * downIn));
             
             int lineLDx      = -1 * (50 - (10 * downOut));
             int lineLDy      = 50 + (10 * leftIn);
             int lineLDheight = 2 * (50 - (10 * leftIn));
             int lineLDwidth  = 2 * (50 - (10 * downOut));
              
             int lineLUx      = -1 * (50 - (10 * upIn));
             int lineLUy      = -1 * (50 - (10 * leftOut));
             int lineLUheight = 2 * (50 - (10 * leftOut));
             int lineLUwidth  = 2 * (50 - (10 * upIn));
                        
            crossRoad.DrawArc(Pens.Black,lineRUx,lineRUy,lineRUwidth,lineRUheight, 90,90);
            crossRoad.DrawArc(Pens.Black,lineRDx,lineRDy,lineRDwidth,lineRDheight, 180,90);
            crossRoad.DrawArc(Pens.Black,lineLDx,lineLDy,lineLDwidth,lineLDheight, 270,90);
            crossRoad.DrawArc(Pens.Black,lineLUx,lineLUy,lineLUwidth,lineLUheight, 0,90);

            return crossRoad;
        }
        

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
        public override string ToString() { return "fork"; }
        int notDirection;
        public Fork()
        {

        }

        public Graphics drawForkroad(int upIn, int upOut, int rightIn, int rightOut, int downIn, int downOut, int leftIn, int leftOut)
        {
            Graphics fork = BitmapGraphics();

            /*Er wordt een array aangemaakt met de vier inkomende wegen als elementen. (Elke kant heeft een inkomende en uitgaande weg of geen wegen
             * dus hoeft alleen in- of output gecheckt te worden.) Vervolgens wordt met een forloop gekeken aan welke kant er geen wegen zijn.
             * In count komt het nummer te staan van de kant die geen wegen heeft (0 is up, 1 is right, 2 is down, 3 is left.)
             */
            int [] sides = {upIn, rightIn, downIn, leftIn};
            int count = 0;
            for(int t=0; t<sides.Length;t++)
            {
                if(sides[t] == 0)
                {
                    count = t;
                    break;
                }
            }

            // Er worden 2 van de 4 booglijnen getekend: lineRU hoort bij de lijn rechtsboven, lineRD bij rechtsonder, lineLD bij linksonder, lineLU bij linksboven
            int lineRUx = 50 + (10 * upOut);
            int lineRUy = -1 * (50 - (10 * rightIn));
            int lineRUheight = 2 * (50 - (10 * rightIn));
            int lineRUwidth = 2 * (50 - (10 * upOut));

            int lineRDx = 50 + (10 * downIn);
            int lineRDy = 50 + (10 * rightOut);
            int lineRDheight = 2 * (50 - (10 * rightOut));
            int lineRDwidth = 2 * (50 - (10 * downIn));

            int lineLDx = -1 * (50 - (10 * downOut));
            int lineLDy = 50 + (10 * leftIn);
            int lineLDheight = 2 * (50 - (10 * leftIn));
            int lineLDwidth = 2 * (50 - (10 * downOut));

            int lineLUx = -1 * (50 - (10 * upIn));
            int lineLUy = -1 * (50 - (10 * leftOut));
            int lineLUheight = 2 * (50 - (10 * leftOut));
            int lineLUwidth = 2 * (50 - (10 * upIn));

            //Afhankelijk van welke kant geen wegen heeft, worden er 2 bogen en een lijn getekend.
            if(count == 0)
            {
                fork.DrawArc(Pens.Black, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
                fork.DrawArc(Pens.Black, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawLine(Pens.Black, 0, (50 - 10 * leftOut), 100, (50 - 10 * rightIn));
            }

            else if(count == 1)
            {
                fork.DrawArc(Pens.Black, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawArc(Pens.Black, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);
                fork.DrawLine(Pens.Black, (50 + 10 * upOut), 0, (50 + 10 * downIn), 100);
            }

            else if(count == 2)
            {
                fork.DrawArc(Pens.Black, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);
                fork.DrawArc(Pens.Black, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
                fork.DrawLine(Pens.Black, 0, (50 + 10 * leftIn), 100, (50 + 10 * rightOut));
            }

            else
            {
                fork.DrawArc(Pens.Black, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
                fork.DrawArc(Pens.Black, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
                fork.DrawLine(Pens.Black, (50 - 10 * upIn), 0, (50 - 10 * downOut), 100);
            }

            return fork;
        }

    }
    public class Road: Roads
    {

        public override string ToString() { return "road"; }
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
