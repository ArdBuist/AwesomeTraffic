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

        //haalt andere tile uit de lijst. Vul direction in voor welke kant je op wilt.
        public Tile GetOtherTile(SimControl s, int direction)
        {
            Tile tile = null;
            try
            {
                switch (direction)
                {
                    case 1: tile = s.tiles[listPlace - s.tilesHorizontal];
                        break;
                    case 2: tile = s.tiles[listPlace + 1];
                        break;
                    case 3: tile = s.tiles[listPlace + s.tilesHorizontal];
                        break;
                    case 4: tile = s.tiles[listPlace - 1];
                        break;
                }
            }
            catch
            { }
            return tile;
        }
        public void SetValues(SimControl s, Point position, int listPlace)
        {
            this.position = position;
            this.listPlace = listPlace;
            if (this.name == "Road")
            {
                Road thisTile = (Road)this;
                thisTile.UpdateFromOtherTiles(s);
            }
        }
        public abstract void UpdateFromOtherTiles(SimControl s);

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
                direction -= 4 ;
            Boolean value = false;
            switch (tile.name)
            {
                case "Spawner": Spawner tileSpawner = (Spawner)tile;
                    if(tileSpawner.direction== direction)
                        value = true;
                    break;
                case "Crossroad": value = true;
                    break;
                case "Road": Road tileRoad = (Road)tile;
                    if(tileRoad.startDirection== direction || tileRoad.endDirection == direction)
                        value = true;
                    break;
                case "Fork": Fork tileFork = (Fork)tile;
                    if(tileFork.notDirection !=direction)
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

        public void AddVehicle(Vehicle v, int lane)
        {
            vehicles[lane].Add(v);
        }

        //Hierin worden verschillende variabelen van de tiles verandert en doorgegeven naar de aanliggende tiles.
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
        private int spawnLane;//handigheid niet duidelijk voor mij
        private int currentSpawn;//baan waarop de volgende auto gespawnt gaat worden
        public int direction;//kant waarop de weg loopt
        private double carsPerSec;//auto's die per seconde gespawned worden
        private double numberOfCars;//opslag voor auto's die gespawned moeten worden voor als de weg vol is.

        public Spawner(int direction)
        {
            this.direction = direction;
            this.name = "Spawner";
            carsPerSec = 0.5;
            spawnLane = 1;

            initialize(spawnLane + 1);
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
        public override void UpdateFromOtherTiles(SimControl s)
        {
            Tile tile = GetOtherTile(s,direction);
            if(tile.name =="Road")
                Update(s, (Road)tile, direction);
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
                t.drawSpawner(Graphics.FromImage(image), direction, lanesLowToHigh, lanesHighToLow);
            return image;
        }

        private Vehicle createVehicle()
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new Vehicle(new Point(), new Point(), 0, 0, 0, 0);
        }
        public override void Update(SimControl s,Tile tile,int direction)
        {
            if (direction == this.direction)
            {
                Road road = (Road)tile;
                //if's voor verschil in richtingen van de wegen.
                if (direction < 3)
                {
                    this.lanesHighToLow  = road.getLaneLowToHigh();
                    this.lanesLowToHigh = road.getLaneHighToLow();
                }
                else
                {
                    this.lanesHighToLow = road.getLaneHighToLow();
                    this.lanesLowToHigh = road.getLaneLowToHigh();
                }

                this.maxSpeed = road.getMaxSpeed();
                s.bitmapMap.AddTile(DrawImage(), position.X / 100, position.Y / 100);
            }
        }
    }

    public class Road : Tile
    {
        public int startDirection;
        public int endDirection;

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
            //if((startDirection+endDirection)%2==0 || startDirection + endDirection < 4)
                t.drawRoad(Graphics.FromImage(image),lanesLowToHigh,lanesHighToLow,startDirection,endDirection);
            //else
               // t.drawRoad(Graphics.FromImage(image), lanesHighToLow, lanesLowToHigh, startDirection, endDirection);
            return image;
        }

        public override bool doesConnect(int side)
        {
            int direction = side + 2;
            if (direction > 4)
                direction -= 4;
            if (direction == startDirection || direction == endDirection)
                return true;
            return false;
        }

        //update de tilevariabelen en zorgt dat tiles eromheen aangeroepen gaan worden.
        public override void Update(SimControl s,Tile tile, int direction)
        {
            Road road = (Road)tile;
            //road is alleen maar null als dit de eerste methode update is die wordt aangeroepen na een verandering in de interface.
            if (road != null && (this.startDirection == direction || this.endDirection == direction))
            {
                //if (direction > 2 && (road.startDirection+road.endDirection)%2!=0)
                //{
                //    this.lanesHighToLow = road.getLaneLowToHigh();
                //    this.lanesLowToHigh = road.getLaneHighToLow();
                //}
                //else
                this.lanesHighToLow = road.getLaneHighToLow();
                this.lanesLowToHigh = road.getLaneLowToHigh();
                if(doesConnect(startDirection))
                if(road.startDirection == 1 && road.endDirection == 2)
                {
                    if (direction == 4)
                    {
                        this.lanesHighToLow = road.getLaneLowToHigh();
                        this.lanesLowToHigh = road.getLaneHighToLow();
                    }
                }
                if(road.startDirection == 3 && road.endDirection == 4)
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
                if(direction == 0)
                {
                    UpdateOtherTile(s, startDirection );
                    UpdateOtherTile(s, endDirection);
                }
                else if(direction != startDirection)
                {
                    UpdateOtherTile(s, startDirection );
                }
                else
                {
                    UpdateOtherTile(s, endDirection );
                }
            }
            s.bitmapMap.AddTile(DrawImage(),position.X/100,position.Y/100);
        }
        public override void UpdateFromOtherTiles(SimControl s)
        {
            Tile tile;
            tile = this.GetOtherTile(s, startDirection);
            if (tile != null && tile.doesConnect(startDirection)&&tile.name == "Road")
            {
                Road otherRoad = (Road)tile;
                if((startDirection +endDirection)%2==0)
                {
                    this.lanesHighToLow = otherRoad.getLaneHighToLow();
                    this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                }
                else if((startDirection == 1 && endDirection == 4) || (startDirection == 2 && endDirection == 3))
                {
                        this.lanesHighToLow = otherRoad.getLaneHighToLow();
                        this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                }
                else if(startDirection == 1)
                {
                    this.lanesHighToLow = otherRoad.getLaneHighToLow();
                    this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
                    //this.lanesLowToHigh = otherRoad.getLaneHighToLow();
                    //this.lanesHighToLow = otherRoad.getLaneLowToHigh();
                }
                else if(startDirection == 3)
                {
                    this.lanesLowToHigh = otherRoad.getLaneHighToLow();
                    this.lanesHighToLow = otherRoad.getLaneLowToHigh();
                }

                this.maxSpeed = otherRoad.getMaxSpeed();
            }
                tile = this.GetOtherTile(s, endDirection);
                if (tile != null && tile.doesConnect(endDirection)&&tile.name == "Road")
                {
                    Road otherRoad = (Road)tile;
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
                            //this.lanesHighToLow = otherRoad.getLaneHighToLow();
                            //this.lanesLowToHigh = otherRoad.getLaneLowToHigh();
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
        
        /*Deze methode zorgt ervoor dat van de tiles om deze tile heen de methode Update wordt aangeroepen.*/
        private void UpdateOtherTile(SimControl s, int direction)
        {
            Tile tile = GetOtherTile(s, direction);
            direction += 2;
            if (direction > 4)
                direction -= 4;
            if(tile != null)
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
        private List<TrafficlightControl> trafficlightControlList;

        public Fork(int notDirection)
        {
            this.name = "Fork";
            this.lanes = new int[] {1,1,1,1,1,1,1,1};
            this.notDirection = notDirection;
            lanes[notDirection * 2 - 2] = 0;
            lanes[notDirection * 2 - 1] = 0;

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 3; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
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
        public override void UpdateFromOtherTiles(SimControl s)
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
                s.bitmapMap.AddTile(DrawImage(), position.X / 100, position.Y / 100);//drawmethode werkt nog niet naar behoren door ontbreken compatibiliteit met lists
            }
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

        public Crossroad()
        {
            this.position = position;
            this.maxSpeed = maxSpeed;
            this.name = "Crossroad";
            this.lanes = new int[] { 1, 1, 1, 1, 1, 1, 1, 1 };

            trafficlightControlList = new List<TrafficlightControl>();
            for (int i = 0; i < 4; i++)
            {
                trafficlightControlList.Add(new TrafficlightControl());
            }
            int totalLanes = CountLanes(lanes);
            initialize(totalLanes);
        }
        public override bool doesConnect(int side)
        {
            return true;
        }
        public override void UpdateFromOtherTiles(SimControl s)
        {
            for(int i = 1; i< 5 ;i++)
            {
                Tile tile = GetOtherTile(s, i);
                if (tile.name == "Road")
                { }
            }
        }
        public override void Update(SimControl s,Tile tile,int direction)
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
