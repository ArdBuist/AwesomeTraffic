using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrafficSimulation
{
    public partial class WindowSelect : Form
    {
        StartWindow startwindow;
        public SimWindow simwindow;

        public WindowSelect()
        {
            Start();
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            int WidthStartScreen = Screen.PrimaryScreen.Bounds.Width, HeightStartScreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);
            simwindow = new SimWindow(screensize, this);

        }

        public void Start()
        {
            this.Controls.Remove(simwindow);
            int WidthStartScreen = Screen.PrimaryScreen.Bounds.Width, HeightStartScreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);

            // Openen van het startscherm
            startwindow = new StartWindow(screensize, this);
            this.BackColor = Color.Green;

            startwindow.Left = ((WidthStartScreen - 300) / 2);
            startwindow.Top = ((HeightStartScreen - 300) / 2);
            this.Controls.Add(startwindow);
        }
        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(startwindow);
            
            // Open simwindow

            simwindow.BackColor = Color.Green;
            //simwindow.Location = new Point(0, 0);
            this.Controls.Add(simwindow);
        }
        private void Close()
        {
            this.Close();
        }
    }
}
