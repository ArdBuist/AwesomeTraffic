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
            
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            int WidthStartScreen;
            int HeightStartScreen;
            using (Graphics graphics = this.CreateGraphics()) 
            {
                WidthStartScreen = (Screen.PrimaryScreen.Bounds.Width * (int)graphics.DpiX) / 96;
                HeightStartScreen = (Screen.PrimaryScreen.Bounds.Height * (int) graphics.DpiY) / 96;
            }
            
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);
            simwindow = new SimWindow(screensize, this);
            Start();
        }

		/// <summary>
		/// Open the homescreen.
		/// </summary>
        public void Start()
        {
            this.Controls.Remove(simwindow);
            int widthstartscreen = Screen.PrimaryScreen.Bounds.Width, heightstartscreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(widthstartscreen, heightstartscreen);

            /// Open homescreen
            startwindow = new StartWindow(screensize, this);
            startwindow.BackColor = Color.Black;

            //startwindow.Left = ((widthstartscreen - 300) / 2);
            //startwindow.Top = ((heightstartscreen - 300) / 2);

			/// Add the control
            this.Controls.Add(startwindow);
        }

		/// <summary>
		/// Create a new field for the traffic simulation.
		/// </summary>
        public void New()
        {
            /// Verwijder start menu
            this.Controls.Remove(startwindow);
            /// Open simwindow
	            this.Controls.Add(simwindow);
        }


        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
				case (Keys.Control | Keys.S):
					this.simwindow.BovenSchermRechts.Save_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;
				case (Keys.Control | Keys.H):
					this.simwindow.BovenSchermRechts.Home_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;
				case (Keys.Control | Keys.I):
					this.simwindow.BovenSchermRechts.Info_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;
                case (Keys.Alt | Keys.F4):
                    WindowSelect_FormClosing(null, null);
                    return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

        private void WindowSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(simwindow.simcontrol.simulation.simStarted)
                simwindow.simcontrol.simulation.StartSim();
            this.Close();

        }
    }
}
