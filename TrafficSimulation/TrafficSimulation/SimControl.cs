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
        Tile currentBuildTile;
        public BitmapControl bitmapMap;
        BitmapControl trafficlightMap;
        BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public Tile[] tiles;
        public int tilesHorizontal;
        ElementHost BovenHost, OnderHost, InfoHost;

        public SimControl(Size size)
        {
            BovenScherm BovenScherm = new BovenScherm();
            OnderScherm OnderScherm = new OnderScherm();
            InfoBalk InfoBalk = new InfoBalk();
            int hoogte = ClientSize.Height;
            int breedte = ClientSize.Width;

            this.Size = new Size(2000, 1500);
            isBuildingMode = true;
            tilesHorizontal = Size.Width / 100;
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
            this.Paint += this.Teken;
            this.MouseUp += this.MouseUnclick;
            this.Visible = true;
            tiles = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
            DrawStartImages();

            BovenHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height= 50,
                Width = ClientSize.Width,
                Child = BovenScherm,
            };
            this.Controls.Add(BovenHost);

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(0,900),
                Height= 120,
                Width = ClientSize.Width,
                Child = OnderScherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(1600, 75),
                Height = 800,
                Width = 300,
                Child = InfoBalk,
            };
            this.Controls.Add(InfoHost);
           
            Invalidate();
        }
        private void Teken(object o, PaintEventArgs pea)
        {
            //dit zorgt ervoor dat de kaart op het scherm wordt weergegeven.
            //dit hoeft alleen maar gebeuren wanneer er nog aan de kaart gewerkt wordt.
            Image image;
            if (isBuildingMode == true)
            {
                image = (Image)bitmapMap.bitmap;
                pea.Graphics.DrawImage(image, 0, 0);
            }
            else
            {
                image = (Image)vehicleMap.bitmap;
                pea.Graphics.DrawImage(image, 0, 0);
                image = (Image)trafficlightMap.bitmap;
                pea.Graphics.DrawImage(image, 0, 0);
            }
        }
        public void MouseUnclick(object obj, MouseEventArgs mea)
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
            currentBuildTile = new Crossroad();
            currentBuildTile = new Fork(2);
            currentBuildTile = new Road(4, 2);
            //currentBuildTile = new Spawner(new Point(mea.X, mea.Y), 2);
            currentBuildTile.SetValues(mea.Location,CalculateListPlace(mea.X, mea.Y));
            tileImage = currentBuildTile.DrawImage();
            currentBuildTile.Update(this, null, 0);
            tiles[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddTile(tileImage, mea.X / 100, mea.Y / 100);


            //host.BackColorTransparent = true;
            //hier moet nog een nieuwe currentBuildTile worden aangemaakt met dezelde waarden als de vorige.
            Invalidate();
        }

        public int CalculateListPlace(int mouseX, int mouseY)
        {
            return mouseY / 100 * tilesHorizontal + mouseX/ 100;
        }
        //hele methode kan weer weg zo gauw er een interface is waar we mee kunnen testen.
        private void DrawStartImages()
        {
            Bitmap tileImage;
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 500 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(500, 500), 500 / 100 * tilesHorizontal + 500 / 100);
            bitmapMap.AddTile(tileImage, 5, 5);
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 700 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(700, 500), 500 / 100 * tilesHorizontal + 700 / 100);
            bitmapMap.AddTile(tileImage, 7, 5);
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 800 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(800, 500), 500 / 100 * tilesHorizontal + 800 / 100);
            bitmapMap.AddTile(tileImage, 8, 5);
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 900 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(900, 500), 500 / 100 * tilesHorizontal + 900 / 100);
            bitmapMap.AddTile(tileImage, 9, 5);
            currentBuildTile = new Road(1,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 1000 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(1000, 500), 500 / 100 * tilesHorizontal + 1000 / 100);
            bitmapMap.AddTile(tileImage, 10, 5);
            currentBuildTile = new Spawner(3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[400 / 100 * tilesHorizontal + 1000 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(1000, 400), 400 / 100 * tilesHorizontal + 1000 / 100);
            bitmapMap.AddTile(tileImage, 10, 4);
            currentBuildTile = new Crossroad();
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 600 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(600, 500), 500 / 100 * tilesHorizontal + 600 / 100);
            bitmapMap.AddTile(tileImage, 6, 5);
            currentBuildTile = new Fork(2);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[600 / 100 * tilesHorizontal + 600 / 100] = currentBuildTile;
            currentBuildTile.SetValues(new Point(600, 600), 600 / 100 * tilesHorizontal + 600 / 100);
            bitmapMap.AddTile(tileImage, 6, 6);
            currentBuildTile = tiles[500 / 100 * tilesHorizontal + 700 / 100];
            currentBuildTile.setLanesHighToLow(1);
            currentBuildTile.Update(this, null, 0);

        }
    }
}
