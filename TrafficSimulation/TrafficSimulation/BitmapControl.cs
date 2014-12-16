using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Resources;
using System.Drawing.Design;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class BitmapControl
    {

        public Bitmap bitmap;

        // altijd aanroepen als er in de bitmap getekend moet worden.
        public Graphics GetBitmapGraphics { get { 
            return Graphics.FromImage(bitmap); } }

        /*
         * 
         */
        public BitmapControl(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }
        /*
         * 
         */
        private void DrawBitmap()
        {

        }
        /*
         * zorgt ervoor dat de nieuwe tile op de kaart komt in het grid.
         */
        public void AddObject(Bitmap bitmap, int x, int y)
        {
            Graphics gr = GetBitmapGraphics;
            gr.DrawImage(bitmap, x, y);
        }
    }
}
