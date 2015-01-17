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

			Graphics gr = GetBitmapGraphics;

			/// Adds a grid to the map
			float[] dashValues = { 5, 3};
			Pen blackPen = new Pen(Color.DarkGreen, 1);
			blackPen.DashPattern = dashValues;
			for (int x = 0; x < size.Width; x += 100 )
				gr.DrawLine(blackPen, new Point(x, 0), new Point(x, size.Height));
			for (int y = 0; y < size.Height; y += 100)
				gr.DrawLine(blackPen, new Point(0, y), new Point(size.Width, y));
        }
        // altijd aanroepen als er in de bitmap getekend moet worden.
        public Graphics GetBitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }

        public void AddObject(Bitmap bitmap, int x, int y)
        {//zorgt ervoor dat de nieuwe tile op de kaart komt in het grid.
            Graphics gr = GetBitmapGraphics;
            gr.DrawImage(bitmap, x, y);

        }
    }
}
