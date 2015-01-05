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

        //lijst voor geselecteerde tiles voor de groene golf
        public Tile[] greenWaveList;
        //lijst voor verwijderen geselcteerde tiles voor de groene golf
        public Tile[] greenWaveRemoveList;
        //counter voor groene golf
        private int countGreenWave;
        //oude groene golf tile
        private Tile oldGreenWaveTile;
        //variabele voor klikmethodes geeft aan: groene golf bouwen of verwijderen
        public String stateGreenWave;

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
            //this.DoubleBuffered = true;
            this.Visible = true;


            //Initialisatie van de array waarin alle tiles worden opgeslagen
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Initialisatie van de array waarin alle geselecteerde tiles voor de groene golf in worden opgeslagen
            greenWaveList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Initialisatie van de array waarin alle geselecteerde tiles voor de groene golf in worden opgeslagen
            greenWaveRemoveList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
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
                trafficlightBC.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
                tileList[CalculateListPlace(mea.X, mea.Y)] = null;
                this.Invalidate();
                //hier moet nog bij dat de trafficlights ook worden verwijderd
            }
        }

        private void DrawTile(MouseEventArgs mea)
        {
            Bitmap tileImage;
            currentBuildTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
            tileImage = currentBuildTile.DrawImage();
            //tile wordt in de lijst van tiles gezet
            tileList[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile
            backgroundBC.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
            trafficlightBC.bitmap.MakeTransparent(Color.Green);
            currentBuildTile = CopyCurrentTile();//hier wordt een nieuwe buildTile gemaakt met dezelfde waardes als daarvoor omdat er dan opnieuw een tile ingeklikt kan worden.
            this.Invalidate();
        }

        //methode om een groene golf te selecteren
        private void DrawGreenWave(MouseEventArgs mea)
        {
            Bitmap tileImage;
            Tile selectedTile = new SelectGreenWaveTile();

            //als er op een al geselecteerde groene golf tile wordt geklikt
            if (greenWaveRemoveList[CalculateListPlace(mea.X, mea.Y)] != null)
            {
                if (countGreenWave != 0)
                {
                    //als de hiervoor aangeklikte groene golf tile is aangeklikt
                    if (mea.X / 100 * 100 == greenWaveList[(countGreenWave - 1)].position.X && mea.Y / 100 * 100 == greenWaveList[(countGreenWave - 1)].position.Y)
                    {
                        //verwijder deze tile uit de removelist + andere groene golf list en teken de tile opnieuw
                        greenWaveRemoveList[CalculateListPlace(mea.X, mea.Y)] = null;
                        greenWaveList[(countGreenWave - 1)] = null;

                        tileImage = tileList[CalculateListPlace(mea.X, mea.Y)].DrawImage();
                        backgroundBC.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
                    }

                    else
                    {
                        //pop-up scherm of info in het infoscherm: "U kunt alleen de laatst geselecteerde groene golf tegel verwijderen."
                    }
                }
            }

            //als er geklikt wordt op een tile die een groene golf mag hebben
            if (ValidSelect(selectedTile, mea.X, mea.Y) == true)
            {
                //tekenen selectielijn om de tile
                selectedTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
                tileImage = selectedTile.DrawImage();
                backgroundBC.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);

                //de geselecteerde tile wordt toegevoegd aan de 2 groene golflijsten en de counter wordt opgehoogd
                greenWaveRemoveList[CalculateListPlace(mea.X, mea.Y)] = selectedTile;
                greenWaveList[countGreenWave] = selectedTile;
                countGreenWave++;

                //de huidige selectedTile wordt de oude selectedtile voor de volgende keer
                oldGreenWaveTile = tileList[CalculateListPlace(mea.X, mea.Y)];

                this.Invalidate();
            }

            //als er op een tile wordt geklikt die niet mag en die nog geen groene golf tile is
            if (ValidSelect(selectedTile, mea.X, mea.Y) == false && greenWaveRemoveList[CalculateListPlace(mea.X, mea.Y)] == null)
            {
                //in infoscherm zetten: "U kunt alleen aansluitende wegen of kruispunten selecteren. Kies een andere tegel."
            }
        }


        //methode om de groene golf te verwijderen
        public void RemoveGreenWave()
        {
            Bitmap tileImageGreen;
            //als er een groene golf is, dan worden alle groene golf tiles overgetekend
            for (int i = 0; i < greenWaveRemoveList.Length; i++)
            {
                if (greenWaveRemoveList[i] != null)
                {
                    tileImageGreen = tileList[i].DrawImage();
                    backgroundBC.AddObject(tileImageGreen, tileList[i].position.X, tileList[i].position.Y);
                    this.Invalidate();
                }
            }

            //counter en de lijst worden weer leeggemaakt
            countGreenWave = 0;
            oldGreenWaveTile = null;
            for (int t = 0; t < greenWaveList.Length; t++)
            {
                greenWaveList[t] = null;
            }
            for (int s = 0; s < greenWaveRemoveList.Length; s++)
            {
                greenWaveRemoveList[s] = null;
            }
        }


        //kijk of de geklikte tile wel een groene golf mag zijn
        private bool ValidSelect(Tile selectedTile, int x, int y)
        {
            //als uit de 4 specifieke ValidSelects true komt, dan is de tile true, anders false
            if (ValidSelectnoGreenWave(x, y) == true && ValidSelecthasRoad(x, y) == true && ValidSelectnexttoOldGreenWave(x, y) == true
                && ValidSelectendsinRoad(x, y) == true)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //checken of de tile niet al een groene golf tile is. True als het geen groene golf tile is
        private bool ValidSelectnoGreenWave(int x, int y)
        {
            if (greenWaveRemoveList[CalculateListPlace(x, y)] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //checken of de tile naast de eerder geselecteerde groene golf tile ligt. True als dat zo is
        private bool ValidSelectnexttoOldGreenWave(int x, int y)
        {
            if (oldGreenWaveTile != null)
            {
                int oldx = oldGreenWaveTile.position.X / 100;
                int oldy = oldGreenWaveTile.position.Y / 100;

                int newx = x / 100;
                int newy = y / 100;

                //geklikte tile ligt direct rechts of direct links van de oude groene golf tile
                if ((newx - oldx == 1 || oldx - newx == 1) && newy == oldy)
                {
                    return true;
                }
                //geklikte tile ligt direct boven of direct onder de oude groene golf tile
                else if ((newy - oldy == 1 || oldy - newy == 1) && newx == oldx)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        //checken of de tile een weg of kruispunt heeft. True als dat zo is
        private bool ValidSelecthasRoad(int x, int y)
        {
            if (tileList[CalculateListPlace(x, y)] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //checken of de tile aan een uitgang van de eerder geselecteerde groene golf tile ligt. True als dat zo is
        private bool ValidSelectendsinRoad(int x, int y)
        {
            if (oldGreenWaveTile != null)
            {
                int oldx = oldGreenWaveTile.position.X / 100;
                int oldy = oldGreenWaveTile.position.Y / 100;

                int newx = x / 100;
                int newy = y / 100;

                //checken bij t-splitsingen
                if (oldGreenWaveTile.name == "Fork")
                {
                    if (oldGreenWaveTile.notDirection != 1 && (oldy - newy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.notDirection != 2 && (newx - oldx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.notDirection != 3 && (newy - oldy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.notDirection != 2 && (oldx - newx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                //checken bij rechte en kromme wegen
                else if (oldGreenWaveTile.name == "Road")
                {
                    if ((oldGreenWaveTile.startDirection == 1 || oldGreenWaveTile.endDirection == 1) && (oldy - newy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if ((oldGreenWaveTile.startDirection == 2 || oldGreenWaveTile.endDirection == 2) && (newx - oldx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else if ((oldGreenWaveTile.startDirection == 3 || oldGreenWaveTile.endDirection == 3) && (newy - oldy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if ((oldGreenWaveTile.startDirection == 4 || oldGreenWaveTile.endDirection == 4) && (oldx - newx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //checken bij spawners
                else if (oldGreenWaveTile.name == "Spawner")
                {
                    if (oldGreenWaveTile.direction == 1 && (oldy - newy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.direction == 2 && (newx - oldx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.direction == 3 && (newy - oldy == 1 && newx == oldx))
                    {
                        return true;
                    }
                    else if (oldGreenWaveTile.direction == 4 && (oldx - newx == 1 && newy == oldy))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (oldGreenWaveTile.name == "Crossroad")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
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
        private int CalculateListPlace(int mouseX, int mouseY)
        {
            return mouseY / 100 * tilesHorizontal + mouseX / 100;
        }

        public void ClearRoad()
        {
            foreach (Tile t in tileList)
            {
                if (t != null)
                {
                    foreach (List<Vehicle> l in t.vehicles)
                    {
                        l.Clear();
                    }
                }
            }
            //alle auto's weer verwijderen
            Graphics g = Graphics.FromImage((System.Drawing.Image)vehicleBC.bitmap);
            g.Clear(System.Drawing.Color.Transparent);
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

        public void MakeTrafficControlList()
        {
            foreach (Tile t in tileList)
            {
                if (t != null)
                {
                    if (t.name == "Crossroad")
                    {
                        Crossroad Cr = (Crossroad)t;
                        if (Cr.control != null)
                        {
                            controlList.Add(Cr.control);
                        }
                    }
                    if (t.name == "Fork")
                    {
                        Fork f = (Fork)t;
                        if (f.control != null)
                        {
                            controlList.Add(f.control);
                        }
                    }
                }
            }
        }
    }
}
