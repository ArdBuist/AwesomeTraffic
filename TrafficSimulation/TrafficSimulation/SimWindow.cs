using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class SimWindow : Form
    {
        SimControl sim; 
        StartControl start;

        public SimWindow()
        {
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            // Laad het beginscherm
            Start();
        }

        public void Start()
        {
            this.Controls.Remove(sim);
            int WidthStartScreen = Screen.PrimaryScreen.Bounds.Width, HeightStartScreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);

            // Openen van het startscherm
            start = new StartControl(screensize, this);
            this.BackColor = Color.Green;

            start.Left = ((WidthStartScreen - 300) / 2);
            start.Top = ((HeightStartScreen - 300) / 2);
            this.Controls.Add(start);
        }

        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(start);

            // Open simcontrol
            sim = new SimControl(this.ClientSize, this);
            this.BackColor = Color.Green;
            sim.Location = new Point(0, 0);
            this.Controls.Add(sim);
        }

        public void Options()
        {

        }

        public void Save()
        {

        }

        public void Open()
        {

        }

        private void Close()
        {
            this.Close();
        }

        private void SimWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
