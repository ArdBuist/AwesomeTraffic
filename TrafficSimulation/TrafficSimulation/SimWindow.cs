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
        SimControl simControl; 
        StartControl startControl;

        // test of mijn branch het doet
        public SimWindow()
        {
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            // Laad het beginscherm
            simStart();
        }

        public void simStart()
        {
            //sim.oThread.Abort();
            this.Controls.Remove(this.simControl);
            int WidthStartScreen = Screen.PrimaryScreen.Bounds.Width, HeightStartScreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);

            // Openen van het startscherm
            startControl = new StartControl(screensize, this);
            this.BackColor = Color.Green;

            startControl.Left = ((WidthStartScreen - 300) / 2);
            startControl.Top = ((HeightStartScreen - 300) / 2);
            this.Controls.Add(startControl);
        }

        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(startControl);

            // Open simcontrol
            simControl = new SimControl(this.ClientSize, this);
            this.BackColor = Color.Green;
            simControl.Location = new Point(0, 0);
            this.Controls.Add(simControl);
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
