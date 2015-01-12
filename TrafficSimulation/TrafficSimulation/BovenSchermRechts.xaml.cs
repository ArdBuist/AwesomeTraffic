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

				string path = saveDialog.InitialDirectory.ToString() + "/" + filename + ".trs";
				
				/// Extension name (.trs => TRafficSimulation)
				saveDialog.DefaultExt = ".trs";
				//saveDialog.InitialDirectory = true;

				while (File.Exists(path))
				{
					number++;
					filename = "Traffic" + number.ToString();
					path = saveDialog.InitialDirectory.ToString() + filename + ".trs";
				}

				saveDialog.FileName = filename;

				/// Set the ability to overwrite another file to true
				saveDialog.OverwritePrompt = true;

				/// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					StreamWriter file = new StreamWriter(@saveDialog.FileName);
					file.AutoFlush = true;

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
							/// 0: tile name
							/// 1: place in list
							/// 2: x position
							/// 3: y position
							/// 4: Maxspeed for a tile
							/// 5: begin direction (notDirection for Fork, direction for Spawner)
							/// 6: end direction (Crossroad doesn't have any directions)
							/// 7: laneshightolow, not for crossroad and fork.
							/// 8: laneslowtohigh, not for crossroad and fork.
							/// 9: -
							switch (currenttile)
							{
								/// Save case for a fork
								case "Fork":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										tile.maxSpeed + "_" +		// 4 Maxspeed
										tile.notDirection);			// 5 De not direction
									break;

								/// Save case for a crossroad
								case "Crossroad":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										tile.maxSpeed);				// 4 Maxspeed
									break;

								/// Save case for a road (that is a straight road or a curved road)
								case "Road":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										tile.maxSpeed + "_"	+		// 4 Maxpeed
										tile.startDirection + "_" +	// 5 Begin richting
										tile.endDirection + "_" +	// 6 Eind richting
										tile.LanesHighToLow + "_" + // 7 Wegen hoog, laag
										tile.LanesLowToHigh);		// 8 Wegen laag, hoog
									break;

								/// Save case for a spawner
								case "Spawner":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										tile.maxSpeed + "_" +		// 4 Maxspeed
										tile.direction + "_" +		// 5 Richting
										" " + "_" +					// 6 Empty
										tile.LanesHighToLow + "_" +	// 7 LanesHighToLow
										tile.LanesLowToHigh);		// 8 LanesLowToHigh
									break;

								// TODO: Save options for extra info, such as greenwave, traffic lights
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
