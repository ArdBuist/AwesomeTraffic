using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

		// Klik op "Open"
		public void Open_Click()
		{
			// De hierop volgende code is grotendeels van http://msdn.microsoft.com/en-us/library/system.io.stream%28v=vs.110%29.aspx

			Stream myStream = null;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					if ((myStream = openFileDialog1.OpenFile()) != null)
					{
						using (myStream)
						{
							// Insert code to read the stream here.
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
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

			// Sluit form als dit niet de active form is
			about.Deactivate += About_LostFocus;

			// Open de form
			about.Show();
		}

		// Als de form niet meer active form is
		public void About_LostFocus(object sender, EventArgs e)
		{
			// Sluit de about form
			about.Close();
		}

		// Klik op "How To"
		public void HowTo_Click()
		{
			// Bij deze code ga ik er nog vanuit dat iedereen acrobat reader heeft geïnstaleerd.

			// path van de file
			string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");

			// Start nieuw process voor acrobat reader en open de path
			Process P = new Process
			{
				StartInfo = { FileName = "AcroRd32.exe", Arguments = path }		
			};

			// Zet process op fullscreen en open acrobat reader met het bestand
			P.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
			P.Start();

		}

		// Klik op "Exit"
        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}
