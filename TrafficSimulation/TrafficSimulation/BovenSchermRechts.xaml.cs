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
    public partial class BovenSchermRechts : UserControl
    {
        public Boolean InfoVisible = true;
        private bool simulationStarted = false;
        InfoBalk infoBalk;
        OnderScherm onderScherm;
        WindowSelect windowselect;
        //int daynightcount = 0;

        public BovenSchermRechts(WindowSelect ws,  InfoBalk info, OnderScherm Onder)
        {
            windowselect = ws;
            infoBalk = info;
            onderScherm = Onder;
            
            InitializeComponent();
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (InfoVisible)
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(0, 0);//moeten nog de goede coordinaten komen
                InfoVisible = false;
            }
            else
            {
                windowselect.simwindow.infoHost.Location = new System.Drawing.Point(windowselect.simwindow.Size);
                InfoVisible = true;
            }
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            windowselect.Start();
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
            //if (windowselect.simwindow.Day == true)
            //{
            //    DayNight.Content = "Day";
            //    s.Day = false;
            //}
            //else
            //{
            //    DayNight.Content = "Night";
            //    s.Day = true;
            //}
        }

    }
}
