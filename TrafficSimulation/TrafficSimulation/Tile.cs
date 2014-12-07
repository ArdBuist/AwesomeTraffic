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
        public string name;
        protected int lanesLowToHigh;
        protected int lanesHighToLow;
        protected int listPlace;

        public Tile()
        {
            this.maxSpeed = 5;
            adjacentTiles = new Tile[4];
            this.lanesHighToLow = 1;
            this.lanesLowToHigh = 1;
            this.listPlace = 1;
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

        public void SetValues(Point position, int listPlace)
        {
            this.position = position;
            this.listPlace = listPlace;
        }
        public int CountLanes(int[] lanes)
        {
            int totalLanes = 0;
            for (int i = 0; i < lanes.Length; i++)
            {
                totalLanes += lanes[i];
            }
            return totalLanes;
        }

        public void RemoveVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Remove(v);
        }

        public void AddVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Add(v);
        }

        //Hierin worden verschillende variabelen van de tiles verandert en doorgegeven naar de aanliggende tiles.
        public abstract void Update(SimControl s, Road road, int direction);

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
        public void setMaxSpeed(int i)
        {
            this.maxSpeed = i;
        }
        public void setLanesHighToLow(int i)
        {
            this.lanesLowToHigh = i;
        }
        public void setLanesLowToHigh(int i)
        {
            this.lanesLowToHigh = i;
        }
    }

    public class Spawner : Tile
    {
        private int spawnLane;//handigheid niet duidelijk voor mij
        private int currentSpawn;//baan waarop de volgende auto gespawnt gaat worden
        private int direction;//kant waarop de weg loopt
        private int lanesOut, lanesIn;//aantal wegen van en naar de spawner
        private double carsPerSec;//auto's die per seconde gespawned worden
        private double numberOfCars;//opslag voor auto's die gespawned moeten worden voor als de weg vol is.

        public Spawner(int direction)
        {
            this.direction = direction;
            this.name = "Spawner";
            this.lanesIn = 1;
            this.lanesOut = 1;
            carsPerSec = 0.5;
            spawnLane = 1;

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
            DrawTile t = new DrawTile();
            t.drawSpawner(Graphics.FromImage(image), direction, lanesOut, lanesIn);
            return image;
        }

        private Vehicle createVehicle()
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new Vehicle(new Point(), new Point(), 0, 0, 0, 0);
        }
        public override void Update(SimControl s,Road road,int direction)
        {
            //if's voor verschil in richtingen van de wegen.
            if(direction<3)
            {
                this.lanesIn = road.getLaneLowToHigh();
                this.lanesOut = road.getLaneHighToLow();
            }
            else
            {
                this.lanesIn = road.getLaneHighToLow();
                this.lanesOut = road.getLaneLowToHigh();
            }
            
            this.maxSpeed = road.getMaxSpeed();
            s.bitmapMap.AddTile(DrawImage(), position.X / 100, position.Y / 100);
        }
        public Graphics drawSpawner(Graphics gr, int side, int lanesIn, int lanesOut)
        {
            Graphics bmSpawner = gr;

            //aanmaken pen die in 1 lijn streepjes zet van 5 px per stuk
            float[] stripesLine = new float[20];

            for (int t = 0; t < stripesLine.Length; t++)
            {
                stripesLine[t] = 5;
            }

            Pen stripesPen = new Pen(Color.Black);
            stripesPen.DashPattern = stripesLine;

            //variabelen voor mogelijke wegen
            int r = 50 - 10 * lanesOut;
            int r2 = 50 + 10 * lanesOut;
            int r3 = 50 - 10 * lanesIn;
            int r4 = 50 + 10 * lanesIn;
            int width = 10 * lanesIn + 10 * lanesOut + 10;

            /*Verschillende plaatjes voor verschillende kanten. Spawner is 30 px hoog
             * en 5px breder aan beide kanten van de wegen.
             **/
            if (side == 1)
            {
                bmSpawner.DrawLine(Pens.Black, r3, 0, r3, 70);
                bmSpawner.DrawLine(stripesPen, 50, 0, 50, 70);
                bmSpawner.DrawLine(Pens.Black, r2, 0, r2, 70);
                bmSpawner.FillRectangle(Brushes.Black, r3 - 5, 70, width, 30);
            }

            else if (side == 2)
            {
                bmSpawner.DrawLine(Pens.Black, 30, r, 100, r);
                bmSpawner.DrawLine(stripesPen, 30, 50, 100, 50);
                bmSpawner.DrawLine(Pens.Black, 30, r4, 100, r4);
                bmSpawner.FillRectangle(Brushes.Black, 0, r3 - 5, 30, width);
            }

            else if (side == 3)
            {
                bmSpawner.DrawLine(Pens.Black, r3, 30, r3, 100);
                bmSpawner.DrawLine(stripesPen, 50, 30, 50, 100);
                bmSpawner.DrawLine(Pens.Black, r2, 30, r2, 100);
                bmSpawner.FillRectangle(Brushes.Black, r3 - 5, 0, width, 30);
            }

            else
            {
                bmSpawner.DrawLine(Pens.Black, 0, r, 70, r);
                bmSpawner.DrawLine(stripesPen, 0, 50, 70, 50);
                bmSpawner.DrawLine(Pens.Black, 0, r4, 70, r4);
                bmSpawner.FillRectangle(Brushes.Black, 70, r - 5, 30, width);
            }

            return bmSpawner;
        }
    }

    public class Road : Tile
    {
        private int startDirection;
        
        private int endDirection;


        public Road( int start, int end)
        {
            this.name = "Road";

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
            DrawTile t = new DrawTile();
            t.drawRoad(Graphics.FromImage(image),lanesLowToHigh,lanesHighToLow,startDirection,endDirection);
            return image;
        }
        //update de tilevariabelen en zorgt dat tiles eromheen aangeroepen gaan worden.
        public override void Update(SimControl s,Road road, int direction)
        {
            //road is alleen maar null als dit de eerste methode update is die wordt aangeroepen na een verandering in de interface.
            if (road != null)
            {
                this.lanesHighToLow = road.getLaneHighToLow();
                this.lanesLowToHigh = road.getLaneLowToHigh();
                this.maxSpeed = road.getMaxSpeed();
            }
            else
            {
                /*hier moet nog code komen om de waarden van aanliggende tiles over te kunnen nemen*/
                //Tile tile = s.tiles[listPlace];
                //if (s.tiles[listPlace] != null && tile.name == "Road")
                //{
                //    Road Tile = (Road)tile;
                //    this.lanesHighToLow = Tile.getLaneHighToLow();
                //    this.lanesLowToHigh = Tile.getLaneLowToHigh();
                //    this.maxSpeed = Tile.getMaxSpeed();
                //}
            }
            //als het een rechte weg is
            if ((startDirection + endDirection) % 2 == 0)
            {
                if (direction == 0)
                {
                    UpdateOtherTile(s, this, endDirection);
                    UpdateOtherTile(s, this, startDirection);
                }
                else if (direction != startDirection)
                {
                    UpdateOtherTile(s, this, endDirection);
                }
                else
                {
                    UpdateOtherTile(s, this, startDirection);
                }
            }
            //als het een bocht is:
            else
            {
                if(direction == 0)
                {
                    UpdateOtherTile(s, this, startDirection + 2);
                    UpdateOtherTile(s, this, endDirection + 2);
                }
                else if(direction != startDirection)
                {
                    UpdateOtherTile(s, this, startDirection + 2);
                }
                else
                {
                    UpdateOtherTile(s, this, endDirection + 2);
                }
            }
            
            s.bitmapMap.AddTile(DrawImage(),position.X/100,position.Y/100);

        }

        /*Deze methode zorgt ervoor dat van de tiles om deze tile heen de methode Update wordt aangeroepen.
        hiervoor zoekt hij in de list van SimControl naar de goede tile.*/
        private void UpdateOtherTile(SimControl s, Road road, int direction)
        {
            Tile tile;
            switch(direction)
            {
                case 1: tile = s.tiles[listPlace + s.tilesHorizontal];
                    if (tile != null)
                        tile.Update(s, this, 1);
                    break;
                case 2: tile = s.tiles[listPlace - 1];
                    if (tile != null)
                        tile.Update(s, this, 2);
                    break;
                case 3: tile = s.tiles[listPlace - s.tilesHorizontal];
                    if (tile != null)
                        tile.Update(s, this, 3);
                    break;
                case 4: tile = s.tiles[listPlace + 1];
                    if(tile != null)
                        tile.Update(s, this, 4);
                    break;
            }
        }
        public int getLaneHighToLow()
        {
            return lanesHighToLow;
        }
        public int getLaneLowToHigh()
        {
            return lanesLowToHigh;
        }
        public int getMaxSpeed()
        {
            return maxSpeed;
        }

        public int[] getValues()
        {
            int[] values = new int[3];
            values[0] = maxSpeed;
            values[1] = lanesHighToLow;
            values[2] = lanesLowToHigh;
            return values;
        }

    }

    public class Fork : Tile
    {
        private int notDirection;
        int[] lanes;
        private List<TrafficlightControl> trafficlightControlList;

        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] {1,1,1,1,0,0,1,1};
            this.notDirection = notDirection;

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl(sim,this,3,notDirection,lanes));
            }
            int totalLanes = CountLanes(lanes);
                initialize(totalLanes);
        }
        public override void Update(SimControl s, Road road, int direction)
        {
            this.maxSpeed = road.getMaxSpeed();
            if (direction < 3)
            {
                lanes[direction * 2 - 2] = road.getLaneLowToHigh();
                lanes[direction * 2 - 1] = road.getLaneHighToLow();
            }
            else
            {
                lanes[direction * 2 - 2] = road.getLaneHighToLow();
                lanes[direction * 2 - 1] = road.getLaneLowToHigh();
            }
            s.bitmapMap.AddTile(DrawImage(), position.X / 100, position.Y / 100);//drawmethode werkt nog niet naar behoren door ontbreken compatibiliteit met lists
        }
        
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }
        
    }

    public class Crossroad : Tile
    {
        int[] lanes;
        private List<TrafficlightControl> trafficlightControlList;

        public Crossroad(SimControl sim)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 4; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl(sim, this, 4, 0, lanes));
            }
            int totalLanes = CountLanes(lanes);
            initialize(totalLanes);
        }
        public override void Update(SimControl s, Road road, int direction)
        {
            this.maxSpeed = road.getMaxSpeed();
            if (direction < 3)
            {
                lanes[direction * 2 - 2] = road.getLaneLowToHigh();
                lanes[direction * 2 - 1] = road.getLaneHighToLow();
            }
            else
            {
                lanes[direction * 2 - 2] = road.getLaneHighToLow();
                lanes[direction * 2 - 1] = road.getLaneLowToHigh();
            }
            s.bitmapMap.AddTile(DrawImage(), position.X / 100, position.Y / 100);//drawmethode werkt nog niet naar behoren door ontbreken compatibiliteit met lists
        }
        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawCrossroad(Graphics.FromImage(image), lanes);
            return image;
        }
        
    }
}
