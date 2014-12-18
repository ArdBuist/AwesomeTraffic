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


namespace TrafficSimulation
{
    /// <summary>
    /// Interaction logic for OnderScherm.xaml
    /// </summary>
    public partial class OnderScherm : UserControl
    {
        SimControl s;

        public OnderScherm(SimControl s)
        {
            this.s = s; 
            InitializeComponent();
        }

        private void Select_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = true;
            s.building = false;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }

        private void Eraser_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = true;
            s.selected = false;
            s.building = false;
        }

        //klikmethode rechte weg 1 naar 3 (verticaal)
        private void Road_Checked(object sender, RoutedEventArgs e)
        {            
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 1;
            int end = 3;
            s.currentBuildTile = new Road(1, 3);           
        }

        //klikmethode rechte weg 2 naar 4 (horizontaal)
        private void Roadhor_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 2;
            int end = 4;
            s.currentBuildTile = new Road(start, end);
        }

        //klikmethode bocht 1 naar 2
        private void Curve12_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 1;
            int end = 2;
            s.currentBuildTile = new Road(start, end);
        }

        //klikmethode bocht 2 naar 3
        private void Curve23_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 2;
            int end = 3;
            s.currentBuildTile = new Road(start, end);
        }

        //klikmethode bocht 3 naar 4
        private void Curve34_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 3;
            int end = 4;
            s.currentBuildTile = new Road(start, end);
        }

        //klikmethode bocht 1 naar 4
        private void Curve14_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int start = 1;
            int end = 4;
            s.currentBuildTile = new Road(start, end);
        }

        private void Bend_Checked(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            s.selected = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
            */
        }

        private void CrossRoad_Checked(object sender, RoutedEventArgs e)
        {
            //s.eraser = false;
            s.selected = false;
            //s.currentBuildTile = new Crossroad();
        }

        private void Fork_Checked(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int notdirection = 1; //=1 vervangen door variabele, variabele waar geen weg naartoe loopt
            s.currentBuildTile = new Fork(notdirection);
            s.selected = false;
             */
        }


        private void Spawner_Checked(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            s.currentBuildTile = new Spawner(direction);
            s.selected = false;
            */
        }
        

    }
}
