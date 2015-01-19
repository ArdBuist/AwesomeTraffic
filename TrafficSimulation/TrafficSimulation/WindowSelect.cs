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
        public Size screensize;

        public WindowSelect()
        {
            int widthStartScreen, heightStartScreen;

            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //Schermgroote bepalen
            using (Graphics graphics = this.CreateGraphics()) 
            {
                widthStartScreen = Screen.PrimaryScreen.Bounds.Width;
                heightStartScreen = Screen.PrimaryScreen.Bounds.Height ;
            }

            screensize = new Size(widthStartScreen, heightStartScreen);
            simwindow = new SimWindow(screensize, this);
            Start();
        }

		// Open the homescreen.
        public void Start()
        {
            this.Controls.Remove(simwindow);
            
            // Open homescreen
            startwindow = new StartWindow(screensize, this);
            this.BackColor = Color.Black;

			// Add the control
            this.Controls.Add(startwindow);
        }


		// Create a new field for the traffic simulation.
        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(startwindow);

            // Open simwindow
            simwindow.BackColor = Color.Green;
            this.Controls.Add(simwindow);
        }
        private void Close()
        {
            simwindow.simcontrol.simulation.StartSim();
            this.Close();
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
