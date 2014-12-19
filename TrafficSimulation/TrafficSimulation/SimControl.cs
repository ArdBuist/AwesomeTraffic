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
        Point mouseMovePoint;
        public Tile[] tileList;
        public Tile oldselectedTile;
        public List<Vehicle> vehicleList;
        public int tilesHorizontal;
        public Simulation sim;
        public PictureBox background, trafficlight, vehicle;
        public List<TrafficlightControl> controlList = new List<TrafficlightControl>();
        public Tile currentBuildTile;
        public bool eraser = false;
        public bool selected = false;
        public bool building = true;
        public int TimeofDay = 1;
        Boolean isBuildingMode; //moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 


       //kaartverslepen

       public Boolean Simulatie = true, Day = true, InfoVisible = true;
        
        //SimWindow simwindow;

        public SimControl(Size size, SimWindow sim)
        {
            this.Size = new Size(1600, 900);
            this.Size = new Size(2000, 1500);
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            background = new PictureBox();
            background.SizeMode = PictureBoxSizeMode.Normal;
            vehicle = new PictureBox();
            trafficlight = new PictureBox();
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

            mouseDownPoint = new Point(0, 0);
            mouseMovePoint = new Point(0, 0);
            //grootte van de kaart

            //buildingmode is true als er word gebouwd en false als de simulatie start

            isBuildingMode = true;
            //
            tilesHorizontal = Size.Width / 100;
            /* De bitmapControlls waar de simulatie in word afgebeeld
             * bitmapMap voor de achtergrond met tileList
             * vehicleMap voor de middelste bitmap met Vehicles
             * trafficlightMap voor de voorgrond met de Trafficlights */

            this.DoubleBuffered = true;
            //this.Paint += this.Teken;
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

            trafficlight.MouseDown += this.MouseClickDown;
            trafficlight.MouseMove += (object o, MouseEventArgs mea) =>
            {
                if (mouseDownPoint != new Point(0, 0))
                {
                    if (TileIsStraight(mouseDownPoint, mea.Location))
                        DrawTile(mea);
                    if (selected == true)
                        MoveMap(mea);
                }
            };
            trafficlight.MouseUp += (object o, MouseEventArgs mea) => { mouseDownPoint = new Point(0, 0); mouseMovePoint = new Point(0, 0); };
            //mouseclick event, zorgt er nu voor dat de simulatie word gestart maar moet worden gebruikt om tileList op het veld te plaatsen
            Simulatie = false;

        }

        private void Teken(object o, PaintEventArgs pea)
        {
            //dit zorgt ervoor dat de kaart op het scherm wordt weergegeven.
            //dit hoeft alleen maar gebeuren wanneer er nog aan de kaart gewerkt wordt.
            if (isBuildingMode == true)
            {
                
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
            mouseMovePoint = mea.Location;

            /*deze code moet worden gedaan zo als de simulatie wordt gestart.*/

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

            //de eerder geselecteerde tile wordt opnieuw getekend en verwijdert zo de blauwe rand
            if (oldselectedTile != null)
            {

                bitmapMap.AddObject(oldselectedTile.DrawImage(), oldselectedTile.position.X, oldselectedTile.position.Y);
            }



            if (selected == true) //als de select-tool is aangeklikt
            {
                DrawSelectLine(mea);                
            }
           
             //als de gum-tool is aangeklikt
            if (eraser == true) 
            {
                removeTile(mea);
             }

            //als je een weg wil bouwen
            if (building == true)
            {
                DrawTile(mea);
            }
               
                //host.BackColorTransparent = true;
            
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
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddObject(tileImage, mea.X / 100 * 100, mea.Y / 100 * 100);
            currentBuildTile = CopyCurrentTile();//hier wordt een nieuwe buildTile gemaakt met dezelfde waardes als daarvoor omdat er dan opnieuw een tile ingeklikt kan worden.
            
            Invalidate();
        }

        private void MoveMap(MouseEventArgs mea)
        {
            if (Math.Abs(mea.X - mouseMovePoint.X) > 5 || Math.Abs(mea.Y - mouseMovePoint.Y) > 5)
            {
                Rectangle moveGround = new Rectangle(new Point(Screen.PrimaryScreen.Bounds.X - background.Size.Width, Screen.PrimaryScreen.Bounds.Y - background.Size.Height), new Size(background.Size.Width - Screen.PrimaryScreen.Bounds.X, background.Size.Height - Screen.PrimaryScreen.Bounds.Y));
                Point newPosition = new Point(background.Location.X + (mea.X - mouseMovePoint.X), background.Location.Y + (mea.Y - mouseMovePoint.Y));

                if (moveGround.Contains(newPosition))

                    background.Location = newPosition;
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
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(3);
            roadX = 6;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(4);
            roadX = 11;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(2);
            roadX = 5;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Spawner(1);
            roadX = 11;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1,2);
            roadX = 5;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,3);
            roadX = 5;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 2);
            roadX = 5;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 3);
            roadX = 5;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,3);
            roadX = 6;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this,3);
            roadX = 6;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 6;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 6;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1, 3);
            roadX = 6;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 4);
            roadX = 6;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1,4);
            roadX = 6;
            roadY = 9;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,4);
            roadX = 7;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,4);
            roadX = 7;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this,1);
            roadX = 7;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 1);
            roadX = 7;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 2);
            roadX = 7;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 8;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this,3);
            roadX = 8;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 2);
            roadX = 8;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,4);
            roadX = 9;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 9;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 9;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 1);
            roadX = 9;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2,4);
            roadX = 9;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 3);
            roadX = 9;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 3);
            roadX = 9;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Fork(this, 1);
            roadX = 10;
            roadY = 2;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 1);
            roadX = 10;
            roadY = 3;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 1);
            roadX = 10;
            roadY = 4;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(4, 3);
            roadX = 10;
            roadY = 5;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Crossroad(this);
            roadX = 10;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(1,4);
            roadX = 10;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(2, 4);
            roadX = 10;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 4);
            roadX = 11;
            roadY = 6;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile = new Road(3, 1);
            roadX = 11;
            roadY = 7;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
            currentBuildTile.LanesHighToLow = 2;
            currentBuildTile.UpdateOtherTiles(this, 0);
            currentBuildTile = new Fork(this,2);
            roadX = 11;
            roadY = 8;
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point((roadX * 100), roadY * 100), roadY * tilesHorizontal + roadX);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
        }

        private void SimControl_Load(object sender, EventArgs e)
        {

        }
    }
}
