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
        Boolean simulation = false;

        InfoBalk Infobalk;
        OnderScherm onderscherm;
        WindowSelect windowselect;
        //int daynightcount = 0;


        public BovenSchermLinks(WindowSelect ws, InfoBalk info, OnderScherm onder)
        {

            windowselect = ws;
            Infobalk = info;
            onderscherm = onder;

            InitializeComponent();
        }

        private void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            
            if (simulation)
            {
                play.IsEnabled = true;
                slowDown.IsEnabled = true;
                speedUp.IsEnabled = true;
                stop.IsEnabled = true;
                pauze.IsEnabled = true;
                onderscherm.selectButton.IsEnabled = true;
                onderscherm.eraserButton.IsEnabled = true;
                onderscherm.roadButton.IsEnabled = true;
                onderscherm.bendButton.IsEnabled = true;
                onderscherm.crossRoadButton.IsEnabled = true;
                onderscherm.forkButton.IsEnabled = true;
                onderscherm.spawnerButton.IsEnabled = true;
                

                simulationDesign.Content = "Simulation";
                simulation = false;
            }
            else
            {
                play.IsEnabled = false;
                slowDown.IsEnabled = false;
                speedUp.IsEnabled = false;
                stop.IsEnabled = false;
                pauze.IsEnabled = false;

                onderscherm.selectButton.IsEnabled = false;
                onderscherm.eraserButton.IsEnabled = false;
                onderscherm.roadButton.IsEnabled = false;
                onderscherm.bendButton.IsEnabled = false;
                onderscherm.crossRoadButton.IsEnabled = false;
                onderscherm.forkButton.IsEnabled = false;
                onderscherm.spawnerButton.IsEnabled = false;

                simulationDesign.Content = "Design";
                simulation = true;
            }
            windowselect.Invalidate();
        }

        private void PlayPauze_Click(object sender, RoutedEventArgs e)
        {

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
