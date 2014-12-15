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

            // Openen van het startscherm
            start = new StartControl(this.ClientSize, this);
            this.BackColor = Color.Green;
            start.Left = (Screen.PrimaryScreen.Bounds.Width - 300) / 2;
            start.Top = (Screen.PrimaryScreen.Bounds.Height - 300) / 2;
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
