﻿using System;
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
        public BitmapControl(Size size)
        {
            bitmap = new Bitmap(size.Width, size.Height);
        }

        public void AddGrid()
        {
            Graphics gr = GetBitmapGraphics;
            /// Adds a grid to the map
            float[] dashValues = { 5, 3 };
            Pen blackPen = new Pen(Color.DarkGreen, 1);
            blackPen.DashPattern = dashValues;
            for (int x = 0; x < bitmap.Size.Width; x += 100)
                gr.DrawLine(blackPen, new Point(x, 0), new Point(x, bitmap.Size.Height));
            for (int y = 0; y < bitmap.Size.Height; y += 100)
                gr.DrawLine(blackPen, new Point(0, y), new Point(bitmap.Size.Width, y));
            gr.DrawLine(blackPen, new Point(0, bitmap.Size.Height-1), new Point(bitmap.Size.Width, bitmap.Size.Height-1));
            gr.DrawLine(blackPen, new Point(bitmap.Size.Width - 1, 0), new Point(bitmap.Size.Width - 1, bitmap.Size.Height));
        }
        // altijd aanroepen als er in de bitmap getekend moet worden.
        public Graphics GetBitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }

        public void AddObject(Bitmap bitmap, Point p)
        {//zorgt ervoor dat de nieuwe tile op de kaart komt in het grid.
            Graphics gr = GetBitmapGraphics;
            gr.DrawImage(bitmap, p.X, p.Y);

        }
    }
}
