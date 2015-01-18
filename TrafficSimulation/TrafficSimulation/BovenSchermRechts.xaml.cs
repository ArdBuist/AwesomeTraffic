using System;
using System.CodeDom.Compiler;
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
        public Boolean InfoVisible = false;
        //private bool simulationStarted = false;
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
            if(InfoVisible)
                ShowOrHideInfoBalk(false);
            else
                ShowOrHideInfoBalk(true);
        }

        public void ShowOrHideInfoBalk(Boolean infoVisible)
        {
            this.InfoVisible = infoVisible;
            /// Hide info
            if (!InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                InfoVisible = false;
                
            }
            // Show info
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point((breedteScherm - breedteInfoBalk), hoogteBovenBalk);
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
				string fileName = "Traffic" + number.ToString();

				
				// Extension name (.trs => TRafficSimulation)
				saveDialog.DefaultExt = ".trs";
				saveDialog.FilterIndex = 1;
				saveDialog.Filter = "Traffic Simulation Files (*.trs) | *.trs";
				saveDialog.RestoreDirectory = true;
				saveDialog.FileName = fileName;

				// Set the ability to overwrite another file to true
				saveDialog.OverwritePrompt = true;

				// Is the button "Save" pressed?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					/// New file
					StreamWriter file = new StreamWriter(@saveDialog.FileName);

					/// File can be bigger than 1024
					file.AutoFlush = true;	

					/// Get every tile in the list
					foreach (Tile tile in windowselect.simwindow.simcontrol.tileList)
					{
						/// If the tile has some value asigned to it						
						if (tile != null)
						{

							string currenttile = tile.name;

							/* Something with saving extra info, not ready yet
							 * 
							/// Save some basic information
							// Building tile:
							switch (windowselect.simwindow.simcontrol.currentBuildTile.name)
							{
								case "Road":
									file.WriteLine(
										windowselect.simwindow.simcontrol.currentBuildTile.name + "_" +	// Name
										tile.startDirection + "_" +										// Begin direction
										tile.endDirection + "_");										// End direction
									break;

								case "Spawner":
									file.WriteLine(
										windowselect.simwindow.simcontrol.currentBuildTile.name + "_" +	// Name
										tile.direction													// Direction of tile
										);
									break;

								case "Fork":
									file.WriteLine(
										windowselect.simwindow.simcontrol.currentBuildTile.name + "_" +	// Name
										tile.notDirection												// The notDirction
										);
									break;

								case "Crossroad":
									file.WriteLine(
										windowselect.simwindow.simcontrol.currentBuildTile.name			// Name
										);
									break;

							}
							// Information (not) visible
							file.WriteLine();
							// Something
							file.WriteLine();
							// Something
							file.WriteLine();
							// Something
							file.WriteLine();

							*/

							// TODO: Extra save options

							/// Every tile has his own information
							/// For saving them, you need the specific information for each tile
							/// This is done with multiple cases, on for each tile
							/// 
							/// Basic information
							///		 0: Tile
							///		 1: Place in list
							///		 2: X position
							///		 3: Y position
							///	Specific information
							///		 4: Trafficlight strat
							///		 5: Maxspeed for a tile
							///		 6: Begin direction (notDirection for Fork, direction for Spawner)
							///		 7: End direction (only for Road)
							///		 8: laneshightolow, not for Crossroad and Fork
							///		 9: laneslowtohigh, not for Crossroad and Fork
							///		10: Cars per second in spawner

							switch (currenttile)
							{
								// Save case for a fork
								case "Fork":
									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " /*tile.strat*/ + "_" + 	// 4 strat
										tile.maxSpeed + "_" +		// 5 Maxspeed
										tile.notDirection);			// 6 De not direction
									break;

								// Save case for a crossroad
								case "Crossroad":
									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " /*tile.strat*/ + "_" + 	// 4 strat
										tile.maxSpeed);				// 5 Maxspeed
									break;

								// Save case for a road (that is a straight road or a curved road)
								case "Road":
									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
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

								// Save case for a spawner
								case "Spawner":
									file.WriteLine(
										tile.name + "_" +			// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y + "_" +		// 3 Y positie
										" " + "_" +					// 4 Empty
										tile.maxSpeed + "_" +		// 5 Maxspeed
										tile.direction + "_" +		// 6 Richting
										" " + "_" +					// 7 Empty
										tile.GetLanesOut((tile.direction + 1) % 4 + 1) + "_" +	// 8 LanesHighToLow
										tile.GetLanesIn((tile.direction + 1) % 4 + 1) /* + "_ "	+	// 9 LanesLowToHigh
										tile.carsPerSec*/);			
									break;

								// TODO: Save options for extra info (Such as..?), traffic lights strat (Maybe done..) and other things
							}
						}
					}
				}
			}
			/// Throw an exception
			catch (Exception exp)
			{
				MessageBox.Show("Error: Could not write file to disk. Original error:" + exp);

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
