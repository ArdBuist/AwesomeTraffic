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
        protected Tile[] adjacenttileList;
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
            adjacenttileList = new Tile[4];
            this.lanesHighToLow = 1;
            this.lanesLowToHigh = 1;
            this.listPlace = 1;
            this.size = new Size(100, 100);

            initialize(lanesHighToLow + lanesLowToHigh);
        }

        public void CarUpdate(SimControl sim)
        {
            foreach (List<Vehicle> list in vehicles)
            {
                foreach (Vehicle v in list)
                {
                    if (v.position.X - v.Speed >= this.position.X && v.position.X + v.Speed <= this.position.X + this.size.Width - v.Size.Width - v.Speed &&
                        v.position.Y - v.Speed >= this.position.Y && v.position.Y + v.Speed <= this.position.Y + this.size.Height - v.Size.Height - v.Speed)
                    {
                        v.Update();
                        sim.vehicleMap.AddObject(v.Bitmap, v.position.X, v.position.Y);
                    }
                    else
                    {

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

        //haalt andere tile uit de lijst. Vul direction in voor welke kant je op wilt.
        public Tile GetOtherTile(SimControl s, int direction)
        {
            Tile tile = null;
            try
            {
                switch (direction)
                {
                    case 1: tile = s.tileList[listPlace - s.tilesHorizontal];
                        break;
                    case 2: tile = s.tileList[listPlace + 1];
                        break;
                    case 3: tile = s.tileList[listPlace + s.tilesHorizontal];
                        break;
                    case 4: tile = s.tileList[listPlace - 1];
                        break;
                }
            }
            catch
            { }
            return tile;
        }
        public virtual void SetValues(SimControl s, Point position, int listPlace)
        {
            this.position.X = (position.X / 100) * 100;
            this.position.Y = (position.Y / 100) * 100;
            this.listPlace = listPlace;
            if (this.name == "Road")
            {
                Road thisTile = (Road)this;
                thisTile.UpdateFromOthertiles(s);
            }
        }
        public abstract void UpdateFromOthertiles(SimControl s);

        public abstract bool doesConnect(int side);

        public virtual int getLanesHighToLow(int side)
        {
            return lanesHighToLow;
        }
        public virtual int getLanesLowToHigh(int side)
        {
            return lanesLowToHigh;
        }
        public bool doesConnect(Tile tile, int side)
        {
            int direction = side + 2;
            if (direction > 4)
                direction -= 4;
            Boolean value = false;
            switch (tile.name)
            {
                case "Spawner": Spawner tileListpawner = (Spawner)tile;
                    if (tileListpawner.direction == direction)
                        value = true;
                    break;
                case "Crossroad": value = true;
                    break;
                case "Road": Road tileRoad = (Road)tile;
                    if (tileRoad.startDirection == direction || tileRoad.endDirection == direction)
                        value = true;
                    break;
                case "Fork": Fork tileFork = (Fork)tile;
                    if (tileFork.notDirection != direction)
                        value = true;
                    break;
                default: ;
                    break;
            }


            return value;
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

        public virtual void AddVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Add(v);
        }

        //Hierin worden verschillende variabelen van de tileList verandert en doorgegeven naar de aanliggende tileList.
        public abstract void Update(SimControl s, Tile tile, int direction);

        public void changeLane(Vehicle v, int begin, int eind)
        {
            RemoveVehicle(v, begin);
            AddVehicle(v, eind);
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
        private double carsPerSec;//auto's die per seconde gespawned worden
        private double numberOfCars;//opslag voor auto's die gespawned moeten worden voor als de weg vol is.
        public int direction;//kant waarop de weg loopt
        private int lanesOut, lanesIn;//aantal wegen van en naar de spawner
        private int spawnLane;//Lane waar de auto gespawnt gaat worden
        private double currentSpawn;//Nummer waarin word opgeslagen hoever de spawner is met het spawnen van een nieuwe auto
        private double spawnPerSec;//Aantal wat elke gametick bij de currentspawn word opgetelt

        public Spawner(int direction)
        {
            this.direction = direction;
            this.name = "Spawner";
            carsPerSec = 0.5;
            spawnLane = 1;
            this.lanesIn = 1;
            this.lanesOut = 1;
            this.spawnPerSec = 0.05;
            spawnLane = 0;
            currentSpawn = 0;
        }
        public override bool doesConnect(int side)
        {
            int direction = side + 2;
            if (direction > 4)
                direction -= 4;
            if (direction == this.direction)
                return true;
            return false;
        }
        public override void UpdateFromOthertiles(SimControl s)
        {
            Tile tile = GetOtherTile(s, direction);
            if (tile.name == "Road")
                Update(s, (Road)tile, direction);
        }

        public void Spawn(Vehicle v)
        {
            //nog onder voorbehoud, weet nog niet zeker of deze code werkt voor elke wegbreedte, werkt samen met Simulation.CreateVehicle
            currentSpawn--;
            vehicles[spawnLane].Add(v);
            spawnLane++;
            spawnLane = spawnLane % lanesOut;
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawSpawner(Graphics.FromImage(image), direction, lanesLowToHigh, lanesHighToLow);
            return image;
        }

        public void Tick()
        {
            currentSpawn += spawnPerSec;
        }

        public double CurrentSpawn { get { return currentSpawn; } }
        public int SpawnLane { get { return spawnLane; } }
        public int LanesOut { get { return lanesOut; } }


        public override void Update(SimControl s, Tile tile, int direction)
        {
            //if's voor verschil in richtingen van de wegen.
            Road road = (Road)tile;
            if (direction == this.direction)
            {
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
        }
    }

    public class Road : Tile
    {
        public int startDirection;
        public int endDirection;

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
        //kijkt of de wegen aan elkaar verbonden zijn
        public override bool doesConnect(int side)
        {
            int direction = side + 2;
            if (direction > 4)
                direction -= 4;
            if (direction == startDirection || direction == endDirection)
                return true;
            return false;
        }

        //update de tilevariabelen en zorgt dat tileList eromheen aangeroepen gaan worden.
        public override void Update(SimControl s, Tile tile, int direction)

        {
            Road road = (Road)tile;
            //road is alleen maar null als dit de eerste methode update is die wordt aangeroepen na een verandering in de interface.
            if (road != null && (this.startDirection == direction || this.endDirection == direction))
            {
                this.lanesHighToLow = road.getLaneHighToLow();
                this.lanesLowToHigh = road.getLaneLowToHigh();
                if (road.startDirection == 1 && road.endDirection == 2)
                {
                    if (direction == 4)
                    {
                        this.lanesHighToLow = road.getLaneLowToHigh();
                        this.lanesLowToHigh = road.getLaneHighToLow();
                    }
                }
                if (road.startDirection == 3 && road.endDirection == 4)
                {
                    if (direction == 1)
                    {
                        this.lanesHighToLow = road.getLaneLowToHigh();
                        this.lanesLowToHigh = road.getLaneHighToLow();
                    }
                }
                this.maxSpeed = road.getMaxSpeed();
            }
            //als het een rechte weg is
            if ((startDirection + endDirection) % 2 == 0)
            {
                if (direction == 0)
                {
                    UpdateOtherTile(s, endDirection);
                    UpdateOtherTile(s, startDirection);
                }
                else if (direction != startDirection)
                {
                    UpdateOtherTile(s, startDirection);
                }
                else
                {
                    UpdateOtherTile(s, endDirection);
                }
            }
            //als het een bocht is:
            else
            {
                if (direction == 0)
                {
                    UpdateOtherTile(s, startDirection);
                    UpdateOtherTile(s, endDirection);
                }
                else if (direction != startDirection)
                {
                    UpdateOtherTile(s, startDirection);
                }
                else
                {
                    UpdateOtherTile(s, endDirection);
                }
            }
            s.bitmapMap.AddObject(DrawImage(), position.X / 100, position.Y / 100);

        }
        public override void UpdateFromOthertiles(SimControl s)
        {
            Tile tile;
            tile = this.GetOtherTile(s, startDirection);
            if (tile != null && tile.doesConnect(startDirection) && tile.name == "Road")
            {
                Road otherRoad = (Road)tile;
                if ((startDirection + endDirection) % 2 == 0)
                {
                    this.lanesHighToLow = otherRoad.getLaneHighToLow();
                    this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                }
                //alle verschillende mogelijkheden voor de bochten
                else if ((startDirection == 1 && endDirection == 4) || (startDirection == 2 && endDirection == 3))
                {
                    this.lanesHighToLow = otherRoad.getLaneHighToLow();
                    this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                }
                else if (startDirection == 1)
                {
                    this.lanesHighToLow = otherRoad.getLaneHighToLow();
                    this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                }
                else if (startDirection == 3)
                {
                    this.lanesLowToHigh = otherRoad.getLaneHighToLow();
                    this.lanesHighToLow = otherRoad.getLaneLowToHigh();
                }
                this.maxSpeed = otherRoad.getMaxSpeed();
            }
            tile = this.GetOtherTile(s, endDirection);
            if (tile != null && tile.doesConnect(endDirection) && tile.name == "Road")
            {
                Road otherRoad = (Road)tile;
                //checken of er meer wegen zijn dan de andere kant.
                if (otherRoad.lanesHighToLow + otherRoad.lanesLowToHigh > this.lanesLowToHigh + this.lanesHighToLow)
                {
                    if ((startDirection + endDirection) % 2 == 0)
                    {
                        this.lanesHighToLow = otherRoad.getLaneHighToLow();
                        this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                    }
                    else if ((startDirection == 1 && endDirection == 4) || (startDirection == 2 && endDirection == 3))
                    {
                        this.lanesHighToLow = otherRoad.getLaneHighToLow();
                        this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                    }
                    else if (startDirection == 1)
                    {
                        this.lanesLowToHigh = otherRoad.getLaneHighToLow();
                        this.lanesHighToLow = otherRoad.getLaneLowToHigh();

                    }
                    else if (startDirection == 3)
                    {
                        this.lanesHighToLow = otherRoad.getLaneHighToLow();
                        this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                    }
                    this.maxSpeed = otherRoad.getMaxSpeed();
                }
            }
            this.Update(s, null, 0);
        }

        /*Deze methode zorgt ervoor dat van de tileList om deze tile heen de methode Update wordt aangeroepen.*/
        private void UpdateOtherTile(SimControl s, int direction)
        {
            Tile tile = GetOtherTile(s, direction);
            direction += 2;
            if (direction > 4)
                direction -= 4;
            if (tile != null)
                tile.Update(s, this, direction);
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
        public int notDirection;
        int[] lanes;
        public TrafficlightControl control;

        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            this.notDirection = notDirection;
            lanes[notDirection * 2 - 2] = 0;
            lanes[notDirection * 2 - 1] = 0;

            control = new TrafficlightControl(sim, this, 3, notDirection, lanes);

            int totalLanes = CountLanes(lanes);
            initialize(totalLanes);
        }

        public override bool doesConnect(int side)
        {
            int direction = side + 2;
            if (direction > 4)
                direction -= 4;
            if (direction != notDirection)
                return true;
            return false;
        }
        public override void UpdateFromOthertiles(SimControl s)
        {
            throw new NotImplementedException();
        }
        public override void Update(SimControl s, Tile tile, int direction)
        {
            if (direction != notDirection)
            {
                Road road = (Road)tile;
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
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(SimControl s, Point position, int listPlace)
        {
            base.SetValues(s,position, listPlace);

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
        public override bool doesConnect(int side)
        {
            return true;
        }
        public override void UpdateFromOthertiles(SimControl s)
        {
            for (int i = 1; i < 5; i++)
            {
                Tile tile = GetOtherTile(s, i);
                if (tile.name == "Road")
                { }
            }
        }
        public override void Update(SimControl s, Tile tile, int direction)
        {
            Road road = (Road)tile;
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

        public override void SetValues(SimControl s, Point position, int listPlace)
        {
            base.SetValues(s, position, listPlace);

            control.ChangeValues(position);
        }
    }
}
