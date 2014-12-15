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
        public BitmapControl bitmapMap;
        public BitmapControl trafficlightMap;
        public BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public Tile[] tileList;
        public List<Vehicle> vehicleList;
        //public List<TrafficlightControl> trafficlightList;
        public int tilesHorizontal;
        private Simulation sim;
        public PictureBox background, trafficlight, vehicle;

        public SimControl(Size size)
        {
            this.Size = new Size(1600, 900);
            isBuildingMode = true;
            tilesHorizontal = Size.Width / 100;
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
            this.Paint += this.Teken;
            this.Visible = true;
            tileList = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            vehicleList = new List<Vehicle>();
            this.sim = new Simulation(this);
            DrawStartImages();

            ////Begin test code
            //NormalCar v1 = new NormalCar(new Point(this.Size.Width/2,this.Size.Height/2), new Point(300, 300), 10, 2, 1, 1);
            //NormalCar v2 = new NormalCar(new Point(this.Size.Width/2,this.Size.Height/2), new Point(300, 300), 10, 2, 2, 1);
            //NormalCar v3 = new NormalCar(new Point(this.Size.Width/2,this.Size.Height/2), new Point(300, 300), 10, 2, 3, 1);
            //NormalCar v4 = new NormalCar(new Point(this.Size.Width/2,this.Size.Height/2), new Point(300, 300), 10, 2, 4, 1);
            ////vehicleMap.AddObject(v.Bitmap, v.position.X, v.position.Y);
            //vehicleList.Add(v1);
            //vehicleList.Add(v2);
            //vehicleList.Add(v3);
            //vehicleList.Add(v4);
            ////Einde test code

            background = new PictureBox();
            trafficlight = new PictureBox();
            vehicle = new PictureBox();

            vehicle.MouseUp += this.MouseUnclick;
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
                //debug code, maakt een wit vierkantje
                //Bitmap testObject = (Bitmap)Properties.Resources.ResourceManager.GetObject("car1");
                //Object o = System.Resources.ResourceManager.GetObject(""); //rm = Properties.Resources.ResourceManager;
                //Bitmap test = (Bitmap)rm.GetObject("car1");

                this.vehicle.Image = vehicleMap.bitmap;
                this.vehicle.BackColor = Color.Transparent;
                this.vehicle.Location = new Point(0, 0);
                this.vehicle.Size = new Size(this.Width, this.Height);
                this.background.Controls.Add(vehicle);
            }
            else
            {
                /*this.trafficlight.Image = trafficlightMap.bitmap;
                this.trafficlight.BackColor = Color.Transparent;
                this.trafficlight.Location = new Point(0, 0);
                this.trafficlight.Size = new Size(this.Width, this.Height);
                this.vehicle.Controls.Add(trafficlight);*/
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
            //currentBuildTile = new Crossroad();
            //currentBuildTile = new Fork(2 * 100);
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
            currentBuildTile.SetValues(new Point(roadX * 100, roadY * 100), roadY * tilesHorizontal + roadX);
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
        }

        public void addVehicle(Vehicle v)
        {
            vehicleList.Add(v);
        }

        private void SimControl_Load(object sender, EventArgs e)
        {

        }
    }
}
