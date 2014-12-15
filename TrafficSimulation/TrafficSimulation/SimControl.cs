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

namespace TrafficSimulation
{
    public partial class SimControl : UserControl
    {
        BuildPanel buildPanel;
        ControlPanel controlPanel;
        string currentTileString;
        Tile currentBuildTile;
        public BitmapControl trafficlightMap;
        public BitmapControl bitmapMap;
        public BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public Tile[] tileList;
        public List<Vehicle> vehicleList;
        //public List<TrafficlightControl> trafficlightList;
        public int tilesHorizontal;
        private Simulation sim;
        public PictureBox background, trafficlight, vehicle;

        public List<TrafficlightControl> controlList = new List<TrafficlightControl>();

        public SimControl(Size size)
        {
            //grootte van de kaart
            this.Size = new Size(1600, 900);
            //buildingmode is true als er word gebouwd en false als de simulatie start
            isBuildingMode = true;
            //
            tilesHorizontal = Size.Width / 100;
            /* De bitmapControlls waar de simulatie in word afgebeeld
             * bitmapMap voor de achtergrond met Tiles
             * vehicleMap voor de middelste bitmap met Vehicles
             * trafficlightMap voor de voorgrond met de Trafficlights */
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            //
            this.DoubleBuffered = true;
            //
            this.Paint += this.Teken;
            //
            this.Visible = true;
            //Initialisatie van de array waarin alle Tiles worden opgeslagen
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            //Nog niet zeker of deze nodig is, nu nog ongebruikt
            vehicleList = new List<Vehicle>();
            //De simulatie zelf, hierin word ervoor gezorgd dat de simulatie daadwerkelijk loopt
            this.sim = new Simulation(this);
            //tekenfunctie voor de Tiles (tijdelijke functie)
            DrawStartImages();

            /* PictureBoxes waar de bitmaps in worden opgeslagen, 
             * background voor de Tiles van de weg
             * vehicle voor de Vehicles die op de weg rijden
             * trafficlight voor de Trafficlights op de wegen */
            background = new PictureBox();
            vehicle = new PictureBox();
            trafficlight = new PictureBox();

            //mouseclick event, zorgt er nu voor dat de simulatie word gestart maar moet worden gebruikt om tiles op het veld te plaatsen
            trafficlight.MouseUp += this.MouseUnclick;
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

        private void MouseUnclick(object obj, MouseEventArgs mea)
        {
            Bitmap tileImage;
            //voor als de code voor het zelf maken van de tiles werkt:
            //Tile[] tileList = new Tile[] {new Crossroad(), new Road(), new Fork()};
            //for (int i = 0; i < tileList.Length;i++ )
            //{
            //    Tile possibleTile = tileList[i];
            //    if(possibleTile.ToString() == currentTileString)
            //    {
            //        currentBuildTile = possibleTile;
            //        break;
            //    }
            //}
            //currentBuildTile = new Crossroad(this);
            //currentBuildTile = new Fork(this,2 * 100);
            //currentBuildTile = new Road(4 * 100, 2 * 100);
            //currentBuildTile = new Spawner(new Point(mea.X, mea.Y), 2);
            //currentBuildTile.SetValues(mea.Location, CalculateListPlace(mea.X, mea.Y));
            //tileImage = currentBuildTile.DrawImage();
            //currentBuildTile.Update(this, null, 0);
            //tileList[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            //bitmapMap.AddObject(tileImage, mea.X, mea.Y);
            //hier moet nog een nieuwe currentBuildTile worden aangemaakt met dezelde waarden als de vorige.

            //tijdelijke sim.Start(), dit moet nog aan een knop verbonden worden
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
                }
            }
            sim.Start();
        }

        public int CalculateListPlace(int mouseX, int mouseY)
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
            currentBuildTile.SetValues(new Point((roadX * 100)+12, roadY * 100+50), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 1;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 2;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 3;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Crossroad(this);
            roadX = 4;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);


            currentBuildTile = new Road(2, 4);
            roadX = 5;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 6;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 7;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 8;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 9;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 10;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 11;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 12;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);

            currentBuildTile = new Road(2, 4);
            roadX = 13;
            roadY = 1;
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tileList[roadY * tilesHorizontal + roadX] = currentBuildTile;
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
            bitmapMap.AddObject(tileImage, roadX * 100, roadY * 100);
        }

        /*
         * 
         */
        private void SimControl_Load(object sender, EventArgs e)
        {


        }
    }
}
