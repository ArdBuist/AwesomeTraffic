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


        /*
         * Variables of the tile
         *      name is the name of the tile, used to determine what kind of tile the tile is
         *      position is the position of the tile, in pixels
         *      maxSpeed is the speed cars are moving on the tile
         *      size is the size of the tile, which is 100,100
         *      
         *      access is a boolean array[,],  each lane has a boolean. If the boolean is true cars can enter the lane and if it's false cars can't enter the lane
         *      lanesLowToHigh is the number of lanes from the lowest direction to the highest direction
         *      lanesHighToLow is the number of lanes from the highest direction to the lowest direction
         *      directions is a list with 
         *      
         *      numberOfVehicles are the number of vehicles that are currently on the tile
         */
        //tilevariables
        public string name;
        public Point position;
        public int maxSpeed;
        public Size size;
        //simulatievariables
        public List<List<Vehicle>>[] vehicles;
        protected bool[,] access;
        protected int lanesLowToHigh;
        protected int lanesHighToLow;
        protected List<int> directions;
        //interfacevariables
        protected int numberOfVehicles;

        /* 
         * returns the directions of the tile
         * it returns one direction because by knowing which direction the tile doesnt have you als know which ones it does have
         */
        public int NotDirection
        {
            get
            {
                if (!directions.Contains(1))
                {
                    return 1;
                }
                else if (!directions.Contains(2))
                {
                    return 2;
                }
                else if (!directions.Contains(3))
                {
                    return 3;
                }
                else
                {
                    return 4;
                }
            }
        }
        /*
         * returns the startdirection of the tile, used by road
         * return the enddirection of the tile, used by road
         */
        public int StartDirection { get { return directions.ElementAt(0); } }
        public int EndDirection { get { return directions.ElementAt(1); } }
        //returns the direction the spawner is facing, used by spawner
        public int Direction { get { return directions.ElementAt(0); } }

        /// <summary>
        /// Tile constructor, no variables are given
        /// it only has getters and setters, when the tile is placed all values are set to default
        /// </summary>
        public Tile()
        {
            this.MaxSpeed = 2;
            this.LanesHighToLow = 1;
            this.LanesLowToHigh = 1;
            this.size = new Size(100, 100);
            directions = new List<int>();
            Initialize();
        }
        /*
         * Getters and setters needed to run the simulation and save the tiles
         */
        public List<int> Directions { get { return this.directions; } }
        public int NumberOfVehicles { get { return numberOfVehicles; } set { numberOfVehicles = value; } }
        public bool[,] Access { get { return access; } set { access = value; } }
        public int LanesHighToLow { get { return this.lanesHighToLow; } set { lanesHighToLow = value; } }
        public int LanesLowToHigh { get { return this.lanesLowToHigh; } set { lanesLowToHigh = value; } }
        public int MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

        /*
         * Called when the tile is made and set all tile values to the default values.
         */
        public void Initialize()
        {
            vehicles = new List<List<Vehicle>>[4];
            access = new bool[4, 3];
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

        /// <summary>
        /// Returns the bitmap of the tile, used when drawing the tile on the map
        /// </summary>
        /// <returns>Tile.bitmap</returns>
        public abstract Bitmap DrawImage();

        /// <summary>
        /// This method is used when a new tile is placed on the map, this causes all the tiles bordering this tile to also update
        /// if you change the lanes on a tile it updates the tiles bordering this tile, and they update the tiles bordering those tiles ect.
        /// the update chain stops at forks and crossroads.
        /// The method uses the lanes on the lowest direction of the placed tile, so the upper side first and the left side last.
        /// </summary>
        /// <param name="simcontrol"></param>
        /// <param name="direction"></param>
        public void UpdateFromOtherTile(SimControl simcontrol, int direction)
        {
            if (direction == 0)
            {
                int ForkNotDirection = 0;
                foreach (int d in directions)
                {
                    if (d != ForkNotDirection)
                    {
                        Tile nextTile = simcontrol.simulationMap.GetSurroundingTiles(this.position)[d - 1];
                        if (nextTile != null && nextTile.doesConnect(d))
                        {
                            this.UpdateLanes(simcontrol, d, nextTile.GetLanesOut((d + 1) % 4 + 1), nextTile.GetLanesIn((d + 1) % 4 + 1));
                            if (this.name == "Road")
                            {
                                Road roadTile = (Road)this;
                                int CounterDirection;
                                if (d == roadTile.StartDirection)
                                    CounterDirection = EndDirection;
                                else
                                    CounterDirection = StartDirection;
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
            List<List<Vehicle>> sideVehicles = vehicles[Side - 1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Remove(v);
            numberOfVehicles--;
            //looks if there is space for other cars to come on the tile
            if (laneVehicles.Count < 5 && this.name != "Spawner" && this.name != "Crossroad" && this.name != "Fork")
            {
                Tile lastTile = sim.simulationMap.GetSurroundingTiles(this.position)[(Side + 1) % 4];
                lastTile.Access[Side - 1, lane] = true;
            }
        }

        public void AddVehicle(SimControl sim, Vehicle v, int Side, int lane)
        {
            List<List<Vehicle>> sideVehicles = vehicles[Side - 1];
            List<Vehicle> laneVehicles = sideVehicles[lane];
            laneVehicles.Add(v);
            numberOfVehicles++;
            //looks if the tile is full
            if (laneVehicles.Count > 5 && this.name != "Spawner" && this.name != "Crossroad" && this.name != "Fork")
            {
                Tile lastTile = sim.simulationMap.GetSurroundingTiles(this.position)[(Side + 1) % 4];
                lastTile.Access[Side - 1, lane] = false;
            }
        }

    }
}
