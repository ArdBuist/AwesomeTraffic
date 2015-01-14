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
		SimControl simcontrol;
        InterfaceStart StartScherm;
		Tile[] tempTileList = new Tile[300];

        public StartWindow(Size size, WindowSelect sim)
        {
            this.Size = size;
            windowselect = sim;

			/// I use this, a lot..
			simcontrol = windowselect.simwindow.simcontrol;

			/// New start screen
            StartScherm = new InterfaceStart(this);
			
            StartHost = new ElementHost()
            {
                Height = 1000,
                Width = 1000,
                Child = StartScherm
            };

			/// Add startbuttons
            this.Controls.Add(StartHost);
            StartHost.Left = (this.Size.Width-StartHost.Size.Width) / 2;
        }

		/// <summary>
		/// Click New
		/// </summary>
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
			///Make new stream
			Stream myStream1 = null;
			Stream myStream2 = null;

			/// New open dialog
			OpenFileDialog openDialog = new OpenFileDialog();

			/// Few basic settings for the opendialog
			//openFileDialog1.InitialDirectory = "c:\\";
			openDialog.Filter = "TrafficSimulation files (*.trs)|*.trs";
			openDialog.FilterIndex = 1;
			openDialog.RestoreDirectory = true;

			if (openDialog.ShowDialog() == DialogResult.OK)
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

					if ((myStream1 = openDialog.OpenFile()) != null && (myStream2 = openDialog.OpenFile()) != null)
					{
						/// Add al the roads to the map
						using (myStream1)
						{
							StreamReader r1 = new StreamReader(myStream1);

							#region Add roads to the map

							while (r1.Peek() >= 0)
							{
								String t = r1.ReadLine();

								// Char die de data splitst
								char[] splitChar = { '_' };

								// Array van info over de tile
								string[] information = t.Split(splitChar);

								Bitmap tileImage;
								Tile currentBuildTile;
								int roadX;
								int roadY;

								int tilesHorizontal = Size.Width / 100;

								/// You need different information from different tiles
								/// So you need multiple cases, one for each tile
								/// 
								/// Basic information
								///		 0: tile
								///		 1: place in list
								///		 2: x position
								///		 3: y position
								///	Specific information
								///		 4: trafficlight strat
								///		 5: Maxspeed for a tile
								///		 6: begin direction (notDirection for Fork, direction for Spawner)
								///		 7: end direction (Crossroad doesn't have any directions)
								///		 8: laneshightolow (For crossroad and fork a number of 8 integers with the road numbers)
								///		 9: laneslowtohigh, not for crossroad and fork.
								///		10: number of 8 integers with the road numbers
								///	Green Wave info
								///		10: ?
								///		11: ?
								///		12: ?
								
								if(information[0] == "TrafficSimulation.Road")
								{
									/// Make new tile
									currentBuildTile = new Road(Convert.ToInt32(information[6]), Convert.ToInt32(information[7]));

									/// Get the location
									roadX = Convert.ToInt32(information[2]) / 100;
									roadY = Convert.ToInt32(information[3]) / 100;

									/// Set some values
									currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[1]));
									currentBuildTile.LanesHighToLow = Convert.ToInt32(information[8]);
									currentBuildTile.LanesLowToHigh = Convert.ToInt32(information[9]);
									currentBuildTile.maxSpeed = Convert.ToInt32(information[5]);

									/// Add to list
									tempTileList[Convert.ToInt32(information[1])] = currentBuildTile;

									/// Draw the tile
									tileImage = currentBuildTile.DrawImage();
									simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);
								}
							}

							#endregion
						}
						
						foreach (Tile tile in tempTileList)
						{
							if (tile != null)
							{
								windowselect.simwindow.simcontrol.tileList[tile.listPlace] = tempTileList[tile.listPlace];
							}
						}

						/// Add the rest to the map
						using (myStream2)
						{
							StreamReader r2 = new StreamReader(myStream2);

							#region Add the rest to the map

							while (r2.Peek() >= 0)
							{
								String t = r2.ReadLine();

								// Char die de data splitst
								char[] splitChar = { '_' };

								// Array van info over de tile
								string[] information = t.Split(splitChar);

								Bitmap tileImage;
								Tile currentBuildTile;
								int roadX;
								int roadY;

								int tilesHorizontal = Size.Width / 100;

								/// You need different information from different tiles
								/// So you need multiple cases, one for each tile
								/// 
								/// Basic information
								///		 0: tile
								///		 1: place in list
								///		 2: x position
								///		 3: y position
								///	Specific information
								///		 4: trafficlight strat
								///		 5: Maxspeed for a tile
								///		 6: begin direction (notDirection for Fork, direction for Spawner)
								///		 7: end direction (Crossroad doesn't have any directions)
								///		 8: laneshightolow (For crossroad and fork a number of 8 integers with the road numbers)
								///		 9: laneslowtohigh, not for crossroad and fork.
								///		10: number of 8 integers with the road numbers
								///	Green Wave info
								///		10: ?
								///		11: ?
								///		12: ?

								switch (information[0])
								{
									/// Load a fork into the list
									case "TrafficSimulation.Fork":
										/// Make new tile
										currentBuildTile = new Fork(simcontrol, Convert.ToInt32(information[6]));

										/// Get the location
										roadX = Convert.ToInt32(information[2]) / 100;
										roadY = Convert.ToInt32(information[3]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[1]));
										currentBuildTile.maxSpeed = Convert.ToInt32(information[5]);
										/*currentBuildTile.strat = Convert.ToInt32(information[4]);*/

										/// Add to list
										tempTileList[Convert.ToInt32(information[1])] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);

										break;

									/// Load a crossroad to the list
									case "TrafficSimulation.Crossroad":
										/// Make new tile
										currentBuildTile = new Crossroad(simcontrol);

										/// Get location
										roadX = Convert.ToInt32(information[2]) / 100;
										roadY = Convert.ToInt32(information[3]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[1]));
										currentBuildTile.maxSpeed = Convert.ToInt32(information[5]);
										//currentBuildTile.strat = Convert.ToInt32(information[4]);

										/// Add to list
										tempTileList[Convert.ToInt32(information[1])] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);

										break;

									/// Load a spawner to the list
									case "TrafficSimulation.Spawner":
										/// Make new tile
										currentBuildTile = new Spawner(Convert.ToInt32(information[6]));

										/// Get location
										roadX = Convert.ToInt32(information[2]) / 100;
										roadY = Convert.ToInt32(information[3]) / 100;

										/// Set some values
										currentBuildTile.SetValues(simcontrol, new Point((roadX * 100), roadY * 100), Convert.ToInt32(information[1]));
										currentBuildTile.maxSpeed = Convert.ToInt32(information[5]);
										currentBuildTile.UpdateLanes(simcontrol, Convert.ToInt32(information[6]), Convert.ToInt32(information[8]), Convert.ToInt32(information[9]));

										/// Add to list
										tempTileList[Convert.ToInt32(information[1])] = currentBuildTile;

										/// Draw the tile
										tileImage = currentBuildTile.DrawImage();
										simcontrol.backgroundBC.AddObject(tileImage, roadX * 100, roadY * 100);

										break;
								}
							}

							#endregion
						}

						foreach (Tile tile in tempTileList)
						{
							if (tile != null)
							{
								windowselect.simwindow.simcontrol.tileList[tile.listPlace] = tempTileList[tile.listPlace];
							}
						}
					}

					windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);
					windowselect.simwindow.simcontrol.state = "selected";
					//windowselect.simwindow.simcontrol.tileList = tempTileList;

					windowselect.New();
				}

				// Throw exception when something is wrong
				catch (Exception ex)
				{
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Click on option...?
		/// </summary>
        public void Option_Click()
        {

        }

		/// <summary>
		/// Click About
		/// </summary> 
		public void About_Click()
		{
			/// New form with info about the program
			about = new AboutWindow();
			about.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			/// Close form on deactivation
			about.Deactivate += About_LostFocus;

			/// Open form
			about.Show();
		}

		/// <summary>
		/// When AboutForm is not the main form anymore
		/// </summary>
		public void About_LostFocus(object sender, EventArgs e)
		{
			/// Close form
			about.Close();
		}

		/// <summary>
		/// Click How-To
		/// </summary>
		public void HowTo_Click()
		{
			/// Adobe Acrobat is geïnstaleerd
			try
			{
				/// Path of the file
				string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");

				/// Make new process and open set Adobe Acrobat as the program to open
				Process P = new Process
				{
					StartInfo = { FileName = "AcroRd32.exe", Arguments = path }
				};

				/// Set process to fullscreen and start the procees
				P.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
				P.Start();
			}
			/// Acrobat reader is not installed, the user has to select his own program
			catch
			{
				string path = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Projectdocument.pdf");
				string args = "shell32.dll,OpenAs_RunDLL " + path;

				/// Make new process and open the program searcher thing
				Process P = new Process
				{
					StartInfo = { FileName = "rundll32.exe", Arguments = args }
				};

				/// Set process to fullscreen and start the process
				P.Start();
			}
		}

		/// <summary>
		/// Click Exit
		/// </summary>
        public void Exit_Click()
        {
			/// Stop the simulation (if true)
			if (windowselect.simwindow.simcontrol.simulation.simStarted == true)
			{
				windowselect.simwindow.simcontrol.simulation.thread.Abort();
				windowselect.simwindow.simcontrol.simulation.simStarted = false;
			}

            /// Close application
            Application.Exit();


        }
    }
}
