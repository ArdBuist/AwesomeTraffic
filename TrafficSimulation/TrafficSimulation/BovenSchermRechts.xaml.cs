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
        InfoBalk infoBalk;
        OnderScherm onderScherm;
        WindowSelect windowselect;
        Boolean day = false;
        int breedteScherm, breedteInfoBalk, hoogteBovenBalk;

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
			// Hide info
            if (InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point((breedteScherm - breedteInfoBalk), hoogteBovenBalk);
                InfoVisible = false;
            }
			// Show info
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                ShowComboBox(windowselect.simwindow.simcontrol.selectedTile.Directions);
                InfoVisible = true;
            }
        }

		// Gets the user to the homescreen
		private void Home_Click(object sender, RoutedEventArgs e)
        {
            windowselect.Start();

			if (windowselect.simwindow.simcontrol.simulation.simStarted == true)
			{
				windowselect.simwindow.simcontrol.simulation.thread.Abort();
				windowselect.simwindow.simcontrol.simulation.simStarted = false;
			}
        }

		// Method for saving the map.
		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				// New savedialog
				System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

				// Custom filename
				int number = 1;
				string filename = "Traffic" + number.ToString();

				string path = saveDialog.InitialDirectory.ToString() + "/" + filename + ".trs";
				
				// Extension name (.trs => TRafficSimulation)
				saveDialog.DefaultExt = ".trs";
				//saveDialog.InitialDirectory = true;

				while (File.Exists(path))
				{
					number++;
					filename = "Traffic" + number.ToString();
					path = saveDialog.InitialDirectory.ToString() + filename + ".trs";
				}

				saveDialog.FileName = filename;

				// Set the ability to overwrite another file to true
				saveDialog.OverwritePrompt = true;

				// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					StreamWriter file = new StreamWriter(@saveDialog.FileName);
					file.AutoFlush = true;

					// Get every tile in the list
					foreach (Tile tile in windowselect.simwindow.simcontrol.tileList)
					{
						// If the tile has some value asigned to it
						if (tile != null)
						{
							String currenttile = tile.name;

							// You need different information from different tiles
							// So you need multiple cases, one for each tile
							switch (currenttile)
							{
								/// Save case for a fork
								case "Fork":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.notDirection + "_" +	// 1 De not direction
										tile.listPlace + "_" +		// 2 Plaats in de lijst
										tile.position.X + "_" +		// 3 X positie
										tile.position.Y);			// 4 Y positie
									// lanes
									break;

								// Save case for a crossroad
								case "Crossroad":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y);			// 3 Y positie
									// lanes
									break;

								// Save case for a road (that is a straight road or a curved road)
								case "Road":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.startDirection + "_" +	// 1 Begin richting
										tile.endDirection + "_" +	// 2 Eind richting
										tile.listPlace + "_" +		// 3 Plaats in de lijst
										tile.position.X + "_" +		// 4 X positie
										tile.position.Y + "_" /* +		// 5 Y positie
										tile.LanesHighToLow + "_" + // 6 Wegen hoog, laag
										tile.LanesLowToHigh*/);		// 7 Wegen laag, hoog
									break;

								// Save case for a spawner
								case "Spawner":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.direction + "_" +		// 1 Richting
										tile.listPlace + "_" +		// 2 Plaats in de lijst
										tile.position.X + "_" +		// 3 X positie
										tile.position.Y);			// 4 Y positie
									// lanes
									// carspersecond
									// other info
									// more info
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

       
        // Method for day and night.
        /*
        private void DayNight_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bm = new BitmapImage(new Uri("@Buttons/Day_Button.png", UriKind.RelativeOrAbsolute));
            imageDay.Source = bm;
            
            if (day == true)
            {
                imageDay.Source = bm;
                day = false;
            }
            else if (day == false)
            {
                imageDay.Source = bm;
                day = true;
            }
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
         * */

    }
}
