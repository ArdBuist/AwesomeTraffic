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
        public List<Vehicle>[] vehicles;
        protected Tile[] adjacentTiles;
        public Point position;
        public int maxSpeed;
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

            initialize(lanesHighToLow + lanesLowToHigh);
        }

        public void CarUpdate()
        {
            foreach (List<Vehicle> list in vehicles)
            {
                if (list != null)
                {
                    foreach (Vehicle v in list)
                    {
                        v.Update();
                    }
                }
            }
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

        public virtual void SetValues(Point position, int listPlace)
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
        public int direction;//kant waarop de weg loopt
        private int lanesOut, lanesIn;//aantal wegen van en naar de spawner
        private int spawnLane;//Lane waar de auto gespawnt gaat worden
        private double currentSpawn;//Nummer waarin word opgeslagen hoever de spawner is met het spawnen van een nieuwe auto
        private double spawnPerSec;//Aantal wat elke gametick bij de currentspawn word opgetelt

        public Spawner(int direction)
        {
            this.direction = direction;
            this.name = "Spawner";
            this.lanesIn = 1;
            this.lanesOut = 1;
            this.spawnPerSec = 0.05;

            spawnLane = 1;
            currentSpawn = 0;
        }


          


        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawSpawner(Graphics.FromImage(image), direction, lanesOut, lanesIn);
            return image;
        }

        public void Tick()
        {
            currentSpawn += spawnPerSec;
        }

        public void Spawn()
        {
            currentSpawn--;
        }

        public double CurrentSpawn { get { return currentSpawn; } }
        public int SpawnLane { get { return spawnLane; } }


        public override void Update(SimControl s, Road road, int direction)
        {
            //if's voor verschil in richtingen van de wegen.
            if (direction < 3)
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
            s.bitmapMap.AddObject(DrawImage(), position.X / 100, position.Y / 100);
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


        public Road(int start, int end)
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
            t.drawRoad(Graphics.FromImage(image), lanesLowToHigh, lanesHighToLow, startDirection, endDirection);
            return image;
        }
        //update de tilevariabelen en zorgt dat tiles eromheen aangeroepen gaan worden.
        public override void Update(SimControl s, Road road, int direction)
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
                if (direction == 0)
                {
                    UpdateOtherTile(s, this, startDirection + 2);
                    UpdateOtherTile(s, this, endDirection + 2);
                }
                else if (direction != startDirection)
                {
                    UpdateOtherTile(s, this, startDirection + 2);
                }
                else
                {
                    UpdateOtherTile(s, this, endDirection + 2);
                }
            }
            s.bitmapMap.AddObject(DrawImage(), position.X / 100, position.Y / 100);

        }

        /*Deze methode zorgt ervoor dat van de tiles om deze tile heen de methode Update wordt aangeroepen.
        hiervoor zoekt hij in de list van SimControl naar de goede tile.*/
        private void UpdateOtherTile(SimControl s, Road road, int direction)
        {
            Tile tile;
            switch (direction)
            {
                case 1: tile = s.tileList[listPlace + s.tilesHorizontal];
                    if (tile != null)
                        tile.Update(s, this, 1);
                    break;
                case 2: tile = s.tileList[listPlace - 1];
                    if (tile != null)
                        tile.Update(s, this, 2);
                    break;
                case 3: tile = s.tileList[listPlace - s.tilesHorizontal];
                    if (tile != null)
                        tile.Update(s, this, 3);
                    break;
                case 4: tile = s.tileList[listPlace + 1];
                    if (tile != null)
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
        public TrafficlightControl control;

        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] { 1, 1, 1, 1, 0, 0, 1, 1 };
            this.notDirection = notDirection;

            control = new TrafficlightControl(sim, this, 3, notDirection, lanes);

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

            s.bitmapMap.AddObject(DrawImage(), position.X / 100, position.Y / 100);//drawmethode werkt nog niet naar behoren door ontbreken compatibiliteit met lists
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(Point position, int listPlace)
        {
            base.SetValues(position, listPlace);

            control.ChangeValues(position);
        }
    }

    public class Crossroad : Tile
    {
        int[] lanes;
        public TrafficlightControl control;

        public Crossroad(SimControl sim)
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            control = new TrafficlightControl(sim, this, 4, 5, lanes);

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
            control = new TrafficlightControl(s, this, 4, 5, lanes, position);
            s.bitmapMap.AddObject(DrawImage(), position.X / 100, position.Y / 100);//drawmethode werkt nog niet naar behoren door ontbreken compatibiliteit met lists
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawCrossroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(Point position, int listPlace)
        {
            base.SetValues(position, listPlace);

            control.ChangeValues(position);
        }
    }
}
