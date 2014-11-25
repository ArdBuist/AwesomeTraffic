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
        string currentTile;
        BitmapControl bitmapMap;
        public ITile[] tiles;

        public SimControl(Size size)
        {
            
            this.Size = new Size(2000, 1500);
            bitmapMap = new BitmapControl(this.Size);
            this.DoubleBuffered = true;
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
            Image image = (Image)bitmapMap.bitmap;
            pea.Graphics.DrawImage(image, 0, 0);
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
            //tileImage = currentTile.DrawImage(hier de variabelen die nodig zijn en vanaf de interface doorgegeven moeten worden);
            //currentTile.SetValues(hier startdirection, enddirection en alle andere nodige variabelen);
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
