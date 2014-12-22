﻿using System;
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
    public partial class StartWindow : UserControl
    {
        ElementHost StartHost;
        WindowSelect windowselect;
		AboutWindow about;
        InterfaceStart StartScherm;

        public StartWindow(Size size, WindowSelect sim)
        {
            this.Size = size;
            windowselect = sim;

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
            windowselect.New();
        }

		// Klik op "Open"
		public void Open_Click()
		{
			
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
			// Adobe Acrobat is geïnstaleerd
			try
			{
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

			// Acrobat reader is niet geïnstaleerd, selecteer programma dat wel pdf kan openen
			catch
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");
				string args = "shell32.dll,OpenAs_RunDLL " + path;

				// Start nieuw process voor acrobat reader en open de path
				Process P = new Process
				{
					StartInfo = { FileName = "rundll32.exe", Arguments = args }
				};

				// Zet process op fullscreen en open acrobat reader met het bestand
				P.Start();
			}

		}

		// Klik op "Exit"
        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}