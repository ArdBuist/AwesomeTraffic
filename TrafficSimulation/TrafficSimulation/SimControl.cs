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
        BuildPanel buildPanel;
        ControlPanel controlPanel;
        string currentTileString;
        public BitmapControl trafficlightMap;
        public BitmapControl bitmapMap;
        public BitmapControl vehicleMap;
        Point mouseDownPoint;
        public Tile[] tileList;
        public List<Vehicle> vehicleList;
        public int tilesHorizontal;
        public Simulation sim;
        public PictureBox background, trafficlight, vehicle;
        public List<TrafficlightControl> controlList = new List<TrafficlightControl>();
        public Tile currentBuildTile;
        public bool eraser = false;
        public bool selected = false;
        public int TimeofDay = 1;
        Boolean isBuildingMode; //moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public ElementHost BovenHost, OnderHost, InfoHost;
        public BovenScherm BovenScherm;
        public InfoBalk InfoBalk;
        public Boolean Simulatie = true, Day = true, InfoVisible = true;
        //SimWindow simwindow;

        public SimControl(Size size, SimWindow sim)
        {
            mouseDownPoint = new Point(0, 0);
            //grootte van de kaart
            this.Size = new Size(1600,900);
            //buildingmode is true als er word gebouwd en false als de simulatie start
            // Maak de infobalk, onderscherm en bovenscherm
            InfoBalk InfoBalk = new InfoBalk(this);
            OnderScherm OnderScherm = new OnderScherm(this);
            BovenScherm BovenScherm = new BovenScherm(sim, this, InfoBalk);
            int HoogteBovenBalk, HoogteOnderbalk, BreedteInfoBalk, HoogteInfobalk, BreedteScherm, HoogteScherm, YLocatieOnderbalk;
            isBuildingMode = true;
            //
            tilesHorizontal = Size.Width / 100;
            /* De bitmapControlls waar de simulatie in word afgebeeld
             * bitmapMap voor de achtergrond met tileList
             * vehicleMap voor de middelste bitmap met Vehicles
             * trafficlightMap voor de voorgrond met de Trafficlights */
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
            this.Paint += this.Teken;

            this.MouseMove += (object o, MouseEventArgs mea) =>
            {
                if (mouseDownPoint != new Point(0, 0))
                    if (TileIsStraight(mouseDownPoint, mea.Location))
                        DrawTile(mea);
            };
            this.MouseUp += (object o, MouseEventArgs mea) => { mouseDownPoint = new Point(0, 0); };
            this.Visible = true;
            //Initialisatie van de array waarin alle tileList worden opgeslagen
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Nog niet zeker of deze nodig is, nu nog ongebruikt
            vehicleList = new List<Vehicle>();
            //De simulatie zelf, hierin word ervoor gezorgd dat de simulatie daadwerkelijk loopt
            this.sim = new Simulation(this);
            //tekenfunctie voor de tileList (tijdelijke functie)
            DrawStartImages();

            /* PictureBoxes waar de bitmaps in worden opgeslagen, 
             * background voor de tileList van de weg
             * vehicle voor de Vehicles die op de weg rijden
             * trafficlight voor de Trafficlights op de wegen */
            background = new PictureBox();
            vehicle = new PictureBox();
            trafficlight = new PictureBox();
            trafficlight.MouseDown += this.MouseClickDown;
            //mouseclick event, zorgt er nu voor dat de simulatie word gestart maar moet worden gebruikt om tileList op het veld te plaatsen
            Simulatie = false;

            //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            BreedteScherm = Screen.PrimaryScreen.Bounds.Width;
            HoogteScherm = Screen.PrimaryScreen.Bounds.Height;
            HoogteBovenBalk = 100;
            HoogteOnderbalk = 100;
            YLocatieOnderbalk = (HoogteScherm - HoogteOnderbalk);
            HoogteInfobalk = (HoogteScherm - (HoogteBovenBalk + HoogteOnderbalk));
            BreedteInfoBalk = 300;

            BovenHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteScherm,
                Child = BovenScherm,
            };
            this.Controls.Add(BovenHost);

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(0, YLocatieOnderbalk),
                Height = HoogteOnderbalk,
                Width = BreedteScherm,
                Child = OnderScherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Height = HoogteInfobalk,
                Width = BreedteInfoBalk,
                Child = InfoBalk,
            };
            this.Controls.Add(InfoHost);

            Invalidate();
        }

        private void Teken(object o, PaintEventArgs pea)
        {
            //dit zorgt ervoor dat de kaart op het scherm wordt weergegeven.
            //dit hoeft alleen maar gebeuren wanneer er nog aan de kaart gewerkt wordt.
            if (isBuildingMode == true)
            {
                this.background.Image = bitmapMap.bitmap;
                this.background.BackColor = Color.Transparent;
                this.background.Location = new Point(0, 0);
                this.background.Size = new Size(this.Width, this.Height);
                this.Controls.Add(background);
                //moet in else komen, als de simulatie word gestart
                this.vehicle.Image = vehicleMap.bitmap;
                this.vehicle.BackColor = Color.Transparent;
                this.vehicle.Location = new Point(0, 0);
                this.vehicle.Size = new Size(this.Width, this.Height);
                this.background.Controls.Add(vehicle);
                this.trafficlight.Image = trafficlightMap.bitmap;
                this.trafficlight.BackColor = Color.Transparent;
                this.trafficlight.Location = new Point(0, 0);
                this.trafficlight.Size = new Size(this.Width, this.Height);
                this.vehicle.Controls.Add(trafficlight);
            }
            else
            {
            }
        }

        private void MouseClickDown(object obj, MouseEventArgs mea)
        {
            Bitmap tileImage;
            //mouseDownPoint wordt gebruikt voor het laten functioneren van het door slepen aanbrengen van wegen
            mouseDownPoint = new Point(mea.X / 100 * 100, mea.Y / 100 * 100);
            DrawTile(mea);
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
            if (selected == true) //als de select-tool is aangeklikt
            {
                Tile selectedTile = tileList[CalculateListPlace(mea.X, mea.Y)];

                //Blauw randje om geselecteerde tile
                tileImage = new Bitmap(100, 100);
                Graphics gr = Graphics.FromImage(tileImage);
                Pen selectPen = new Pen(Color.LightBlue, Width = 3);
                gr.DrawRectangle(selectPen, (mea.X / 100 * 100), (mea.Y / 100 * 100), 100, 100);
            }

            else
            {
                if (eraser == false) //als de gum-tool niet is aangeklikt
                {
                    currentBuildTile.SetValues(this, mea.Location, CalculateListPlace(mea.X, mea.Y));
                    tileImage = currentBuildTile.DrawImage();
                    //currentBuildTile.Update(this, null, 0);
                    tileList[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
                    //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
                    bitmapMap.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
                }
                else //alsde gum-tool is geselecteerd, wordt er een nieuwe bitmap gemaakt waarop een groen vlak 
                //wordt getekend (oftewel, de geklikte tile wordt 'verwijderd')
                {
                    tileImage = new Bitmap(100, 100);
                    Graphics gr = Graphics.FromImage(tileImage);
                    gr.FillRectangle(Brushes.Green, (mea.X / 100 * 100), (mea.Y / 100 * 100), 100, 100);
                    tileList[CalculateListPlace(mea.X, mea.Y)] = null;
                }

                //host.BackColorTransparent = true;
            }
        }
        /*controleert of de tile een rechte weg is en checkt of de weg naar de goede kant doorloopt zodat je een hele weg kunt maken door rechtdoor te slepen
        *Hierdoor kun je alleen rechte wegen door slepen op de kaart aanbrengen. Dit verhoogt het gebruiksgemak omdat het wegen leggen zo een stuk sneller gaat.
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

        private void DrawTile(MouseEventArgs mea)
        {
            Bitmap tileImage;
            //currentBuildTile = new Crossroad(this);
            currentBuildTile = new Fork(this, 1);
            //currentBuildTile = new Road(4, 3);
            //currentBuildTile = new Spawner(2);
            currentBuildTile.SetValues(this, new Point(mea.X / 100 * 100, mea.Y / 100 * 100), CalculateListPlace(mea.X, mea.Y));
            tileImage = currentBuildTile.DrawImage();
            tileList[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
            currentBuildTile = CopyCurrentTile();//hier wordt een nieuwe buildTile gemaakt met dezelfde waardes als daarvoor omdat er dan opnieuw een tile ingeklikt kan worden.
            Invalidate();
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
                    tile = new Fork(this, currentForkTile.notDirection);
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
            currentBuildTile = new Spawner(2);
            roadX = 0;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100) + 12, roadY * 100 + 50), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 1;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 2;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 3;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Crossroad(this);
            roadX = 4;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);


            currentBuildTile = new Road(2, 4);
            roadX = 5;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 6;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 9;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 10;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 11;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 12;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 13;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

        }

        /*
         * 
         */
        private void SimControl_Load(object sender, EventArgs e)
        {




            //tiles[600 / 100 * tilesHorizontal + 600 / 100] = currentBuildTile;
            //currentBuildTile.SetValues(new Point(600, 600), 600 / 100 * tilesHorizontal + 600 / 100);
            //bitmapMap.AddTile(tileImage, 6, 6);
            //currentBuildTile = tiles[500 / 100 * tilesHorizontal + 700 / 100];
            //currentBuildTile.setLanesHighToLow(1);
            //currentBuildTile.Update(this, null, 0);
        }
    }
}
