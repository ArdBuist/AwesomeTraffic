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
        int breedteOnderBalk, yLocatieOnderBalk, xLocatieOnderBalk;

        public OnderScherm(WindowSelect ws, InfoBalk info, ExtraButtonsOS extra, ElementHost extraHost, int bob, int ylob, int xlob)
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
            int xlocationroadmenu = (((breedteOnderBalk / 7) * 2) + xLocatieOnderBalk), ylocationroadmenu = 500;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xlocationroadmenu, ylocationroadmenu);

            extraButtonsOS.Visibility = Visibility.Visible;
            extraButtonsOS.Road_1_3.Visibility = Visibility.Visible;
            extraButtonsOS.Road_2_4.Visibility = Visibility.Visible;

        }
        private void RoadButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            extraButtonsOS.Road_1_3.Visibility = Visibility.Hidden;
            extraButtonsOS.Road_2_4.Visibility = Visibility.Hidden;
        }

        
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            int xlocationbendmenu = (((breedteOnderBalk / 7) * 3) + xLocatieOnderBalk), ylocationbendmenu = 500;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xlocationbendmenu, ylocationbendmenu);
            extraButtonsOS.Bend_1_2.Visibility = Visibility.Visible;
            extraButtonsOS.Bend_2_3.Visibility = Visibility.Visible;
            extraButtonsOS.Bend_3_4.Visibility = Visibility.Visible;
            extraButtonsOS.Bend_4_1.Visibility = Visibility.Visible;
       }

        private void BendButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            extraButtonsOS.Bend_1_2.Visibility = Visibility.Hidden;
            extraButtonsOS.Bend_2_3.Visibility = Visibility.Hidden;
            extraButtonsOS.Bend_3_4.Visibility = Visibility.Hidden;
            extraButtonsOS.Bend_4_1.Visibility = Visibility.Hidden;
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
            int xlocationforkmenu = (((breedteOnderBalk / 7) * 4) + xLocatieOnderBalk), ylocationforkmenu = 500;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xlocationforkmenu, ylocationforkmenu);
            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
            extraButtonsOS.Height = 100;
            extraButtonsOS.Fork_1_2.Visibility = Visibility.Visible;
            extraButtonsOS.Fork_2_3.Visibility = Visibility.Visible;
            extraButtonsOS.Fork_3_4.Visibility = Visibility.Visible;
            extraButtonsOS.Fork_4_1.Visibility = Visibility.Visible;
           
        }
        private void ForkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            extraButtonsOS.Fork_1_2.Visibility = Visibility.Hidden;
            extraButtonsOS.Fork_2_3.Visibility = Visibility.Hidden;
            extraButtonsOS.Fork_3_4.Visibility = Visibility.Hidden;
            extraButtonsOS.Fork_4_1.Visibility = Visibility.Hidden;
        }

        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {
            int xlocationspawnermenu = (((breedteOnderBalk / 7) * 5) + xLocatieOnderBalk), ylocationspawnermenu = 600;
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(xlocationspawnermenu, ylocationspawnermenu);
            extraButtonsOS.Spawner_1.Visibility = Visibility.Visible;
            extraButtonsOS.Spawner_2.Visibility = Visibility.Visible;
            extraButtonsOS.Spawner_3.Visibility = Visibility.Visible;
            extraButtonsOS.Spawner_4.Visibility = Visibility.Visible;
        }    

        private void SpawnerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            extraButtonsOS.Spawner_1.Visibility = Visibility.Hidden;
            extraButtonsOS.Spawner_2.Visibility = Visibility.Hidden;
            extraButtonsOS.Spawner_3.Visibility = Visibility.Hidden;
            extraButtonsOS.Spawner_4.Visibility = Visibility.Hidden;
                      
        }

             
    }
}
