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
        BitmapControl trafficlightMap;
        BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public Tile[] tiles;
        public int tilesHorizontal;

        public SimControl(Size size)
        {
            
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
        private void MouseUnclick(object obj, MouseEventArgs mea)
        {
            Bitmap tileImage;
            currentBuildTile = new Crossroad();
            currentBuildTile = new Fork(2);
            currentBuildTile = new Road(4, 2);
            //currentBuildTile = new Spawner(new Point(mea.X, mea.Y), 2);
            currentBuildTile.SetValues(this,mea.Location,CalculateListPlace(mea.X, mea.Y));
            tileImage = currentBuildTile.DrawImage();
            tiles[CalculateListPlace(mea.X, mea.Y)] = currentBuildTile;
            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddTile(tileImage, mea.X / 100, mea.Y / 100);
            currentBuildTile = CopyCurrentTile();//hier wordt een nieuwe buildTile gemaakt met dezelfde waardes als daarvoor omdat er dan opnieuw een tile ingeklikt kan worden.
            Invalidate();
        }
        //methode maakt een kopie van de huidige tile die net getekend is, zodat dezelfde tile nog een keer getekend kan worden.
        private Tile CopyCurrentTile() 
        { 
            Tile tile;
            string tileName = currentBuildTile.name;
            switch(tileName)
            {
                case "Spawner":     Spawner currentSpawnerTile = (Spawner) currentBuildTile;
                                    tile = new Spawner(currentSpawnerTile.direction);
                    break;
                case "Crossroad":   tile = new Crossroad();
                    break;
                case "Road" :       Road currentRoadTile = (Road) currentBuildTile;
                                    tile = new Road(currentRoadTile.startDirection, currentRoadTile.endDirection);
                    break;
                case "Fork" :       Fork currentForkTile = (Fork) currentBuildTile;
                                    tile = new Fork(currentForkTile.notDirection);
                    break;
                default :           tile = new Crossroad();
                    break;
            }
            return tile;
        }
        private int CalculateListPlace(int mouseX, int mouseY)
        {
            return mouseY / 100 * tilesHorizontal + mouseX/ 100;
        }
        //hele methode kan weer weg zo gauw er een interface is waar we mee kunnen testen.
        private void DrawStartImages()
        {
            Bitmap tileImage;
            
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 700 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(700, 500), 500 / 100 * tilesHorizontal + 700 / 100);
            bitmapMap.AddTile(tileImage, 7, 5);
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 800 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(800, 500), 500 / 100 * tilesHorizontal + 800 / 100);
            bitmapMap.AddTile(tileImage, 8, 5);
            currentBuildTile = new Road(2,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 900 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(900, 500), 500 / 100 * tilesHorizontal + 900 / 100);
            bitmapMap.AddTile(tileImage, 9, 5);
            currentBuildTile = new Road(1,4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 1000 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(1000, 500), 500 / 100 * tilesHorizontal + 1000 / 100);
            bitmapMap.AddTile(tileImage, 10, 5);
            currentBuildTile = new Spawner(3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[400 / 100 * tilesHorizontal + 1000 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(1000, 400), 400 / 100 * tilesHorizontal + 1000 / 100);
            bitmapMap.AddTile(tileImage, 10, 4);
            currentBuildTile = new Crossroad();
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 600 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(600, 500), 500 / 100 * tilesHorizontal + 600 / 100);
            bitmapMap.AddTile(tileImage, 6, 5);
            currentBuildTile = new Fork(4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[600 / 100 * tilesHorizontal + 600 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this,new Point(600, 600), 600 / 100 * tilesHorizontal + 600 / 100);
            bitmapMap.AddTile(tileImage, 6, 6);
            currentBuildTile = tiles[500 / 100 * tilesHorizontal + 700 / 100];
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);
            currentBuildTile = new Road(2, 4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 500 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(500, 500), 500 / 100 * tilesHorizontal + 500 / 100);
            bitmapMap.AddTile(tileImage, 5, 5);
            currentBuildTile.setLanesHighToLow(2);
            currentBuildTile.Update(this, null, 0);

        }
    }
}
