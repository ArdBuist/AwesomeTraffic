using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Resources;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class BitmapControl
    {
        public BitmapControl(Size size)
        {
        }
        private void DrawBitmap()
        {
            
        }
        public void Teken(Graphics gr)
        {
        }
        public void AddTile(Graphics gr,string tileName, int x,int y)
        {
            //afbeelding uit de resources halen en in de grid op een plek invoegen.
            ResourceManager rm = Properties.Resources.ResourceManager;
            Bitmap tile = (Bitmap)rm.GetObject(tileName);
            gr.DrawImage(tile, x*100, y*100);
        }
    }
}
