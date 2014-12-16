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
        public Boolean InfoVisible = true;
        private bool simulationStarted = false;
        SimControl s;
        SimWindow simwindow;
        InfoBalk Infobalk;
        //int daynightcount = 0;

        public BovenScherm(SimWindow sim, SimControl s, InfoBalk info)
        {
            this.s = s;
            simwindow = sim;
            Infobalk = info;

            InitializeComponent();
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (InfoVisible)
            {
                Infobalk.Visibility = Visibility.Visible;
                InfoVisible = false;
            }
            else
            {
                Infobalk.Visibility = Visibility.Hidden;
                InfoVisible = true;
            }
            s.Invalidate();
        }

        private void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            if (s.Simulatie == true)
            {
                PlayPauze.IsEnabled = true;
                SlowDown.IsEnabled = true;
                SpeedUp.IsEnabled = true;
                SimulationDesign.Content = "Simulation";
                s.Simulatie = false;

            }
            else if (s.Simulatie == false)
            {
                PlayPauze.IsEnabled = false;
                SlowDown.IsEnabled = false;
                SpeedUp.IsEnabled = false;
                SimulationDesign.Content = "Design";
                s.Simulatie = true;
            }
            s.Invalidate();
        }

       private void PlayPauze_Click(object sender, RoutedEventArgs e)
        {
            if (!simulationStarted)
            {
                s.sim.Start();
                simulationStarted = true;
            }
            else
            {
                s.sim.Stop();
                simulationStarted = false;
            }
        }
        private void Home_Click(object sender, RoutedEventArgs e)
        {
            simwindow.Start();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {

        }

        //klikmethode voor dag- of nachtsimulatie
        private void DayNight_Click(object sender, RoutedEventArgs e)
        {
            if (s.Day == true)
            {
                DayNight.Content = "Day";
                s.Day = false;
            }
            else
            {
                DayNight.Content = "Night";
                s.Day = true;
            }
        }
        //klikmethode voor het vertragen van de simulatie
        private void SlowDown_Click(object sender, RoutedEventArgs e)
        {
            //hier moet de variabele van de sleep veranderd worden, zodat de simulatie langzamer gaat
        }

        private void SpeedUp_Click(object sender, RoutedEventArgs e)
        {
            //Hier moeten de variabele van de sleep veranderd worden, zodat de simulatie sneller gaat
        }
    }
}
