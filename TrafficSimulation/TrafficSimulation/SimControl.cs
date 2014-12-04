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
        BitmapControl bitmapMap;
        BitmapControl trafficlightMap;
        BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public Tile[] tiles;

        public SimControl(Size size)
        {
            
            this.Size = new Size(2000, 1500);
            isBuildingMode = true;
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
            this.Paint += this.Teken;
            this.MouseUp += this.MouseUnclick;
            this.Visible = true;
            tiles = new Tile[(this.Size.Height / 100) * (this.Size.Width / 100)];
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
            currentBuildTile = new Crossroad(new Point(mea.X, mea.Y), 60, 2, 2, 2, 2);
            currentBuildTile = new Fork(new Point(mea.X, mea.Y), 60, 2, 2, 2, 2);
            currentBuildTile = new Road(new Point(mea.X, mea.Y), 60, 3, 2, 1, 4);
            currentBuildTile = new Spawner(new Point(mea.X, mea.Y), 60, 1, 3, 10);
            //currentBuildTile.SetValues(hier startdirection, enddirection en alle andere nodige variabelen);
            tileImage = currentBuildTile.DrawImage(/*hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden*/);
            tiles[mea.Y/100*10+mea.X/100] = currentBuildTile;

            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddTile(tileImage, mea.X / 100, mea.Y / 100);    
            Invalidate();
        }
    }
}
