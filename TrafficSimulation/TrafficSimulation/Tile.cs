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
        //all vehicles on the Tile
        public List<List<Vehicle>> [] vehicles;
        //list with adjacent tiles
        protected Tile[] adjacenttileList;
        //position of the tile on the grid
        public Point position;
        //maximum speed allowed on the tile
        public int maxSpeed;
        //is true if vehicles can enter the specific lane
        protected bool[,] access;
        //size of the tile, (100,100)
        public Size size;
        //name of the type of tile
        public string name;
        //de kant van een fork waar geen weg heengaat
        public int notDirection;
        //begin en eind van een weg
        public int startDirection;
        public int endDirection;
        //kant waarnaar de weg loopt bij een spawner
        public int direction;
        //number of lanes from the lowest direction to the highest direction       
        protected int lanesLowToHigh;
        //number of lanes from the highest direction to the lowest direction
        protected int lanesHighToLow;

        protected int numberOfVehicles;
        //
        protected List<int> directions;
        //
        List<Vehicle> removedVehicles;

        public Tile()
        {
            this.MaxSpeed = 2;
            adjacenttileList = new Tile[4];
            this.LanesHighToLow = 1;
            this.LanesLowToHigh = 1;
            this.size = new Size(100, 100);
            removedVehicles = new List<Vehicle>();
            directions = new List<int>();
            Initialize();
        }
        public List<int> Directions
        {
            get { return this.directions; }
        }

        public int NumberOfVehicles
        {
            get { return numberOfVehicles; }
            set { numberOfVehicles = value; }
        }
        public bool[,] Access
        {
            get { return access; }
            set { access = value; }
        }
        public int LanesHighToLow
        {
            get { return this.lanesHighToLow; }
            set { lanesHighToLow = value; }
        }

        public int LanesLowToHigh
        {
            get { return this.lanesLowToHigh; }
            set { lanesLowToHigh = value; }
        }
        public int MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public void Initialize()
        {
            vehicles = new List<List<Vehicle>>[4];
            access = new bool[4,3];
            for (int i = 0; i < 4; i++)
            {
                vehicles[i] = new List<List<Vehicle>>();
                for (int j = 0; j < 3; j++)
                {
                    access[i, j] = true;
                    vehicles[i].Add(new List<Vehicle>());
                }

            }
        }

        //hierin worden in de andere klassen de bitmaps gemaakt voor de kaart
        public abstract Bitmap DrawImage();

        /*komende methodes zijn voor het tekenen en updaten van de waardes van de tiles.*/


        public void UpdateFromOtherTile(SimControl simcontrol,int direction)
        {
            
            if(direction == 0)
            {
                int ForkNotDirection = 0;
                foreach(int d in directions)
                {
                    if (d != ForkNotDirection)
                    {
                        Tile nextTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[d - 1];
                        if (nextTile != null && nextTile.doesConnect(d)) 
                        {
                            this.UpdateLanes(simcontrol, d, nextTile.GetLanesOut((d + 1) % 4 + 1), nextTile.GetLanesIn((d + 1) % 4 + 1));
                            if(this.name == "Road")
                            {
                                Road roadTile = (Road)this;
                                int CounterDirection;
                                if (d == roadTile.startDirection)
                                    CounterDirection = endDirection;
                                else
                                    CounterDirection = startDirection;
                                Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[CounterDirection - 1];
                                if (otherTile != null)
                                {
                                    if (this.GetLanesOut(CounterDirection) != otherTile.GetLanesIn((CounterDirection + 1) % 4 + 1) || otherTile.GetLanesOut((CounterDirection + 1) % 4 + 1) != this.GetLanesIn(CounterDirection))
                                    {
                                        otherTile.UpdateLanes(simcontrol, (CounterDirection + 1) % 4 + 1, this.GetLanesOut(CounterDirection), this.GetLanesIn(CounterDirection));
                                        otherTile.UpdateOtherTiles(simcontrol, (CounterDirection + 1) % 4 + 1);
                                        break;
                                    }
                                }
                            }
                            if (this.name == "Fork")
                            {
                                Fork forkTile = (Fork)this;
                                if ((d == (forkTile.NotDirection + 2) % 4 + 1 || d == (forkTile.NotDirection) % 4 + 1))
                                {
                                    ForkNotDirection = (d + 1) % 4 + 1;
                                    forkTile.UpdateLanes(simcontrol, ForkNotDirection, this.GetLanesOut(d), this.GetLanesIn(d));
                                    Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[ForkNotDirection - 1];
                                    if (otherTile != null)
                                    {
                                        otherTile.UpdateLanes(simcontrol, d, this.GetLanesIn(d), this.GetLanesOut(d));
                                        otherTile.UpdateOtherTiles(simcontrol, d);
                                    }

                                }
                            }
                        }
                        
                    }
                }
            }
            simcontrol.backgroundBC.AddObject(this.DrawImage(), this.position);
        }

        public void UpdateOtherTiles(SimControl simcontrol, int NotDirection)
        {
            if (this.name != "Crossroad")
            {
                if (this.name != "Fork")
                {
                    foreach (int d in directions)
                    {
                        if (d != NotDirection)
                        {
                            int CounterDirection = (d + 1) % 4 + 1;
                            Tile nextTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[d - 1];
                            if (nextTile != null)
                            {
                                if (this.GetLanesOut(d) != nextTile.GetLanesIn(CounterDirection) || nextTile.GetLanesOut(CounterDirection) != this.GetLanesIn(d))
                                {
                                    nextTile.UpdateLanes(simcontrol, CounterDirection, this.GetLanesOut(d), this.GetLanesIn(d));
                                    nextTile.UpdateOtherTiles(simcontrol, CounterDirection);
                                }
                            }
                        }
                    }
                }
                else
                {
                    Fork forkTile = (Fork)this;
                    if ((NotDirection == (forkTile.NotDirection + 2) % 4 + 1 || NotDirection == (forkTile.NotDirection) % 4 + 1))
                    {
                        int ForkNotDirection = (NotDirection + 1) % 4 + 1;
                        forkTile.UpdateLanes(simcontrol, ForkNotDirection, this.GetLanesOut(NotDirection), this.GetLanesIn(NotDirection));
                        Tile otherTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[ForkNotDirection - 1];
                        if (otherTile != null)
                        {
                            otherTile.UpdateLanes(simcontrol, NotDirection, this.GetLanesIn(NotDirection), this.GetLanesOut(NotDirection));
                            otherTile.UpdateOtherTiles(simcontrol, NotDirection);
                        }

                    }
                }
            }
            simcontrol.backgroundBC.AddObject(this.DrawImage(), this.position);
        }

        //krijgt een aantal banen die binnenkomen en eruit moeten gaan voor een bepaald richting. Tile moet dat dan in zijn gegevens verwerken.
        public abstract void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut);

        //returnt de banen die er bij een bepaalde kant uitgaan.
        public abstract int GetLanesIn(int direction);

        //returnt de banen die er bij een bepaald kant ingaan.
        public abstract int GetLanesOut(int direction);

        //controleert of aan de aangegeven zijkant ook echt een weg ligt.
        public abstract bool doesConnect(int side);

		public abstract TrafficlightControl GetControl();

        //returnt de tile die aan één van de vier zijkanten van de tile ligt.
        
        //verwerkt de waarden die verkregen worden als de tile op de kaart wordt geplaatst.
        public virtual void SetValues(SimControl s, Point position)
        {
            this.position.X = (position.X / 100) * 100;
            this.position.Y = (position.Y / 100) * 100;
            this.UpdateFromOtherTile(s, 0);
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

        public void RemoveVehicle(SimControl sim, Vehicle v, int Side, int lane)
        {
            List<List<Vehicle>> sideVehicles = vehicles[Side-1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Remove(v);
            numberOfVehicles--;
            //looks if there is space for other cars to come on the tile
                if (laneVehicles.Count < 5 && this.name != "Spawner"&&this.name!="Crossroad" && this.name!="Fork")
                {
                    Tile lastTile = sim.simulationMap.GetSurroundingTiles(this.position)[(Side + 1) % 4];
                    lastTile.Access[Side - 1, lane] = true;
                }
        }

        public void AddVehicle(SimControl sim, Vehicle v, int Side, int lane)
        {
            List<List<Vehicle>> sideVehicles = vehicles[Side-1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Add(v);
            numberOfVehicles++;
            //looks if the tile is full
            if (laneVehicles.Count > 5 && this.name != "Spawner"&&this.name!="Crossroad" && this.name!="Fork")
            {
                Tile lastTile = sim.simulationMap.GetSurroundingTiles(this.position)[(Side + 1) % 4];
                lastTile.Access[Side - 1, lane] = false;
            }
        }

    }

    public class Spawner : Tile
    {
        private int carsSpawnChance;//De kans dat een auto gespawned wordt.
        private double numberOfCars;//opslag voor auto's die gespawned moeten worden voor als de weg vol is.
        private int lanesOut, lanesIn;//aantal wegen van en naar de spawner
        private int spawnLane;//Lane waar de auto gespawnt gaat worden
        private double currentSpawn;//Nummer waarin word opgeslagen hoever de spawner is met het spawnen van een nieuwe auto
        private double spawnPerSec;//Aantal wat elke gametick bij de currentspawn word opgetelt
        protected static System.Security.Cryptography.RNGCryptoServiceProvider rnd;//om de auto's op willekeurige tijden te laten spawnen

        public Spawner(SimControl sim, int direction)
        {
            this.direction = direction;
            this.name = "Spawner";
            carsSpawnChance = 3;
            spawnLane = 0;
            this.lanesIn = 1;
            this.lanesOut = 1; 
            this.spawnPerSec = 0.05;
            directions.Add(direction);
            currentSpawn = 1;
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
            
            
        }

        public double CurrentSpawn { get { return currentSpawn; } }
        public int SpawnLane { get { return spawnLane; } }
        public int CarsSpawnChance { get { return carsSpawnChance; } set { carsSpawnChance = value; } }

        public override int GetLanesIn(int direction)
        {
            if (this.direction == direction)
                return lanesIn;
            else if (this.direction == (direction + 1) % 4 + 1)
                return lanesOut;
            else
                return 1;
        }

        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);

            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawnerBlock(Graphics.FromImage(image), direction, lanesOut, lanesIn);
            s.trafficlightBC.AddObject(image, this.position);
        }
        public override int GetLanesOut(int direction)
        {
            if (this.direction == direction)
                return lanesOut;
            else if(this.direction == (direction+1)%4+1)
                return lanesIn;
            else
                return 1;
        }

		public override TrafficlightControl GetControl()
		{
			throw new NotImplementedException();
		}

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == this.direction)
            {
                this.lanesIn = lanesIn;
                this.lanesOut = lanesOut;
            }
        }

        public void Tick(SimControl sim,int extraSpeed,double extraTime)
        {
            currentSpawn += (spawnPerSec*(extraSpeed+1))+((extraTime/50)*spawnPerSec);

            if (currentSpawn >= 1)
            {
                Spawn(sim);
            }
        }

        public void Spawn(SimControl sim)
        {
            //spawnt op een willekeurig moment een auto in een willekeurige baan.
            Byte[] random;
            random = new Byte[1];
            rnd.GetBytes(random);
            if (random[0] % carsSpawnChance == 0)
            {
                //takes a random lane to spawn in
                spawnLane = ((random[0]*10)/8) % lanesOut;
                List<List<Vehicle>> vehicleList = vehicles[this.direction - 1];
                if (vehicleList[spawnLane].Count < 4)
                {
                    AddVehicle(sim, createVehicle(spawnLane), direction, spawnLane);
                    sim.totalCars++;
                }
            }
            currentSpawn--;
        }

        public Vehicle createVehicle(int spawnLane)
        {
			return new NormalCar(this.position, this.position, 10, this.maxSpeed, this.direction, spawnLane);
        }

        public override bool doesConnect(int side)
        {   
            if ((side + 1) % 4 + 1 == this.direction)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawner(Graphics.FromImage(image), direction, lanesOut, lanesIn);
            return image;
        }
    }

    public class Road : Tile
    {

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
            Initialize();
        }

        public override int GetLanesIn(int direction)
        {
            if (direction == startDirection)
                return lanesLowToHigh;
            else if (direction == endDirection)
                return lanesHighToLow;
            //else if (direction == startDirection)
            //    return lanesLowToHigh;
            //else if (direction == endDirection)
            //    return lanesHighToLow;
            else
                return 1;
        }

        public override int GetLanesOut(int direction)
        {
            //if ((direction + 1) % 4 + 1 == startDirection)
            //    return lanesLowToHigh;
            //else if ((direction + 1) % 4 + 1 == endDirection)
            //    return lanesHighToLow;
            if (direction == startDirection)
                return lanesHighToLow;
            else if (direction == endDirection)
                return lanesLowToHigh;
            else
                return 1;      
        }

		public override TrafficlightControl GetControl()
		{
			throw new NotImplementedException();
		}

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == startDirection)
            {
                this.lanesLowToHigh = lanesIn;
                this.lanesHighToLow = lanesOut;
            }
            if (direction == endDirection)
            {
                
                
                this.lanesLowToHigh = lanesOut;
                this.lanesHighToLow = lanesIn;
            }
        }

        //kijkt of de wegen aan elkaar verbonden zijn
        public override bool doesConnect(int side)
        {
            int direction = (side + 1) % 4 + 1;
            if (direction == startDirection || direction == endDirection)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRoad(Graphics.FromImage(image), lanesLowToHigh, lanesHighToLow, startDirection, endDirection);
            return image;
        }
    }

    public class Fork : Tile
    {
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
            Initialize();
        }

        public int NotDirection { get { return notDirection; } }

        public override int GetLanesIn(int direction)
        {
            int value;
            value = lanes[direction * 2 - 2];
            if (value != 0)
                return value;
            else
                return 1;
        }

        public override int GetLanesOut(int direction)
        {
            int value;
            value = lanes[direction * 2 - 1];
            if (value != 0)
                return value;
            else
                return 1;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (directions.Contains(direction))
            {
                lanes[direction * 2 - 1] = lanesOut;
                lanes[direction * 2 - 2] = lanesIn;   
            }
            control = new TrafficlightControl(s, this, 3, notDirection, lanes, position);
        }

		public override TrafficlightControl GetControl()
		{
			return control;
		}

        public override bool doesConnect(int side)
        {
            if ((side + 1) % 4 + 1 != notDirection)
                return true;
            return false;
        }

        public override Bitmap DrawImage()
        {

            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawForkroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);

            control.ChangeValues(position);
        }
    }

    public class Crossroad : Tile
    {
        int[] lanes;
        public TrafficlightControl control;

        public Crossroad(SimControl sim)
        {
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            control = new TrafficlightControl(sim, this, 4, 5, lanes);
            directions.Add(1);
            directions.Add(2);
            directions.Add(3);
            directions.Add(4);
            int totalLanes = CountLanes(lanes);
            Initialize();
        }

        public override int GetLanesIn(int direction)
        {
            return lanes[direction * 2 - 2];
        }

        public override int GetLanesOut(int direction)
        {
            return lanes[direction * 2 - 1];
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            lanes[direction * 2 - 1] = lanesOut;
            lanes[direction * 2 - 2] = lanesIn;
            control = new TrafficlightControl(s, this, 4, 5, lanes, position);
        }

		public override TrafficlightControl GetControl()
		{
			return control;
		}

        public override bool doesConnect(int side)
        {
            return true;
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawCrossroad(Graphics.FromImage(image), lanes);
            return image;
        }

        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);
            control.ChangeValues(position);
        }
    }

    public class SelectTile : Tile
    {
        public SelectTile()
        {
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSelectTile(Graphics.FromImage(image));
            return image;
        }

        //returnt de banen die er bij een bepaalde kant uitgaan.
        public override int GetLanesIn(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

        //returnt de banen die er bij een bepaald kant ingaan.
        public override int GetLanesOut(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

		public override TrafficlightControl GetControl()
		{
			throw new NotImplementedException();
		}

        //controleert of aan de aangegeven zijkant ook echt een weg ligt.
        public override bool doesConnect(int side)
        {
            bool noLanes = false;
            return noLanes;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
        }
        
    }

    public class removeTile : Tile
    {
        public removeTile()
        {
        }

        public override Bitmap DrawImage()
        { // hier wordt een bitmap gemaakt en getekend door de andere methode. 
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawRemoveTile(Graphics.FromImage(image));
            return image;
        }

        //returnt de banen die er bij een bepaalde kant uitgaan.
        public override int GetLanesIn(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

        //returnt de banen die er bij een bepaald kant ingaan.
        public override int GetLanesOut(int direction)
        {
            int noLanes = 0;
            return noLanes;
        }

		public override TrafficlightControl GetControl()
		{
			throw new NotImplementedException();
		}

        //controleert of aan de aangegeven zijkant ook echt een weg ligt.
        public override bool doesConnect(int side)
        {
            bool noLanes = false;
            return noLanes;
        }

        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
        }
        public override void SetValues(SimControl s, Point position)
        {
        }
    }
}
