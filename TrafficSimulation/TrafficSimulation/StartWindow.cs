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
    public partial class StartWindow : UserControl
    {
        ElementHost StartHost;
        WindowSelect windowselect;
		AboutWindow about;
        InterfaceStart interfaceStart;
		Tile[] tempTileList = new Tile[300];
        int WidthStartScreen, HeightStartScreen;

        public StartWindow(Size size, WindowSelect sim)
        {
            this.Size = size;
            windowselect = sim;

            interfaceStart = new InterfaceStart(this);
            StartHost = new ElementHost()
            {
                Height = size.Height,
                Width = size.Width,
                Location = new Point(0,0),
                Child = interfaceStart,
            };
			// Voeg de startknoppen toe   
            this.Controls.Add(StartHost);
            //StartHost.Left = (this.Size.Width-StartHost.Size.Width) / 2;
        }

		// Klik op "Nieuw"
        public void New_Click()
        {   
			/// Set current build tile to a straight road
			windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);

			/// Set state to building mode
			//windowselect.simwindow.simcontrol.state = "building";

            /// Open simcontrol
            windowselect.New();
        }

		/// <summary>
		/// When the button "Open" is clicked, this method will start.
		/// </summary>
		public void Open_Click()
		{
			Stream myStream = null;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			//openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "TrafficSimulation files (*.trs)|*.trs";
			openFileDialog1.FilterIndex = 1;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					/*
					if (windowselect.simwindow.simcontrol.tileList != null)
					{
						foreach (Tile tile in windowselect.simwindow.simcontrol.tileList)
						{
							if (tile != null)
							{
								Bitmap tileImage;
								Tile selectedTile = new removeTile();
								tileImage = selectedTile.DrawImage();
								windowselect.simwindow.simcontrol.trafficlightBC.AddObject(tileImage, tile.position.X, tile.position.Y);
							}
						}
					}
					*/

					if ((myStream = openFileDialog1.OpenFile()) != null)
					{
						using (myStream)
						{
							StreamReader r = new StreamReader(myStream);

							while (r.Peek() >= 0)
							{
								String t = r.ReadLine();

								// Char die de data splitst
								char[] splitChar = { '_' };

								// Array van info over de tile
								string[] information = t.Split(splitChar);

								Bitmap tileImage;
								Tile currentBuildTile;
								int roadX;
								int roadY;


								int tilesHorizontal = Size.Width / 100;

								switch (information[0])
								{
									case "TrafficSimulation.Fork":
										currentBuildTile = new Fork(windowselect.simwindow.simcontrol, Convert.ToInt32(information[1]));
										roadX = Convert.ToInt32(information[3]) / 100;
										roadY = Convert.ToInt32(information[4]) / 100;
										tempTileList[Convert.ToInt32(information[2])] = currentBuildTile;
										currentBuildTile.SetValues(windowselect.simwindow.simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[2]));
										tileImage = currentBuildTile.DrawImage();
										windowselect.simwindow.simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
										break;

									case "TrafficSimulation.Road":
										currentBuildTile = new Road(Convert.ToInt32(information[1]), Convert.ToInt32(information[2]));
										//currentBuildTile.LanesHighToLow = Convert.ToInt32(information[6]);
										//currentBuildTile.LanesLowToHigh = Convert.ToInt32(information[7]);
										roadX = Convert.ToInt32(information[4]) / 100;
										roadY = Convert.ToInt32(information[5]) / 100;
										tempTileList[Convert.ToInt32(information[3])] = currentBuildTile;
										currentBuildTile.SetValues(windowselect.simwindow.simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[3]));
										tileImage = currentBuildTile.DrawImage();
										windowselect.simwindow.simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);

										break;

									case "TrafficSimulation.Crossroad":
										currentBuildTile = new Crossroad(windowselect.simwindow.simcontrol);
										roadX = Convert.ToInt32(information[2]) / 100;
										roadY = Convert.ToInt32(information[3]) / 100;
										tempTileList[Convert.ToInt32(information[1])] = currentBuildTile;
										currentBuildTile.SetValues(windowselect.simwindow.simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[1]));
										tileImage = currentBuildTile.DrawImage();
										windowselect.simwindow.simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
										break;

									case "TrafficSimulation.Spawner":
										currentBuildTile = new Spawner(Convert.ToInt32(information[1]));
										roadX = Convert.ToInt32(information[3]) / 100;
										roadY = Convert.ToInt32(information[4]) / 100;
										tempTileList[Convert.ToInt32(information[2])] = currentBuildTile;
										currentBuildTile.SetValues(windowselect.simwindow.simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[2]));
										tileImage = currentBuildTile.DrawImage();
										windowselect.simwindow.simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
										break;
								}
							}
						}
					}

					windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);
					windowselect.simwindow.simcontrol.state = "selected";
					windowselect.simwindow.simcontrol.tileList = tempTileList;

					windowselect.New();
				}

				// Throw exception when something is wrong
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
