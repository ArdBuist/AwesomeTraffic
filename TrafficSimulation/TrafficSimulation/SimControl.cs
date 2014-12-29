using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Integration;


namespace TrafficSimulation
{
    public partial class SimControl : UserControl
    {
        //BitmapControls used for the bitmaps in which the background, vehicles and trafficlights are stored
        public BitmapControl backgroundBC, trafficlightBC, vehicleBC;
        //PictureBox 
        public PictureBox backgroundPB, trafficlightPB, vehiclePB;

        //
        Point mouseDownPoint;
        Point mouseMovePoint;

        //list for all the tiles made in the simulation
        public Tile[] tileList;

        //lijst voor geselecteerde tiles voor de groene zone
        public Tile[] greenZoneList;
        //lijst voor verwijderen geselcteerde tiles voor de groene zone
        public Tile[] gzRemoveList;
        //counter voor groene zone
        private int countGreenZone;

        //de oude geselecteerde tile
        public Tile oldselectedTile;
        //list for all vehicles needs to be removed
        public List<Vehicle> vehicleList;
        //
        public int tilesHorizontal;
        //the simulation, has a new thread which is started when the simulation starts
        public Simulation simulation;
        //list for all the trafficlight controls needs to be removed
        public List<TrafficlightControl> controlList = new List<TrafficlightControl>();

        //
        public Tile currentBuildTile;

        //variabelen voor klikmethodes: state geeft aan op welke knop er is geklikt en dus wat voor actie de klikmethode moet uitvoeren
        public String state = "selected";
        public int TimeofDay = 1;
        //
        bool isMoved;

        public Boolean Simulatie;
        public bool Day;
        public bool InfoVisible;

        //SimWindow simwindow;

        public SimControl(Size size, SimWindow simulation)
        {
            //methode in the partial class creating all the objects needed for the simulation
            this.Size = new Size(2000, 1500);//has to be changed to the windowsize
            /* 
             * De bitmapControls in which the simulation takes place, all the bitmapcontrols have a bitmap with
             * which it interacts. Use the BitmapControl to change the bitmaps used for the simulation
             */
            backgroundBC = new BitmapControl(this.Size);
            trafficlightBC = new BitmapControl(this.Size);
            vehicleBC = new BitmapControl(this.Size);

            //Initialisation of the array in which all the positions of the tiles will be saved.
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
     
            Simulatie = false;

            Simulatie = true;
            Day = true;
            InfoVisible = true;

            //
            isMoved = false;
            //
            mouseDownPoint = new Point(0, 0);
            mouseMovePoint = new Point(0, 0);

            //aantal tiles die horizontaal in de bitmap passen
            tilesHorizontal = Size.Width / 100; //nutteloos

            //
            this.DoubleBuffered = true;
            this.Visible = true;


            //Initialisatie van de array waarin alle tiles worden opgeslagen
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Initialisatie van de array waarin alle geselecteerde tiles voor de groene zone in worden opgeslagen
            greenZoneList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Nog niet zeker of deze nodig is, nu nog ongebruikt
            vehicleList = new List<Vehicle>();
            //De simulatie zelf, hierin word ervoor gezorgd dat de simulatie daadwerkelijk loopt
            this.simulation = new Simulation(this);

            //tekenfunctie voor de tileList (tijdelijke functie)
            InitializeComponent();
            DrawStartImages();

            //The simulation thread will be started here, the whole simulation will be regulated in this class.
            this.simulation = new Simulation(this);
        }

        /*controleert of de tile een rechte weg is en checkt of de weg naar de goede kant doorloopt zodat je een hele weg kunt 
         * maken door rechtdoor te slepen. Hierdoor kun je alleen rechte wegen door slepen op de kaart aanbrengen. Dit verhoogt 
         * het gebruiksgemak omdat het wegen leggen zo een stuk sneller gaat.
        */
        private bool TileIsStraight(Point mouseDown, Point mousePoint)
        {
            if (currentBuildTile.name == "Road")
            {
                Road tile = (Road)currentBuildTile;
                if ((tile.startDirection + tile.endDirection) % 2 == 0)
                {
                    if (tile.startDirection == 2 && mouseDown.Y < mousePoint.Y && mouseDown.Y + 100 > mousePoint.Y)
                        return true;
                    if (tile.startDirection == 1 && mouseDown.X < mousePoint.X && mouseDown.X + 100 > mousePoint.X)
                        return true;
                }
            }
            return false;
        }

        //tekent een blauwe lijn om de geselecteerde tile
        private void DrawSelectLine(MouseEventArgs mea)
        {
            if (tileList[CalculateListPlace(mea.X, mea.Y)] != null)
            {
                Bitmap tileImage;
                Tile selectedTile = new SelectTile();

                //Er wordt een blauw randje getekend om de geselecteerde tile
                selectedTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
                tileImage = selectedTile.DrawImage();
                //de huidige selectedTile wordt de oude selectedtile voor de volgende keer
                oldselectedTile = tileList[CalculateListPlace(mea.X, mea.Y)];
                this.Invalidate();
            }
        }

        //"verwijdert" een tile (d.m.v. tekenen groen vlak)
        private void removeTile(MouseEventArgs mea)
        {
            if (tileList[CalculateListPlace(mea.X, mea.Y)] != null)
            {
                Bitmap tileImage;
                Tile selectedTile = new removeTile();
                selectedTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
                tileImage = selectedTile.DrawImage();
                tileList[CalculateListPlace(mea.X, mea.Y)] = null;

                this.Invalidate();

                //hier moet nog bij dat de trafficlights ook worden verwijderd
            }
        }

        private void DrawTile(MouseEventArgs mea)
        {
            Bitmap tileImage;
            //currentBuildTile = new Crossroad(this);
            //currentBuildTile = new Fork(this, 1);
            //currentBuildTile = new Road(4, 2);
            //currentBuildTile = new Spawner(3);
            currentBuildTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
            tileImage = currentBuildTile.DrawImage();
            tileList[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile
            backgroundBC.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
            currentBuildTile = CopyCurrentTile();//hier wordt een nieuwe buildTile gemaakt met dezelfde waardes als daarvoor omdat er dan opnieuw een tile ingeklikt kan worden.

            Invalidate();
        }

        //methode om een groene zone te selecteren
        private void DrawGreenZone(MouseEventArgs mea)
        {
            Bitmap tileImage;

            //checken 
            /*if(ValidSelect() == true)
            {
                for()
                {}
            }
             * */
            Tile selectedTile = new SelectTile();

            //Er wordt een blauw randje getekend om de geselecteerde tile
            selectedTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
            tileImage = selectedTile.DrawImage();
            //de geselecteerde tile wordt toegevoegd aan de lijst om de groene zone te verwijderen
            gzRemoveList[CalculateListPlace(mea.X, mea.Y)] = selectedTile;
            //de geselecteerde tile wordt toegevoegd aan de lijst met de groene zone tiles
            greenZoneList[countGreenZone] = selectedTile;
            countGreenZone++;
            

            this.Invalidate();
        }

        //methode om de groene zone te verwijderen
        private void RemoveGreenZone()
        {
            //als er een groene zone is, dan worden alle groene zone tiles overgetekend
                for (int i = 0; i < gzRemoveList.Length;i++)
                {
                    if(gzRemoveList[i] == tileList[i] && gzRemoveList[i] != null)
                    {
                        Bitmap tileImage = tileList[i].DrawImage();
                        backgroundBC.AddObject(tileImage, tileList[i].position.X / 100 * 100 , tileList[i].position.Y / 100 * 100);
                    }
                }

            //counter voor de green zone komt op 0 voor een volgende groene zone
            countGreenZone = 0;

            //de lijst van groene zone tiles wordt leeggemaakt
            for(int t = 0; t < greenZoneList.Length; t++)
            {
                greenZoneList[t] = null;
            }
        }


        //kijk of 
        private bool ValidSelect()
        {
           // if (countGreenZone != 0)
            
                return true;
            
        }
        
         
        private void MoveMap(MouseEventArgs mea)
        {
            if (Math.Abs(mea.X - mouseMovePoint.X) > 5 || Math.Abs(mea.Y - mouseMovePoint.Y) > 5)
            {
                Rectangle moveGround = new Rectangle(new Point(Screen.PrimaryScreen.Bounds.X - backgroundPB.Size.Width, Screen.PrimaryScreen.Bounds.Y - backgroundPB.Size.Height), new Size(backgroundPB.Size.Width - Screen.PrimaryScreen.Bounds.X, backgroundPB.Size.Height - Screen.PrimaryScreen.Bounds.Y));
                Point newPosition = new Point(backgroundPB.Location.X + (mea.X - mouseMovePoint.X), backgroundPB.Location.Y + (mea.Y - mouseMovePoint.Y));

                if (moveGround.Contains(newPosition))
                {
                    backgroundPB.Location = newPosition;
                    isMoved = false;
                }
                this.Update();
            }
        }

        //methode maakt een kopie van de huidige tile die net getekend is, zodat dezelfde tile nog een keer getekend kan worden.
        private Tile CopyCurrentTile()
        {
            Tile tile;
            string tileName = currentBuildTile.name;
            switch (tileName)
            {
                case "Spawner": Spawner currentSpawnerTile = (Spawner)currentBuildTile;
                    tile = new Spawner(currentSpawnerTile.direction);
                    break;
                case "Crossroad": tile = new Crossroad(this);
                    break;
                case "Road": Road currentRoadTile = (Road)currentBuildTile;
                    tile = new Road(currentRoadTile.startDirection, currentRoadTile.endDirection);
                    break;
                case "Fork": Fork currentForkTile = (Fork)currentBuildTile;
                    tile = new Fork(this, currentForkTile.NotDirection);
                    break;
                default: tile = new Crossroad(this);
                    break;
            }
            return tile;
        }
        private int CalculateListPlace(int mouseX, int mouseY)
        {
            return mouseY / 100 * tilesHorizontal + mouseX / 100;
        }

        //hele methode kan weer weg zo gauw er een interface is waar we mee kunnen testen.
        private void DrawStartImages()
        {
            Bitmap tileImage;
            int roadX, roadY;

            /*kaart voor testen en laten zien*/
            currentBuildTile = new Spawner(3);
            roadX = 5;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(3);
            roadX = 6;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(4);
            roadX = 11;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(2);
            roadX = 5;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(1);
            roadX = 11;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 2);
            roadX = 5;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 3);
            roadX = 5;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 2);
            roadX = 5;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 3);
            roadX = 5;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 3);
            roadX = 6;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 3);
            roadX = 6;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 6;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 6;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 3);
            roadX = 6;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 4);
            roadX = 6;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 4);
            roadX = 6;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 1);
            roadX = 7;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 1);
            roadX = 7;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 2);
            roadX = 7;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 8;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 3);
            roadX = 8;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 2);
            roadX = 8;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 9;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 9;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 9;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 1);
            roadX = 9;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 9;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 3);
            roadX = 9;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 3);
            roadX = 9;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 1);
            roadX = 10;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 1);
            roadX = 10;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 1);
            roadX = 10;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 3);
            roadX = 10;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 10;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 4);
            roadX = 10;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 10;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 11;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 1);
            roadX = 11;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile.LanesHighToLow = 2;
            currentBuildTile.UpdateOtherTiles(this, 0);
            currentBuildTile = new Fork(this, 2);
            roadX = 11;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
        }

        private void SimControl_Load(object sender, EventArgs e)
        {

        }
    }
}
