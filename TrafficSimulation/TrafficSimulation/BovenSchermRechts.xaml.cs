﻿using System;
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
        
        SimControl s;
        SimWindow simwindow;
        InfoBalk Infobalk;
        OnderScherm Onderscherm;
        //int daynightcount = 0;

        public BovenSchermRechts(SimWindow sim,  InfoBalk info, OnderScherm Onder)
        {
            
            simwindow = sim;
            s = simwindow.sim;
            Infobalk = info;
            Onderscherm = Onder;
            
            InitializeComponent();
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (InfoVisible)
            {
                simwindow.Infobalk.Visibility = Visibility.Visible;
                InfoVisible = false;
            }
            else
            {
                simwindow.Infobalk.Visibility = Visibility.Hidden;
                InfoVisible = true;
            }
            s.Invalidate();
        }


        private void Home_Click(object sender, RoutedEventArgs e)
        {
            
            simwindow.OnderHost.Visible = false;
            simwindow.BovenHostLinks.Visible = false;
            simwindow.BovenHostRechts.Visible = false;
            simwindow.InfoHost.Visible = false;
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

    }
}
