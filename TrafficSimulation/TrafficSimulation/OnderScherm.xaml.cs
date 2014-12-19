using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Forms.Integration;


namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for OnderScherm.xaml
    /// </summary>
    public partial class OnderScherm : UserControl
    {
        SimControl s;
        SimWindow simwindow;
        InfoBalk Infobalk;
        ExtraButtonsOS ExtrabuttonsOS;
        ElementHost ExtraButtonsHost;
       // public Boolean bend = false, road = false, spawner = false, fork = false;

        public OnderScherm(SimWindow sim, InfoBalk info, ExtraButtonsOS Extra, ElementHost ExtraHost)
        {
            simwindow = sim;
            s = simwindow.sim;
            ExtraButtonsHost = ExtraHost;
            Infobalk = info;
            ExtrabuttonsOS = Extra;
            InitializeComponent();
        }

        private void SelectButton_Checked(object sender, RoutedEventArgs e)
        {
           
            //s.selected = true;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            
            //s.eraser = true;
        }

        private void RoadButton_Checked(object sender, RoutedEventArgs e)
        {
            
            ExtrabuttonsOS.Visibility = Visibility.Visible;         
            ExtrabuttonsOS.Road_1_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Road_2_4.Visibility = Visibility.Visible;

            /*
            s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
             */
        }
        private void RoadButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Road_1_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Road_2_4.Visibility = Visibility.Hidden;
        }
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Visible;
            ExtraButtonsHost.Location = new System.Drawing.Point(300, 300);
            ExtrabuttonsOS.Bend_1_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_2_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_3_4.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_4_1.Visibility = Visibility.Visible;

            /*
            s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
            */
        }
        private void BendButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_1_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_2_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_3_4.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_4_1.Visibility = Visibility.Hidden;
        }
        private void CrossRoadButton_Checked(object sender, RoutedEventArgs e)
        {

           
            ExtrabuttonsOS.Visibility = Visibility.Hidden;
            //s.eraser = false;
            //s.currentBuildTile = new Crossroad();
        }

        private void ForkButton_Checked(object sender, RoutedEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Height = 100;
            
            ExtrabuttonsOS.Fork_1_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_2_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_3_4.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_4_1.Visibility = Visibility.Visible;
            /*
             s.eraser = false;
             int notdirection = 1; //=1 vervangen door variabele, variabele waar geen weg naartoe loopt
             s.currentBuildTile = new Fork(notdirection);
              */
        }
        private void ForkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_1_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_2_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_3_4.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_4_1.Visibility = Visibility.Hidden;
        }

        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {

            ExtrabuttonsOS.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_1.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_4.Visibility = Visibility.Visible;
            /*
            s.eraser = false;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            s.currentBuildTile = new Spawner(direction);
            */
        }

        private void SpawnerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ExtrabuttonsOS.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_1.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_4.Visibility = Visibility.Hidden;
        }

    }
}
