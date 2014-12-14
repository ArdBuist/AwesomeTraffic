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
    /// Interaction logic for BovenScherm.xaml
    /// </summary>
    public partial class BovenScherm : UserControl
    {
        SimControl s;
        
        public BovenScherm(SimControl s)
        {
            this.s = s;



            InitializeComponent();
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (s.InfoVisible == true)
            {
                // s.InfoBalk.IsVisible = true;
                s.InfoVisible = false;
                s.Invalidate();
            }
            else if (s.InfoVisible == false)
            {
                s.InfoVisible = true;
                s.Invalidate();
            }

        }

        private void SimulatieOntwerp_Click(object sender, RoutedEventArgs e)
        {

            if (s.Simulatie == true)
            {
                PlayPauze.IsEnabled = true;
                Vertragen.IsEnabled = true;
                Versnellen.IsEnabled = true;
                SimulatieOntwerp.Content = "Simulatie";
                s.Simulatie = false;

            }
            else if (s.Simulatie == false)
            {
                PlayPauze.IsEnabled = false;
                Vertragen.IsEnabled = false;
                Versnellen.IsEnabled = false;
                SimulatieOntwerp.Content = "Ontwerp";
                s.Simulatie = true;
            }
            s.Invalidate();
        }

        private void Versnellen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPauze_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Vertragen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DayNight_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
