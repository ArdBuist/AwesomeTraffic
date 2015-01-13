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
    /// Interaction logic for BovenScherm.xaml
    /// </summary>
    public partial class BovenSchermLinks : UserControl
    {
        public Boolean InfoVisible = true;
        private Boolean simulation = false;
        InfoBalk infobalk;
        OnderScherm onderscherm;
        WindowSelect windowselect;

        public BovenSchermLinks(WindowSelect ws, InfoBalk info, OnderScherm onder)
        {
            windowselect = ws;
            infobalk = info;
            onderscherm = onder;
            InitializeComponent();

            //aan het begin is het simulatiegedeelte niet te gebruiken.
            play.IsEnabled = false;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
            stop.IsEnabled = false;
            pauze.IsEnabled = false;
        }

        //Klikmethode om te kunnen wisselen tussen de simulatie en designer
        public void SimulationDesign_Click(object sender, RoutedEventArgs e)
        {
            if (simulation)
            {
                // Buttons in het het bovenschermlinks inschakelen
                play.IsEnabled = false;
                slowDown.IsEnabled = false;
                speedUp.IsEnabled = false;
                stop.IsEnabled = false;
                pauze.IsEnabled = false;

                // Buttons in het onderscherm uitschakelen
                onderscherm.selectButton.IsEnabled = true;
                onderscherm.eraserButton.IsEnabled = true;
                onderscherm.roadButton.IsEnabled = true;
                onderscherm.bendButton.IsEnabled = true;
                onderscherm.crossRoadButton.IsEnabled = true;
                onderscherm.forkButton.IsEnabled = true;
                onderscherm.spawnerButton.IsEnabled = true;

                //stoppen van laten rijden van de auto's
                if(windowselect.simwindow.simcontrol.simulation.simStarted)
                    windowselect.simwindow.simcontrol.simulation.StartSim();

                //verwijderen van alle auto's
                windowselect.simwindow.simcontrol.ClearRoad();
                windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);
               
                windowselect.simwindow.simcontrol.Invalidate();
                windowselect.simwindow.InfoBalk.UpdateSimulationReset();
                simulationDesign.Content = "Simulation";
                simulation = false;
            }
            else
            {
                // Buttons in het het bovenschermlinks inschakelen
                play.IsEnabled = true;
                pauze.IsEnabled = false;
                stop.IsEnabled = false;
                slowDown.IsEnabled = false;
                speedUp.IsEnabled = false;
                
                // Buttons in het onderscherm uitschakelen
                onderscherm.selectButton.IsEnabled = false;
                onderscherm.eraserButton.IsEnabled = false;
                onderscherm.roadButton.IsEnabled = false;
                onderscherm.bendButton.IsEnabled = false;
                onderscherm.crossRoadButton.IsEnabled = false;
                onderscherm.forkButton.IsEnabled = false;
                onderscherm.spawnerButton.IsEnabled = false;
                windowselect.simwindow.extraButtonsHost.Location = new System.Drawing.Point(windowselect.simwindow.ClientSize);

                //zorgen dat er niet meer getekend kan worden, er kan alleen verschoven worden
                windowselect.simwindow.simcontrol.state = "selected";

                simulationDesign.Content = "Design";
                simulation = true;
            }
            windowselect.Invalidate();
        }

        //Klikmethode voor het starten van de simulatie
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.simulation.StartSimknop();
            play.IsEnabled = false;
            pauze.IsEnabled = true;
            stop.IsEnabled = true;
            slowDown.IsEnabled = true;
            speedUp.IsEnabled = true;
        }

        //klikmethode voor het pauzeren van de simulatie
        private void Pauze_Click(object sender, RoutedEventArgs e)
        {
            windowselect.simwindow.simcontrol.simulation.PauseSimknop() ;
            pauze.IsEnabled = false;
            play.IsEnabled = true;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
        }

        //Klikmethode voor het stoppen van de simulatie
        private void Stop_Clik(object sender, RoutedEventArgs e)
        {
            
            windowselect.simwindow.simcontrol.simulation.StartSim();
            windowselect.simwindow.simcontrol.ClearRoad();
            play.IsEnabled = true;
            pauze.IsEnabled = false;
            stop.IsEnabled = false;
            slowDown.IsEnabled = false;
            speedUp.IsEnabled = false;
            windowselect.simwindow.InfoBalk.UpdateSimulationReset();
            
        }

        //klikmethode voor het vertragen van de simulatie
        private void SlowDown_Click(object sender, RoutedEventArgs e)
        {
            if (windowselect.simwindow.simcontrol.simulation.extraSpeed != 0)
                windowselect.simwindow.simcontrol.simulation.extraSpeed--;
            else
                windowselect.simwindow.simcontrol.simulation.PauseSeconds += 10;
            
        }

        //klikmethode voor het versnellen van de simulatie
        private void SpeedUp_Click(object sender, RoutedEventArgs e)
        {
            if (windowselect.simwindow.simcontrol.simulation.PauseSeconds>50)
                windowselect.simwindow.simcontrol.simulation.PauseSeconds -= 10;
            else
                windowselect.simwindow.simcontrol.simulation.extraSpeed++;
        }

    }
}
