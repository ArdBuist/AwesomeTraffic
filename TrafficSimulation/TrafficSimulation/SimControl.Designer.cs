using System.Drawing;
using System.Windows.Forms;

namespace TrafficSimulation
{
    partial class SimControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SimControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SimControl";
            this.Load += new System.EventHandler(this.SimControl_Load);
            this.ResumeLayout(false);
        #endregion

            #region Code for creating all of the objects
            /*
             * PictureBox in which the background is saved, all the tiles in the simulation will be placed on the bitmap
             * in this PictureBox.
             */
            this.backgroundPB = new PictureBox();
            this.backgroundPB.Image = backgroundBC.bitmap;
            this.backgroundPB.BackColor = Color.Transparent;
            this.backgroundPB.Location = new Point(0, 0);
            this.backgroundPB.Size = new Size(this.Width, this.Height);
            /*
             * PictureBox in which the vehicles are saved, all the vehicles in the simulation will be placed on the bitmap
             * in this PictureBox. When the vehicles start to move the bitmaps will also move across the bitmap.
             */
            this.vehiclePB = new PictureBox();
            this.vehiclePB.Image = vehicleBC.bitmap;
            this.vehiclePB.BackColor = Color.Transparent;
            this.vehiclePB.Location = new Point(0, 0);
            this.vehiclePB.Size = new Size(this.Width, this.Height);
            /*
             * PictureBox in which the trafficlights are saved, all the trafficlights in the simulation will be placed on the 
             * bitmap in this PictureBox. When the trafficlights start to change color, the bitmap will update the lightcolors. 
             */
            this.trafficlightPB = new PictureBox();
            this.trafficlightPB.Image = trafficlightBC.bitmap;
            this.trafficlightPB.BackColor = Color.Transparent;
            this.trafficlightPB.Location = new Point(0, 0);
            this.trafficlightPB.Size = new Size(this.Width, this.Height);

            this.Controls.Add(backgroundPB);
            this.backgroundPB.Controls.Add(vehiclePB);
            this.vehiclePB.Controls.Add(trafficlightPB);
            #endregion

            #region ClickEvents
            trafficlightPB.MouseDown += MouseDownEvent;
            trafficlightPB.MouseMove += MouseMoveEvent;
            trafficlightPB.MouseUp += MouseClickUp;
        }

        private void MouseDownEvent(object o, MouseEventArgs mea)
        {
            mouseDownPoint = new Point(mea.X / 100 * 100, mea.Y / 100 * 100);
            mouseMovePoint = mea.Location;
        }

        private void MouseMoveEvent(object o, MouseEventArgs mea)
        {
            if (mouseDownPoint != new Point(0, 0))
            {
                if (TileIsStraight(mouseDownPoint, mea.Location))
                    DrawTile(mea);
                if (selected == true)
                {
                    MoveMap(mea);
                }
            }
        }

        private void MouseClickUp(object obj, MouseEventArgs mea)
        {
            mouseDownPoint = new Point(0, 0); mouseMovePoint = new Point(0, 0);

            /*deze code moet worden gedaan zo als de simulatie wordt gestart.*/
            foreach (Tile t in tileList)
            {
                if (t != null)
                {
                    if (t.name == "Crossroad")
                    {
                        Crossroad Cr = (Crossroad)t;
                        if (Cr.control != null)
                        {
                            controlList.Add(Cr.control);
                        }
                    }
                    if (t.name == "Fork")
                    {
                        Fork f = (Fork)t;
                        if (f.control != null)
                        {
                            controlList.Add(f.control);
                        }
                    }
                }
            }
            //de eerder geselecteerde tile wordt opnieuw getekend en verwijdert zo de blauwe rand
            if (oldselectedTile != null)
            {
                backgroundBC.AddObject(oldselectedTile.DrawImage(), oldselectedTile.position.X, oldselectedTile.position.Y);
            }
            if (selected == true) //als de select-tool is aangeklikt
            {
                DrawSelectLine(mea);
            }
            //als de gum-tool is aangeklikt
            if (eraser == true)
            {
                removeTile(mea);
            }
            //als je een weg wil bouwen
            if (building == true)
            {
                DrawTile(mea);
            }
            //host.BackColorTransparent = true;
        }
            #endregion
    }
}
