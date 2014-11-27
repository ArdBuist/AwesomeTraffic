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
        ITile currentBuildTile;
        BitmapControl bitmapMap;
        BitmapControl trafficlightMap;
        BitmapControl vehicleMap;
        Boolean isBuildingMode;//moet veranderd worden als van het kaartbouwen wordt overgesprongen naar het "spelen" 
        public ITile[] tiles;

        public SimControl(Size size)
        {
            
            this.Size = new Size(2000, 1500);
            isBuildingMode = true;
            bitmapMap = new BitmapControl(this.Size);
            trafficlightMap = new BitmapControl(this.Size);
            vehicleMap = new BitmapControl(this.Size);
            this.DoubleBuffered = false;
            this.Paint += this.Teken;
            this.MouseUp += this.MouseUnclick;
            this.Visible = true;
            tiles = new ITile[(this.Size.Height / 100) * (this.Size.Width / 100)];
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
            //Begin moet weer weg, dat is alleen om te laten zien dat het werkt
            Bitmap tileImage;
            if(mea.Button == MouseButtons.Left)
                tileImage = DrawImage("horizontalroad");
            else
                tileImage = DrawImage("crossroad");
            //voor als de code voor het zelf maken van de tiles werkt:
            //ITile[] tileList = new ITile[] {new Crossroad(), new Road(), new Fork()};
            //for (int i = 0; i < tileList.Length;i++ )
            //{
            //    ITile possibleTile = tileList[i];
            //    if(possibleTile.ToString() == currentTileString)
            //    {
            //        currentBuildTile = possibleTile;
            //        break;
            //    }
            //}
            //currentBuildTile.SetValues(hier startdirection, enddirection en alle andere nodige variabelen);
            //tileImage = currentBuildTile.DrawImage(hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden);
            //list[mea.Y/100*10+mea.X/100] = currentTile;

            //Dit zorgt ervoor dat de kaart geupdate wordt met de nieuwe tile.
            bitmapMap.AddTile(tileImage, mea.X / 100, mea.Y / 100);    
            Invalidate();
        }
        //deze methode moet anders komen in elke tileklasse, kan weg als dat klaar is. Deze is alleen maar als tussenproduct.
        private Bitmap DrawImage(string name)
        {
            ResourceManager rm = Properties.Resources.ResourceManager;
            Bitmap tile = (Bitmap)rm.GetObject(name);
            return tile;
        }
    }
}
