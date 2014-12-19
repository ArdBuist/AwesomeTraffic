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

        //klikmethode selecttool
        private void Select_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = true;
            s.building = false;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }

        //klikmethode gum
        private void Eraser_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = true;
            s.selected = false;
            s.building = false;
        }

        //klikmethode weg (verwijzing naar extrabuttonsonderscherm)
        private void Road_Checked(object sender, RoutedEventArgs e)
        {                      
        }

        //klikmethode bocht (verwijzing naar extrabuttonsonderscherm)
        private void Bend_Checked(object sender, RoutedEventArgs e)
        {
        }

        //klikmethode kruispunt
        private void CrossRoad_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Crossroad(s);
        }

        //klikmethode fork
        private void Fork_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int notDirection = 1;
            s.currentBuildTile = new Fork(s,notDirection); //notdirection is nu  1
        }

        //klikmethode spawner
        private void Spawner_Checked(object sender, RoutedEventArgs e)
        {            
            s.eraser = false;
            s.selected = false;
            s.building = true;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            s.currentBuildTile = new Spawner(direction);           
        }        
    }
}
