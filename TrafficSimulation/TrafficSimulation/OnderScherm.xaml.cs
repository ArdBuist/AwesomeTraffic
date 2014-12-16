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
            //bool selected = s.selected;
            InitializeComponent();
        }

        private void RechtWeg_Click(object sender, RoutedEventArgs e)
        {
            /*
             * s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
             */
        }

        private void Bocht_Click(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
            */
        }

        private void Kruispunt_Click(object sender, RoutedEventArgs e)
        {
            //s.eraser = false;
            //s.currentBuildTile = new Crossroad();
        }

        private void Tsplitsing_Click(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int notdirection = 1; //=1 vervangen door variabele, variabele waar geen weg naartoe loopt
            s.currentBuildTile = new Fork(notdirection);
             */
        }

        private void Gum_Click(object sender, RoutedEventArgs e)
        {
            //s.eraser = true;
        }

        //klikmethode voor selecteren tile
        private void Select_Click(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            //s.selected = true;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }


        public void spawnerClick(Object obj, MouseEventArgs mea)
        {
            /*
            s.eraser = false;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            s.currentBuildTile = new Spawner(direction);
            */
        }

    }
}
