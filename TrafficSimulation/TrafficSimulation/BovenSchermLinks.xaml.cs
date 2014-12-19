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
    public partial class BovenSchermLinks : UserControl
    {
        public Boolean InfoVisible = true;
        private bool simulationStarted = false;
        SimControl s;
        WindowSelect windowselect;
        InfoBalk Infobalk;
        OnderScherm Onderscherm;
        //int daynightcount = 0;


        public BovenScherm(WindowSelect sim, SimControl s, InfoBalk info)
        {
            this.s = s;
            windowselect = sim;
            Infobalk = info;
            Onderscherm = Onder;
            InitializeComponent();
        }

        private void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            if (s.Simulatie)
            {
                PlayPauze.IsEnabled = true;
                SlowDown.IsEnabled = true;
                SpeedUp.IsEnabled = true;
                SimulationDesign.Content = "Simulation";
                simulationStarted = false;
            }
            else
            {
                PlayPauze.IsEnabled = false;
                SlowDown.IsEnabled = false;
                SpeedUp.IsEnabled = false;
                SimulationDesign.Content = "Design";
                simulationStarted = true;
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
            windowselect.Start();
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
