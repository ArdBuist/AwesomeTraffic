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
        Point mouseDownPoint;
        public int tilesHorizontal;

        public SimControl(Size size)
        {
            mouseDownPoint = new Point(0,0);
            this.Size = new Size(2000, 1500);
            isBuildingMode = true;
            tilesHorizontal = Size.Width / 100;
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
            this.Paint += this.Teken;
            this.MouseDown += this.MouseClickDown;
            this.MouseMove += (object o, MouseEventArgs mea) => { if (mouseDownPoint != new Point(0,0))
                                                                   if(TileIsStraight(mouseDownPoint,mea.Location))
                                                                        DrawTile(mea);};
            this.MouseUp += (object o, MouseEventArgs mea) => { mouseDownPoint = new Point(0, 0); };
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
        private void MouseClickDown(object obj, MouseEventArgs mea)
        {
            //mouseDownPoint wordt gebruikt voor het laten functioneren van het door slepen aanbrengen van wegen
            mouseDownPoint = new Point(mea.X/100*100,mea.Y/100*100);
            DrawTile(mea);
        }
        /*controleert of de tile een rechte weg is en checkt of de weg naar de goede kant doorloopt zodat je een hele weg kunt maken door rechtdoor te slepen
        *Hierdoor kun je alleen rechte wegen door slepen op de kaart aanbrengen. Dit verhoogt het gebruiksgemak omdat het wegen leggen zo een stuk sneller gaat.
        */
        private bool TileIsStraight(Point mouseDown, Point mousePoint)
        {
        if(currentBuildTile.name == "Road")
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
            currentBuildTile = new Crossroad();
            currentBuildTile = new Fork(2);
            currentBuildTile = new Road(4, 3);
            //currentBuildTile = new Spawner(2);
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
            currentBuildTile = new Road(2, 4);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[500 / 100 * tilesHorizontal + 800 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(800, 500), 500 / 100 * tilesHorizontal + 800 / 100);
            bitmapMap.AddTile(tileImage, 8, 5);
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);

            currentBuildTile = new Road(1, 3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[400 / 100 * tilesHorizontal + 700 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(700, 400), 400 / 100 * tilesHorizontal + 700 / 100);
            bitmapMap.AddTile(tileImage, 7, 4);
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);

            currentBuildTile = new Road(1, 3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[600 / 100 * tilesHorizontal + 700 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(700, 600), 600 / 100 * tilesHorizontal + 700 / 100);
            bitmapMap.AddTile(tileImage, 7, 6);
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);

            currentBuildTile = new Road(1, 3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[400 / 100 * tilesHorizontal + 900 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(900, 400), 400 / 100 * tilesHorizontal + 900 / 100);
            bitmapMap.AddTile(tileImage, 9, 4);
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);

            currentBuildTile = new Road(1, 3);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[600 / 100 * tilesHorizontal + 900 / 100] = currentBuildTile;
            currentBuildTile.SetValues(this, new Point(900, 600), 600 / 100 * tilesHorizontal + 900 / 100);
            bitmapMap.AddTile(tileImage, 9, 6);
            currentBuildTile.setLanesHighToLow(3);
            currentBuildTile.Update(this, null, 0);
        }
    }
}
