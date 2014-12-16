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
        // test of mijn branch het doet
        
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
            // Deze screensizing verkloot mijn scherm
            Size Screensize = new Size(1920,1080);
            
            // Openen van het startscherm
            start = new StartControl(Screensize, this);
            this.BackColor = Color.Green;
            start.Size = Screensize;
            start.Location = new Point(0, 0);
            this.Controls.Add(start);
            

            //start.Location = new Point(((WidthStartScreen-300)/2), ((HeightStartScreen -300)/2));
            //start.Left = ((WidthStartScreen - 300) / 2);
            //start.Top = ((HeightStartScreen - 300) / 2);
            
        }

        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(start);

            // Open simcontrol
            sim = new SimControl(Screen.PrimaryScreen.Bounds.Size, this);
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
