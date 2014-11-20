﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficSimulation
{
    public partial class SimControl : UserControl
    {
        BuildPanel buildPanel;
        ControlPanel controlPanel;
        ITile currentTile;
        BitmapControl bitmap;

        public SimControl(Size size)
        {
            bitmap = new BitmapControl(size);
            this.Size = new Size(1000,1000);
            this.Paint += this.Teken;
            this.Visible = true;
            Invalidate();
        }
        private void Teken(object o, PaintEventArgs pea)
        {
            //Dit moet bij een betere interface anders worden aangeroepen.
            bitmap.AddTile(pea.Graphics, "crossroad", 5, 5);
            bitmap.AddTile(pea.Graphics, "crossroad", 3, 3);
            bitmap.AddTile(pea.Graphics, "horizontalroad", 6, 5);
            bitmap.AddTile(pea.Graphics, "horizontalroad", 4, 5);
            bitmap.AddTile(pea.Graphics, "horizontalroad", 6, 3);
        }
    }
}
