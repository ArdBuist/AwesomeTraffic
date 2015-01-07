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

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point((breedteScherm - breedteInfoBalk), hoogteBovenBalk);
                InfoVisible = false;
            }
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                InfoVisible = true;
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            windowselect.Start();
        }

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				System.Windows.Forms.SaveFileDialog saveDialog = new System.Windows.Forms.SaveFileDialog();

				int number = 1;
				string filename = "Traffic" + number.ToString();

				saveDialog.DefaultExt = ".trs";
				/*
				while (File.Exists(filename))
				{
					number++;
					filename = "Traffic" + number.ToString();
				}
				*/

				saveDialog.FileName = filename;

				// Zorgt ervoor dat je een bestand kan overschrijven
				saveDialog.OverwritePrompt = true;

				// Is er op "Opslaan" gedrukt?
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					StreamWriter file = new StreamWriter(@saveDialog.FileName);
					file.AutoFlush = true;

					// Ga alle tiles langs
					foreach (Tile tile in windowselect.simwindow.simcontrol.tileList)
					{
						// Als de tile een weg bevat
						if (tile != null)
						{
							String currenttile = tile.name;

							switch (currenttile)
							{
								case "Fork":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.notDirection + "_" +	// 1 De not direction
										tile.listPlace + "_" +		// 2 Plaats in de lijst
										tile.position.X + "_" +		// 3 X positie
										tile.position.Y);			// 4 Y positie
									// Aantal banen
									break;

								//Schrijf een regel voor een kruispunt
								case "Crossroad":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.listPlace + "_" +		// 1 Plaats in de lijst
										tile.position.X + "_" +		// 2 X positie
										tile.position.Y);			// 3 Y positie
									// Aantal banen
									break;
								// Aantal banen

								// Schrijf een regel voor bocht of rechte weg
								case "Road":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.startDirection + "_" +	// 1 Begin richting
										tile.endDirection + "_" +	// 2 Eind richting
										tile.listPlace + "_" +		// 3 Plaats in de lijst
										tile.position.X + "_" +		// 4 X positie
										tile.position.Y);			// 5 Y positie
									// Aantal banen
									break;

								// Schrijf een regel voor een spawner
								case "Spawner":
									file.WriteLine(
										tile + "_" +				// 0 Welke tile
										tile.direction + "_" +		// 1 Richting
										tile.listPlace + "_" +		// 2 Plaats in de lijst
										tile.position.X + "_" +		// 3 X positie
										tile.position.Y);			// 4 Y positie
									// Aantal banen
									break;
							}
						}
					}
				}
			}
			// Throw exception
			catch (Exception exp)
			{
				MessageBox.Show("" + exp);
			}
		}	
        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        //klikmethode voor dag- of nachtsimulatie
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
