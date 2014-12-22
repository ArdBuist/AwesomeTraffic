using System;
using System.Collections.Generic;
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
    /// Interaction logic for ExtraButtonsOnderScherm.xaml
    /// </summary>
    public partial class ExtraButtonsOS : UserControl
    {
        SimControl s;
        //SimWindow simwindow;
        InfoBalk Infobalk;
        WindowSelect windowselect;
        
        

        public ExtraButtonsOS(WindowSelect ws, InfoBalk info)
        {
            windowselect = ws;
            
            //simwindow = sim;
            //s = simwindow.sim; ;
            Infobalk = info;

            
            InitializeComponent();
        }

      
        private void GridExtraButtonsOS_LostFocus(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);

            this.roadhor.Visibility = Visibility.Hidden;
            this.roadver.Visibility = Visibility.Hidden;
            this.bend_1_2.Visibility = Visibility.Hidden;
            this.bend_2_3.Visibility = Visibility.Hidden;
            this.bend_3_4.Visibility = Visibility.Hidden;
            this.bend_4_1.Visibility = Visibility.Hidden;
            this.fork_1_2.Visibility = Visibility.Hidden;
            this.fork_2_3.Visibility = Visibility.Hidden;
            this.fork_3_4.Visibility = Visibility.Hidden;
            this.fork_4_1.Visibility = Visibility.Hidden;
            this.spawner_1.Visibility = Visibility.Hidden;
            this.spawner_2.Visibility = Visibility.Hidden;
            this.spawner_3.Visibility = Visibility.Hidden;
            this.spawner_4.Visibility = Visibility.Hidden;

            windowselect.simwindow.extraButtonsHost.BackColor = System.Drawing.Color.Transparent;
        }

        //klikmethode rechte weg 1 naar 3 (verticaal)
        private void Roadver_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 3);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
            this.roadhor.Visibility = Visibility.Hidden;
            this.roadver.Visibility = Visibility.Hidden;
            this.Visibility = Visibility.Hidden;
        }

        //klikmethode rechte weg 2 naar 4 (horizontaal)
        private void Roadhor_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(2, 4);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

        //klikmethode bocht 1 naar 2
        private void Curve12_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 2);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

        //klikmethode bocht 2 naar 3
        private void Curve23_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(2, 3);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

        //klikmethode bocht 3 naar 4
        private void Curve34_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(3, 4);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

        //klikmethode bocht 1 naar 4
        private void Curve14_Checked(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.eraser = false;
            windowselect.simwindow.simcontrol.selected = false;
            windowselect.simwindow.simcontrol.building = true;
            windowselect.simwindow.simcontrol.currentBuildTile = new Road(1, 4);
            windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
        }

        
    }
}
