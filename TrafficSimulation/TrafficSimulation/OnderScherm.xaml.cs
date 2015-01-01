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
            int hoogteExtraButtonOSRoad = 100 ;
            int xLocationRoadMenu = (60 + xLocatieOnderBalk);
            int yLocationRoadMenu = yLocatieOnderBalk - hoogteExtraButtonOSRoad;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSRoad;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationRoadMenu, yLocationRoadMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.roadhor.Visibility = Visibility.Visible;
            extraButtonsOS.roadver.Visibility = Visibility.Visible;

            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = false;
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSBend = 200;
            int xLocationBendMenu = (60 + (100) + xLocatieOnderBalk);
            int yLocationBendMenu = yLocatieOnderBalk - hoogteExtraButtonOSBend;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSBend;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationBendMenu, yLocationBendMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.bend12.Visibility = Visibility.Visible;
            extraButtonsOS.bend23.Visibility = Visibility.Visible;
            extraButtonsOS.bend34.Visibility = Visibility.Visible;
            extraButtonsOS.bend14.Visibility = Visibility.Visible;

            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = false;
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
            int xLocationForkMenu = (60 + (100 * 3) + xLocatieOnderBalk);
            int yLocationForkMenu = yLocatieOnderBalk - hoogteExtraButtonOSFork;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSFork;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationForkMenu, yLocationForkMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.fork12.Visibility = Visibility.Visible;
            extraButtonsOS.fork23.Visibility = Visibility.Visible;
            extraButtonsOS.fork34.Visibility = Visibility.Visible;
            extraButtonsOS.fork14.Visibility = Visibility.Visible;

            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = false;
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }
        
        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {
            int hoogteExtraButtonOSSpawner = 200;
            int xLocationSpawnerMenu = (60 + (100 * 4) + xLocatieOnderBalk);
            int yLocationSpawnerMenu = yLocatieOnderBalk - hoogteExtraButtonOSSpawner;
            windowselect.simwindow.extraButtonsHost.Height = hoogteExtraButtonOSSpawner;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xLocationSpawnerMenu, yLocationSpawnerMenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerdown.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerleft.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerup.Visibility = Visibility.Visible;
            extraButtonsOS.spawnerright.Visibility = Visibility.Visible;

            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = false;
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }              
    }
}
