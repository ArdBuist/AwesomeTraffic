using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class StartControl : UserControl
    {
        ElementHost StartHost;
        SimWindow simwindow;
		AboutWindow about;
        InterfaceStart StartScherm;

        public StartControl(Size size, SimWindow sim)
        {
            this.Size = size;
            simwindow = sim;

            StartScherm = new InterfaceStart(this);

            StartHost = new ElementHost()
            {
                Height = 300,
                Width = 300,
                Child = StartScherm
            };
			// Voeg de startknoppen toe
            this.Controls.Add(StartHost);
        }

		// Klik op "Nieuw"
        public void New_Click()
        {   
            // Open simcontrol
            simwindow.New();
        }

		// Klik op "Option"
        public void Option_Click()
        {

        }

		// Klik op "About" 
		public void About_Click()
		{
			// Niewe form met info over 't programma
			about = new AboutWindow();

			about.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// Sluit form als dit niet de 'hoofdform' is
			about.Deactivate += About_LostFocus;

			// Open de form
			about.Show();
		}

		public void About_LostFocus(object sender, EventArgs e)
		{
			// Sluit de about form
			about.Close();
		}

		// Klik op "Exit"
        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}
