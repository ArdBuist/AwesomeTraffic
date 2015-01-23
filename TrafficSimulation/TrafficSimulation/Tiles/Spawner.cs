using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class Spawner : Tile
    {
        private int carsSpawnChance;//De kans dat een auto gespawned wordt.
        private double numberOfCars;//opslag voor auto's die gespawned moeten worden voor als de weg vol is.
        private int lanesOut, lanesIn;//aantal wegen van en naar de spawner
        private int spawnLane;//Lane waar de auto gespawnt gaat worden
        private double currentSpawn;//Nummer waarin word opgeslagen hoever de spawner is met het spawnen van een nieuwe auto
        private double spawnPerSec;//Aantal wat elke gametick bij de currentspawn word opgetelt
        protected static System.Security.Cryptography.RNGCryptoServiceProvider rnd;//om de auto's op willekeurige tijden te laten spawnen

        /// <summary>
        /// Constructor used by fork, based on the constructor in Tile
        /// here all default values of the Spawner are set.
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="direction"></param>
        public Spawner(SimControl sim, int direction)
        {
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

        /// <summary>
        /// Based on the method GetLanesIn in Spawner
        /// returns the lanes going into the tile in the specified direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesIn(int direction)
        {
            if (this.Direction == direction)
                return lanesIn;
            else if (this.Direction == (direction + 1) % 4 + 1)
                return lanesOut;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method SetValues in Tile
        /// sets the values given to the Spawner when the Spawner is placed on the map
        /// </summary>
        /// <param name="s"></param>
        /// <param name="position"></param>
        public override void SetValues(SimControl s, Point position)
        {
            base.SetValues(s, position);

            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawnerBlock(Graphics.FromImage(image), Direction, lanesOut, lanesIn);
            s.trafficlightBC.AddObject(image, this.position);
        }

        /// <summary>
        /// Based on the method GetLanesOut in Spawner
        /// returns the lanes going out of the tile in the specified direction
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override int GetLanesOut(int direction)
        {
            if (this.Direction == direction)
                return lanesOut;
            else if (this.Direction == (direction + 1) % 4 + 1)
                return lanesIn;
            else
                return 1;
        }

        /// <summary>
        /// Based on the method GetControl in Tile
        /// returns the TrafficLightControl used by the Spawner
        /// </summary>
        /// <returns></returns>
        public override TrafficlightControl GetControl()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Based on the method UpdateLanes in Spawner
        /// this method is called when the lanes are updated.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="direction"></param>
        /// <param name="lanesIn"></param>
        /// <param name="lanesOut"></param>
        public override void UpdateLanes(SimControl s, int direction, int lanesIn, int lanesOut)
        {
            if (direction == this.Direction)
            {
                this.lanesIn = lanesIn;
                this.lanesOut = lanesOut;
            }
        }

        public void Tick(SimControl sim, int extraSpeed, double extraTime)
        {
            currentSpawn += (spawnPerSec * (extraSpeed + 1)) + ((extraTime / 50) * spawnPerSec);

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
                spawnLane = ((random[0] * 10) / 8) % lanesOut;
                List<List<Vehicle>> vehicleList = vehicles[this.Direction - 1];
                if (vehicleList[spawnLane].Count < 4)
                {
                    AddVehicle(sim, createVehicle(spawnLane), Direction, spawnLane);
                    sim.totalCars++;
                }
            }
            currentSpawn--;
        }

        public Vehicle createVehicle(int spawnLane)
        {
            return new NormalCar(this.position, this.position, 10, this.maxSpeed, this.Direction, spawnLane);
        }

        /// <summary>
        /// Based on the method doesConnect in Tile
        /// returns true if the given side connects to another tile and false if it doesn't
        /// </summary>
        /// <param name="side"></param>
        /// <returns></returns>
        public override bool doesConnect(int side)
        {
            if ((side + 1) % 4 + 1 == this.Direction)
                return true;
            return false;
        }

        /// <summary>
        /// Based on the method DrawImage in Tile
        /// returns the bitmap of the Spawner.
        /// </summary>
        /// <returns></returns>
        public override Bitmap DrawImage()
        {
            Bitmap image = new Bitmap(100, 100);
            DrawTile.drawSpawner(Graphics.FromImage(image), Direction, lanesOut, lanesIn);
            return image;
        }
    }
}
