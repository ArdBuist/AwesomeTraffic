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
        InfoBalk Infobalk;
        ExtraButtonsOS ExtrabuttonsOS;
        ElementHost ExtraButtonsHost;
       // public Boolean bend = false, road = false, spawner = false, fork = false;

        public OnderScherm( WindowSelect ws, InfoBalk info, ExtraButtonsOS Extra, ElementHost ExtraHost)
        {
            windowselect = ws;
            
            //simwindow = sim;
            //s = simwindow.sim;
            ExtraButtonsHost = ExtraHost;
            Infobalk = info;
            ExtrabuttonsOS = Extra;
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
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(400, 500);//deze moet nog aangepast worden zodat het direct boven de button komt
            ExtrabuttonsOS.Visibility = Visibility.Visible;         
            ExtrabuttonsOS.Road_1_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Road_2_4.Visibility = Visibility.Visible;

            
        }
        private void RoadButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            ExtrabuttonsOS.Road_1_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Road_2_4.Visibility = Visibility.Hidden;
        }
        private void BendButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(400, 500);//deze moet nog aangepast worden zodat het direct boven de button komt
            ExtrabuttonsOS.Bend_1_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_2_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_3_4.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Bend_4_1.Visibility = Visibility.Visible;

            
        }
        private void BendButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            ExtrabuttonsOS.Bend_1_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_2_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_3_4.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Bend_4_1.Visibility = Visibility.Hidden;
            windowselect.simwindow.ExtraButtonsHost.BackColor = System.Drawing.Color.Transparent;
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
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(400, 500);//deze moet nog aangepast worden zodat het direct boven de button komt
            windowselect.simwindow.ExtraButtonsHost.BackColor = System.Drawing.Color.Transparent;
            ExtrabuttonsOS.Height = 100;
            ExtrabuttonsOS.Fork_1_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_2_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_3_4.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Fork_4_1.Visibility = Visibility.Visible;
           
        }
        private void ForkButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            ExtrabuttonsOS.Fork_1_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_2_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_3_4.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Fork_4_1.Visibility = Visibility.Hidden;
        }

        private void SpawnerButton_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(400, 500);//deze moet nog aangepast worden zodat het direct boven de button komt
            ExtrabuttonsOS.Spawner_1.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_2.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_3.Visibility = Visibility.Visible;
            ExtrabuttonsOS.Spawner_4.Visibility = Visibility.Visible;
        }    

        private void SpawnerButton_MouseLeave(object sender, MouseEventArgs e)
        {
            windowselect.simwindow.ExtraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
            ExtrabuttonsOS.Spawner_1.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_2.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_3.Visibility = Visibility.Hidden;
            ExtrabuttonsOS.Spawner_4.Visibility = Visibility.Hidden;
                      
        }        
    }
}
