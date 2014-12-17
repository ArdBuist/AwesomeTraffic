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
        protected List<int> directions;
        List<Vehicle> removedVehicles;

        public Tile()
        {
            this.MaxSpeed = 5;
            adjacenttileList = new Tile[4];
            this.LanesHighToLow = 1;
            this.LanesLowToHigh = 1;
            this.listPlace = 1;
            this.size = new Size(100, 100);
            removedVehicles = new List<Vehicle>();
            directions = new List<int>();
            initialize(lanesHighToLow + lanesLowToHigh);
        }

        public int LanesHighToLow{get{return this.lanesHighToLow;}set{lanesHighToLow = value;}}
        public int LanesLowToHigh { get { return this.lanesLowToHigh; } set { lanesLowToHigh= value; } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

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

        /*komende methodes zijn voor het tekenen en updaten van de waardes van de tiles.*/

        //wordt gebruikt als de tile geplaatst wordt, of een aanliggende tile veranderd wordt.
        public  void UpdateFromOtherTile(SimControl s, int direction)
        {
            int ForkDirection = 0;
            if(direction == 0)
            {
                foreach(int d in directions)
                {
                    Tile tile = GetOtherTile(s, d);
                    if (tile != null && d != ForkDirection)
                    {
                        UpdateLanes(s,d, tile.GetLanesIn(direction), tile.GetLanesOut(direction));
                        //Als aan de lange kant bij een Fork aan één kant de banen worden veranderd moet dat natuurlijk ook aan de andere kant gebeuren
                        if (name == "Fork")
                        {
                            Fork fork = (Fork)this;
                            //Als de verandering van een kant van de lange kant komt
                            if ((d == (fork.NotDirection + 2) % 4 + 1 || d == (fork.NotDirection) % 4 + 1))
                            {
                                ForkDirection = (d + 1) % 4 + 1;
                                UpdateLanes(s,(d + 1) % 4 + 1, tile.GetLanesOut(direction), tile.GetLanesIn(direction));
                                //Een verkorte versie van UpdateOtherTiles, hier hoeft namelijk alleen de andere kant van de lange kant geupdate worden.
                                Tile tile1 = GetOtherTile(s, ForkDirection);
                                if(tile1!= null)
                                tile1.UpdateFromOtherTile(s, ForkDirection);
                            }
                        }
                    }
                }
                s.bitmapMap.AddObject(DrawImage(), position.X, position.Y);
            }
            else
            {
                int notSide = (direction + 1) % 4 + 1;
                Tile tile = GetOtherTile(s, notSide);
                if (tile.name != "Crossroad"  )
                {
                    if (tile.name == "Fork")
                    {
                        UpdateLanes(s,notSide,  tile.GetLanesOut(direction),tile.GetLanesIn(direction));
                        UpdateOtherTiles(s, notSide);
                    }
                    else
                    {
                        UpdateLanes(s,notSide, tile.GetLanesIn(direction), tile.GetLanesOut(direction));
                        UpdateOtherTiles(s, notSide);
                    }
                    
                }
            }
        }
        //roept een tile aan als hij veranderd is, om ook de andere tiles te laten veranderen.
        public void UpdateOtherTiles(SimControl s, int NotDirection)
        {
            if (name != "Fork")
            {
                foreach (int d in directions)
                {
                    if (d != NotDirection)
                    {
                        Tile tile = GetOtherTile(s, d);
                        if (tile != null && tile.doesConnect(d))
                            tile.UpdateFromOtherTile(s, d);
                    }
                }
            }
            else
            {
                //een Fork is speciaal, want aan de lange kant moet de hoeveelheid banen doorlopen terwijl er nog een derde uitgang is.
                Fork fork = (Fork)this;
                if(NotDirection == (fork.NotDirection+2)%4+1 || NotDirection == (fork.NotDirection)%4+1)
                {
                    //de hoeveelheid banen wordt hier van de ene kant van de Fork naar de andere kant overgeheveld.
                    fork.lanes[((NotDirection + 1) % 4 + 1) * 2 - 1] = fork.lanes[NotDirection * 2 - 2];
                    fork.lanes[((NotDirection + 1) % 4 + 1) * 2 - 2] = fork.lanes[NotDirection * 2 - 1];
                    int d = (NotDirection+1)%4+1;
                    Tile tile = GetOtherTile(s, d);
                    if (tile != null && tile.doesConnect(d))
                        tile.UpdateFromOtherTile(s, d);
                }
                
                
            }
            s.bitmapMap.AddObject(DrawImage(), position.X, position.Y);
        }
        //krijgt een aantal banen die binnenkomen en eruit moeten gaan voor een bepaald richting. Tile moet dat dan in zijn gegevens verwerken.
        public abstract void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut);

        //returnt de banan die er bij een bepaalde kant uitgaan.
        public abstract int GetLanesIn(int direction);

        //returnt de banen die er bij een bepaald kant ingaan.
        public abstract int GetLanesOut(int direction);

        //controleert of aan de aangegeven zijkant ook echt een weg ligt.
        public abstract bool doesConnect(int side);

        //returnt de tile die aan één van de vier zijkanten van de tile ligt.
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
        //verwerkt de waarden die verkregen worden als de tile op de kaart wordt geplaatst.
        public virtual void SetValues(SimControl s, Point position, int listPlace)
        {
            this.position.X = (position.X / 100) * 100;
            this.position.Y = (position.Y / 100) * 100;
            this.listPlace = listPlace;
            this.UpdateFromOtherTile(s,0);
        }

        //telt alle banen zodat voor elke baan een list voor de auto's aangemaakt kan worden.
        public int CountLanes(int[] lanes)
        {
            int totalLanes = 0;
            for (int i = 0; i < lanes.Length; i++)
            {
                totalLanes += lanes[i];
            }
            return totalLanes;
        }
        
        /*komende methodes zijn voor het laten rijden van de auto's*/
        public void changeLane(Vehicle v, int begin, int eind)
        {
            RemoveVehicle(v, begin);
            AddVehicle(v, eind);
        }

        public void RemoveVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Remove(v);
        }

        public virtual void AddVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Add(v);
        }

        public void CarUpdate(SimControl sim)
        {
            foreach (List<Vehicle> list in vehicles)
            {
                for (int i = 0; i < list.Count - 1; i++)
                {
                    if (list[i] != null)
                    {
                        Vehicle v = list[i];
                        {
                            if (v.position.X - v.Speed >= this.position.X && v.position.X + v.Speed <= this.position.X + this.size.Width - v.Size.Width - v.Speed &&
                                v.position.Y - v.Speed >= this.position.Y && v.position.Y + v.Speed <= this.position.Y + this.size.Height - v.Size.Height - v.Speed)
                            {
                                v.Update();
                                sim.vehicleMap.AddObject(v.Bitmap, v.position.X, v.position.Y);
                            }
                            else
                            {
                                //list[i] = null;
                                list.Remove(v);
                                //Tile tile = this.GetOtherTile(sim, v.Direction);
                                //if(tile != null)
                                //{
                                //    tile.AddVehicle(v, v.Lane);
                                //}
                            }
                        }
                    }
                }
            }
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
            spawnLane = 0;
            this.lanesIn = 1;
            this.lanesOut = 1;
            this.spawnPerSec = 0.05;
            directions.Add(direction);
            currentSpawn = 1;
        }

        public double CurrentSpawn { get { return currentSpawn; } }
        public int SpawnLane { get { return spawnLane; } }
        public int LanesOut { get { return lanesOut; } }
        public int LanesIn { get { return lanesIn; }}

        public override int GetLanesIn(int direction)
        {
            return lanesOut;
        }

        public override int GetLanesOut(int direction)
        {
            return lanesIn;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == this.direction)
            {
                this.lanesIn = lanesIn;
                this.lanesOut = lanesOut;
            }
        }
   
        public void Tick()
        {
            currentSpawn += spawnPerSec;
        }

        public void Spawn(Vehicle v)
        {
            //nog onder voorbehoud, weet nog niet zeker of deze code werkt voor elke wegbreedte, werkt samen met Simulation.CreateVehicle
            spawnLane++;
            spawnLane = spawnLane % lanesOut;
            currentSpawn--;
            vehicles[spawnLane].Add(v);
            
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

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawSpawner(Graphics.FromImage(image), direction, lanesOut, lanesIn);
            return image;
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
            directions.Add(startDirection);
            directions.Add(endDirection);
            initialize(lanesLowToHigh + lanesHighToLow);
        }
        
        public override int GetLanesIn(int direction)
        {
            if (direction == startDirection)
                return lanesHighToLow;
            else
                return lanesLowToHigh;
        }

        public override int GetLanesOut(int direction)
        {
            if (direction == startDirection)
                return lanesLowToHigh;
            else
                return lanesHighToLow;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if(direction == startDirection)
            {
                this.lanesLowToHigh = lanesIn;
                this.lanesHighToLow = lanesOut;
            }
            if(direction == endDirection)
            {
                this.lanesLowToHigh = lanesOut;
                this.lanesHighToLow = lanesIn;
            }
        }

        //kijkt of de wegen aan elkaar verbonden zijn
        public override bool doesConnect(int side)
        {
            int direction = (side+1)%4 + 1;
            if (direction == startDirection || direction == endDirection)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile t = new DrawTile();
            t.drawRoad(Graphics.FromImage(image), lanesLowToHigh, lanesHighToLow, startDirection, endDirection);
            return image;
        }
    }

    public class Fork : Tile
    {
        protected int notDirection;
        public int[] lanes;
        public TrafficlightControl control;

        public Fork(SimControl sim, int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };
            this.notDirection = notDirection;
            lanes[notDirection * 2 - 2] = 0;
            lanes[notDirection * 2 - 1] = 0;
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            directions.Remove(notDirection);
            control = new TrafficlightControl(sim, this, 3, notDirection, lanes);
            int totalLanes = CountLanes(lanes);
            initialize(totalLanes);
        }

        public int NotDirection { get { return notDirection; } }

        public override int GetLanesIn(int direction)
        {
            int thisSide = (direction + 1) % 4 + 1;
            return lanes[thisSide * 2 - 1];
        }

        public override int GetLanesOut(int direction)
        {
            int thisSide = (direction + 1) % 4 + 1;
            return lanes[thisSide * 2 - 2];
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (directions.Contains(direction))
            {
                lanes[direction * 2 - 1] = lanesOut;
                lanes[direction * 2 - 2] = lanesIn;
                control = new TrafficlightControl(s, this, 3, notDirection, lanes, position);
            }
        }

        public override bool doesConnect(int side)
        {
            int direction = (side+1)%4+1;
            if (direction != notDirection)
                return true;
            return false;
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
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            int totalLanes = CountLanes(lanes);
            initialize(totalLanes);
        }

        public override int GetLanesIn(int direction)
        {
            int thisSide = (direction + 1) % 4 + 1;
            return lanes[thisSide * 2 - 1];
        }

        public override int GetLanesOut(int direction)
        {
            int thisSide = (direction + 1) % 4 + 1;
            return lanes[thisSide * 2 - 2];
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (directions.Contains(direction))
            {
                lanes[direction * 2 - 1] = lanesOut;
                lanes[direction * 2 - 2] = lanesIn;
                control = new TrafficlightControl(s, this, 4, 5, lanes, position);
            }
        }

        public override bool doesConnect(int side)
        {
            return true;
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
