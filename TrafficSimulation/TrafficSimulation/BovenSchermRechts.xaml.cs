using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for BovenScherm.xaml
    /// </summary>
    public partial class BovenSchermRechts : UserControl
    {
        public Boolean InfoVisible = true;
        //private bool simulationStarted = false;
        InfoBalk infoBalk;
        OnderScherm onderScherm;
        WindowSelect windowselect;
        int breedteScherm, breedteInfoBalk, hoogteBovenBalk;
        //int daynightcount = 0;

        public BovenSchermRechts(WindowSelect ws,  InfoBalk info, OnderScherm Onder, int bs, int bib, int hbb)
        {
            windowselect = ws;
            infoBalk = info;
            onderScherm = Onder;
            breedteScherm = bs;
            breedteInfoBalk = bib;
            hoogteBovenBalk = hbb;
            InitializeComponent();
        }

		/// <summary>
		/// Opens and closes the information thing.
		/// It's by default hidden.
		/// </summary>
        public void Info_Click(object sender, RoutedEventArgs e)
        {
			/// Hide info
            if (InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point((breedteScherm - breedteInfoBalk), hoogteBovenBalk);
                InfoVisible = false;
            }
			// Show info
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                InfoVisible = true;
            }
        }

		/// <summary>
		/// Gets the user to the homescreen
		/// </summary>
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            windowselect.Start();

			if (windowselect.simwindow.simcontrol.simulation.simStarted == true)
			{
				windowselect.simwindow.simcontrol.simulation.thread.Abort();
				windowselect.simwindow.simcontrol.simulation.simStarted = false;
			}
        }

		/// <summary>
		/// Method for saving the map.
		/// </summary>
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				/// New savedialog
				System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

				/// Custom filename
				int number = 1;
				string filename = "Traffic" + number.ToString();

				
				/// Extension name (.trs => TRafficSimulation)
				saveDialog.DefaultExt = ".trs";
				saveDialog.FilterIndex = 1;
				saveDialog.Filter = "Traffic Simulation Files (*.trs) | *.trs";
				saveDialog.RestoreDirectory = true;


				/// Set the ability to overwrite another file to true
				saveDialog.OverwritePrompt = true;

				/*
				string path = saveDialog.InitialDirectory.ToString() + "/" + filename + ".trs";

				while (File.Exists(path))
				{
					number++;
					filename = "Traffic" + number.ToString();
					path = saveDialog.InitialDirectory.ToString() + filename + ".trs";
				}
				*/

				/// Set the filename
				saveDialog.FileName = filename;

				/// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					/// New file
					StreamWriter file = new StreamWriter(@saveDialog.FileName);

					/// File can be bigger than 1024
					file.AutoFlush = true;

					/// Save some basic information
					// Building tile:
					// file.WriteLine();
					// Information (not) visible
					// file.WriteLine();

					/// Get every tile in the list
					foreach (Tile tile in windowselect.simwindow.simcontrol.tileList)
					{
						/// If the tile has some value asigned to it
						if (tile != null)
						{
							String currenttile = tile.name;

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
							///	Green Wave info
							///		10: ?
							///		11: ?
							///		12: ?

							switch (currenttile)
							{
								/// Save case for a fork
								case "Fork":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " /*tile.strat*/ + "_" + 	// 4 strat
										tile.maxSpeed + "_" +		// 5 Maxspeed
										tile.notDirection);			// 6 De not direction
									break;

								/// Save case for a crossroad
								case "Crossroad":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " /*tile.strat*/ + "_" + 	// 4 strat
										tile.maxSpeed);				// 5 Maxspeed
									break;

								/// Save case for a road (that is a straight road or a curved road)
								case "Road":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " + "_" +					// 4 Empty
										tile.maxSpeed + "_"	+		// 5 Maxpeed
										tile.startDirection + "_" +	// 6 Begin richting
										tile.endDirection + "_" +	// 7 Eind richting
										tile.LanesHighToLow + "_" + // 8 Wegen hoog, laag
										tile.LanesLowToHigh);		// 9 Wegen laag, hoog
									break;

								/// Save case for a spawner
								case "Spawner":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " + "_" +					// 4 Empty
										tile.maxSpeed + "_" +		// 5 Maxspeed
										tile.direction + "_" +		// 6 Richting
										" " + "_" +					// 7 Empty
										tile.LanesHighToLow + "_" +	// 8 LanesHighToLow
										tile.LanesLowToHigh);		// 9 LanesLowToHigh
									break;

								// TODO: Save options for extra info, traffic lights strat and other things
							}
						}
					}
				}
			}
			/// Throw an exception
			catch (Exception exp)
			{
				MessageBox.Show("" + exp);

				// TODO: Make a better screen. This isn't very useful to many users...
			}
		}	
        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Method for day and night.
        /// </summary>
        private void DayNight_Click(object sender, RoutedEventArgs e)
        {
            //if (windowselect.simwindow.Day == true)
            //{
            //    DayNight.Content = "Day";
            //    s.Day = false;
            //}
            //else
            //{
            //    DayNight.Content = "Night";
            //    s.Day = true;
            //}
        }

    }
}
