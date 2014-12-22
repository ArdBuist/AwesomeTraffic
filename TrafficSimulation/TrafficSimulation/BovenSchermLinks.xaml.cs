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
        InfoBalk Infobalk;
        OnderScherm onderscherm;
        WindowSelect windowselect;
       //int daynightcount = 0;


        public BovenSchermLinks(WindowSelect ws, InfoBalk info, OnderScherm onder)
        {
            //this.s = s;
            windowselect = ws;
            Infobalk = info;
            onderscherm = onder;
            InitializeComponent();
        }

        private void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            if (simulationStarted)
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
            windowselect.Invalidate();
        }

        private void PlayPauze_Click(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.simulation.StartSim();
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
