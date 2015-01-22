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

            /// Screen set to max
            this.WindowState = FormWindowState.Maximized;

            /// Remove borders from form.
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            /// Determine screensize
            using (Graphics graphics = this.CreateGraphics()) 
            {
                widthStartScreen = Screen.PrimaryScreen.Bounds.Width;
                heightStartScreen = Screen.PrimaryScreen.Bounds.Height ;
            }

			/// Set screensize, make new simwindow and open homescreen
            screensize = new Size(widthStartScreen, heightStartScreen);
            simwindow = new SimWindow(screensize, this);
            Start();
        }

		/// <summary>
		///  Open the homescreen.
		/// </summary>
        public void Start()
        {
            this.Controls.Remove(simwindow);
            
            /// Open homescreen
            startwindow = new StartWindow(screensize, this);
            startwindow.BackColor = Color.Black;

			/// Add the control
            this.Controls.Add(startwindow);
        }


		/// <summary>
		///  Create a new field for the traffic simulation.
		/// </summary>
        public void New()
        {
            /// Remove start menu
            this.Controls.Remove(startwindow);
            /// Open simwindow
	        this.Controls.Add(simwindow);
        }

		/// <summary>
		/// Shows a messagebox with the desired message
		/// </summary>
		/// <param name="message"> string of the message that has to be shown </param>
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

		/// <summary>
		/// Shortcuts for program
		/// </summary>
		/// <param name="msg"> A Message, passed by reference, that represents the window message to process </param>
		/// <param name="keyData"> One of the Keys values that represents the key to process </param>
		/// <returns> true if the character was processed by the control; otherwise, false </returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			switch (keyData)
			{
					/// Shortcut for saving
				case (Keys.Control | Keys.S):
					this.simwindow.BovenSchermRechts.Save_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;

					/// Shortcut for homescreen
				case (Keys.Control | Keys.H):
					this.simwindow.BovenSchermRechts.Home_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;

					/// Shortcut for the info screen
				case (Keys.Control | Keys.I):
					this.simwindow.BovenSchermRechts.Info_Click(new object(), new System.Windows.RoutedEventArgs());
					return true;

					/// Shortcut for closing the form
                case (Keys.Alt | Keys.F4):
                    WindowSelect_FormClosing(null, null);
                    return true;
			}
			/// Return the base method of ProcessCmdKey for keys that are not processed by the control
			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// Close the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        private void WindowSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
			/// Stops the simulation if it is still running while the code 
			if (simwindow.simcontrol.simulation.simStarted)
			{
				simwindow.simcontrol.simulation.StartSim();
			}

            this.Close();
        }
    }
}
