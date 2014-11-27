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
        protected bool[] access;
        protected int TotalVehicleLength;
        protected Size size;
        protected string name;

        public Tile(Point position, int maxSpeed)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            adjacentTiles = new Tile[4];
        }

        public void initialize(int totalLanes)
        {
            vehicles = new List<Vehicle>[totalLanes];
            access = new bool[totalLanes];
            for (int i = 0; i < totalLanes; i++)
            {
                vehicles[i] = new List<Vehicle>();
                access[i] = true;
            }
        }
        //hierin worden in de andere klassen de bitmaps gemaakt voor de kaart
        public abstract Bitmap DrawImage();

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
            //deze hele methode moet nog gemaakt worden (De weg moet zelf een update krijgen en de aanliggende tiles moeten een update krijgen als er wat in deze weg verandert is)
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

        public string Number
        {
            get { return name; }
        }
    }

    public class Spawner : Tile
    {
        private int direction;
        private int spawnLane;
        private int currentSpawn;
        private double carsPerSec;
        private double numberOfCars;

        public Spawner(Point position, int maxSpeed, int spawnLane, int direction, double carsPerSec)
            : base(position, maxSpeed)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Spawner";

            initialize(spawnLane + 1);
        }

        private void spawnVehicle()
        {
            numberOfCars += carsPerSec;
            if (numberOfCars >= 1)
            {
                //true moet vervangen worden door de methode waarin wordt bepaald of er een auto gespawned kan worden
                if (true)
                {
                    AddVehicle(createVehicle(), currentSpawn);
                    currentSpawn = (currentSpawn + 1);

                }
                else
                {
                    //hierzo moet hij iets doen als er niet gespawnt kan worden, bijv het bijhouden of een queue maken ofzo
                }
                numberOfCars = numberOfCars % 1;
            }
        }
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            //drawForkroad(Graphics.FromImage(image), 0, 0, 1, 1, 1, 1, 1, 1);
            return image;
        }

        private Vehicle createVehicle()
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new Vehicle(new Point(), new Point(), 0, 0, 0, 0);
        }
    }

    public class Road : Tile
    {
        private int startDirection;
        private int endDirection;
        private int totalLanes;
        private int lanesLowToHigh, lanesHighToLow;

        public Road(Point position, int maxSpeed, int lanesLowToHigh, int lanesHighToLow, int start, int end)
            : base(position, maxSpeed)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Road";
            this.lanesHighToLow = lanesHighToLow;
            this.lanesLowToHigh = lanesLowToHigh;

            if (start < end)
            {
                this.startDirection = start;
                this.endDirection = end;
            }
            else
            {
                this.startDirection = end;
                this.endDirection = start;
            }
            initialize(lanesLowToHigh + lanesHighToLow);
        }
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            //drawForkroad(Graphics.FromImage(image), 0, 0, 1, 1, 1, 1, 1, 1);
            return image;
        }
    }

    public class Fork : Tile
    {
        private int notDirection;
        private int lanes1, lanes2, lanes3;
        private List<TrafficlightControl> trafficlightControlList;

        public Fork(Point position, int maxSpeed, int lanes1, int lanes2, int lanes3, int notDirection)
            : base(position, maxSpeed)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Fork";
            this.lanes1 = lanes1;
            this.lanes2 = lanes2;
            this.lanes3 = lanes3;
            this.notDirection = notDirection;

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
            initialize(lanes1 + lanes2 + lanes3);
        }
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            drawForkroad(Graphics.FromImage(image), 0,0,1,1,1,1,1,1);
            return image;
        }
        public Graphics drawForkroad(Graphics gr, int upIn, int upOut, int rightIn, int rightOut, int downIn, int downOut, int leftIn, int leftOut)
        {
            Graphics fork = gr;

            /*Er wordt een array aangemaakt met de vier inkomende wegen als elementen. (Elke kant heeft een inkomende en uitgaande weg of geen wegen
             * dus hoeft alleen in- of output gecheckt te worden.) Vervolgens wordt met een forloop gekeken aan welke kant er geen wegen zijn.
             * In count komt het nummer te staan van de kant die geen wegen heeft (0 is up, 1 is right, 2 is down, 3 is left.)
             */
            int[] sides = { upIn, rightIn, downIn, leftIn };
            int count = 0;
            for (int t = 0; t < sides.Length; t++)
            {
                if (sides[t] == 0)
                {
                    count = t + 1;
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
            if (count == 1)
            {
                fork.DrawArc(Pens.Black, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
                fork.DrawArc(Pens.Black, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawLine(Pens.Black, 0, (50 - 10 * leftOut), 100, (50 - 10 * rightIn));
            }

            else if (count == 2)
            {
                fork.DrawArc(Pens.Black, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawArc(Pens.Black, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);
                fork.DrawLine(Pens.Black, (50 + 10 * upOut), 0, (50 + 10 * downIn), 100);
            }

            else if (count == 3)
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

    public class Crossroad : Tile
    {
        private int totalLanes;
        private int lanes1, lanes2, lanes3, lanes4;
        private List<TrafficlightControl> trafficlightControlList;

        public Crossroad(Point position, int maxSpeed, int lanes1, int lanes2, int lanes3, int lanes4)
            : base(position, maxSpeed)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Crossroad";
            this.lanes1 = lanes1;
            this.lanes2 = lanes2;
            this.lanes3 = lanes3;
            this.lanes4 = lanes4;

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 4; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
            initialize(lanes1 + lanes2 + lanes3 + lanes4);
        }
        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            drawCrossroad(Graphics.FromImage(image), 1, 1, 1, 1, 1, 1, 1, 1);//deze variabelen moeten nog echt variabel worden.
            return image;
        }
        //Hier wordt het kruispunt getekend m.b.v. parameters die aangeven hoeveel wegen er in en uit gaan bij elke zijde.
        public Graphics drawCrossroad(Graphics gr, int upIn, int upOut, int rightIn, int rightOut, int downIn, int downOut, int leftIn, int leftOut)
        {
            Graphics crossRoad = gr;

            // Er worden vier lijnen getekend: lineRU hoort bij de lijn rechtsboven, lineRD bij rechtsonder, lineLD bij linksonder, lineLU bij linksboven
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

            crossRoad.DrawArc(Pens.Black, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
            crossRoad.DrawArc(Pens.Black, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
            crossRoad.DrawArc(Pens.Black, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
            crossRoad.DrawArc(Pens.Black, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);

            return crossRoad;
        }
    }
}
