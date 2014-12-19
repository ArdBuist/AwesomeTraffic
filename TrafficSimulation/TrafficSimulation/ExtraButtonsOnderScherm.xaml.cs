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
    public partial class ExtraButtonsOnderScherm : UserControl
    {
        public ExtraButtonsOnderScherm()
        {
            InitializeComponent();
        }

        //klikmethode rechte weg 1 naar 3 (verticaal)
        private void Roadver_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(1, 3);
        }

        //klikmethode rechte weg 2 naar 4 (horizontaal)
        private void Roadhor_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(2, 4);
        }

        //klikmethode bocht 1 naar 2
        private void Curve12_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(1, 2);
        }

        //klikmethode bocht 2 naar 3
        private void Curve23_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(2, 3);
        }

        //klikmethode bocht 3 naar 4
        private void Curve34_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(3, 4);
        }

        //klikmethode bocht 1 naar 4
        private void Curve14_Checked(object sender, RoutedEventArgs e)
        {
            s.eraser = false;
            s.selected = false;
            s.building = true;
            s.currentBuildTile = new Road(1, 4);
        }
    }
}
