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
        WindowSelect windowselect;
        InfoBalk infoBalk;
        ExtraButtonsOS extraButtonsOS;
        ElementHost extraButtonsHost;
        int breedteOnderBalk, yLocatieOnderBalk, xLocatieOnderBalk, hoogteOnderBalk;

        public OnderScherm(WindowSelect ws, InfoBalk info, ExtraButtonsOS extra, ElementHost extraHost, int bob, int ylob, int xlob, int hob)
        {
            windowselect = ws;
            
            //simwindow = sim;
            //s = simwindow.sim;
            extraButtonsHost = extraHost;
            infoBalk = info;
            extraButtonsOS = extra;
            breedteOnderBalk = bob;
            yLocatieOnderBalk = ylob;
            xLocatieOnderBalk = xlob;
            hoogteOnderBalk = hob;
            InitializeComponent();
        }

        private void SelectButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = true;
            windowselect.simwindow.simcontrol.building = false;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
            
        }

        private void EraserButton_Checked(object sender, RoutedEventArgs e)
        {
            
            windowselect.simwindow.simcontrol.eraser = true;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = false;
        }

        private void RoadButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSRoad = 200;
            int xLocationRoadMenu = (((breedteOnderBalk / 7) * 2) + xLocatieOnderBalk);
            int yLocationRoadMenu = yLocatieOnderBalk - hoogteExtraButtonOSRoad;

            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationRoadMenu, yLocationRoadMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            //extraButtonsHost.Height = hoogteExtraButtonOSRoad;
            extraButtonsOS.roadhor.Visibility = Visibility.Visible;
            extraButtonsOS.roadver.Visibility = Visibility.Visible;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSBend = 200;
            int xLocationBendMenu = (((breedteOnderBalk / 7) * 3) + xLocatieOnderBalk);
            int yLocationBendMenu = yLocatieOnderBalk - hoogteExtraButtonOSBend;

            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationBendMenu, yLocationBendMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            //extraButtonsHost.Height = hoogteExtraButtonOSBend;

            extraButtonsOS.bend_1_2.Visibility = Visibility.Visible;
            extraButtonsOS.bend_2_3.Visibility = Visibility.Visible;
            extraButtonsOS.bend_3_4.Visibility = Visibility.Visible;
            extraButtonsOS.bend_4_1.Visibility = Visibility.Visible;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
       }

       private void CrossRoadButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;

            windowselect.simwindow.simcontrol.currentBuildTile = new Crossroad(windowselect.simwindow.simcontrol);
            
        }

        private void ForkButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSFork = 200;
            int xLocationForkMenu = (((breedteOnderBalk / 7) * 4) + xLocatieOnderBalk);
            int yLocationForkMenu = yLocatieOnderBalk - hoogteExtraButtonOSFork;

            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationForkMenu, yLocationForkMenu);
            
            extraButtonsOS.Visibility = Visibility.Visible;
            //extraButtonsHost.Height = hoogteExtraButtonOSFork;

            extraButtonsOS.fork_1_2.Visibility = Visibility.Visible;
            extraButtonsOS.fork_2_3.Visibility = Visibility.Visible;
            extraButtonsOS.fork_3_4.Visibility = Visibility.Visible;
            extraButtonsOS.fork_4_1.Visibility = Visibility.Visible;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        
        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSSpawner = 200;
            int xLocationSpawnerMenu = (((breedteOnderBalk / 7) * 5) + xLocatieOnderBalk);
            int yLocationSpawnerMenu = yLocatieOnderBalk - hoogteExtraButtonOSSpawner;

            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationSpawnerMenu, yLocationSpawnerMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            //extraButtonsHost.Height = hoogteExtraButtonOSSpawner;

            extraButtonsOS.spawner_1.Visibility = Visibility.Visible;
            extraButtonsOS.spawner_2.Visibility = Visibility.Visible;
            extraButtonsOS.spawner_3.Visibility = Visibility.Visible;
            extraButtonsOS.spawner_4.Visibility = Visibility.Visible;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }              
    }
}
