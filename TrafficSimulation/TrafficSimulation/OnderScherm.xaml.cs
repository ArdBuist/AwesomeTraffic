﻿using System;
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
        InfoBalk Infobalk;
        ExtraButtonsOS ExtraButtonsOS;

        public OnderScherm(SimControl s, InfoBalk info, ExtraButtonsOS Extra)
        {
            this.s = s;
            Infobalk = info;
            ExtraButtonsOS = Extra;
            InitializeComponent();
        }

        private void Select_Checked(object sender, RoutedEventArgs e)
        {
            //s.selected = true;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }

        private void Eraser_Checked(object sender, RoutedEventArgs e)
        {
            //s.eraser = true;
        }

        private void Road_Checked(object sender, RoutedEventArgs e)
        {
            ExtraButtonsOS.Visibility = Visibility.Visible;
            ExtraButtonsOS.Road_1_3.Visibility = Visibility.Visible;
            ExtraButtonsOS.Road_2_4.Visibility = Visibility.Visible;
          
            
            /*
            s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
             */
        }

        private void Bend_Checked(object sender, RoutedEventArgs e)
        {
            ExtraButtonsOS.Visibility = Visibility.Visible;
            ExtraButtonsOS.Bend_1_2.Visibility = Visibility.Visible;
            ExtraButtonsOS.Bend_2_3.Visibility = Visibility.Visible;
            ExtraButtonsOS.Bend_3_4.Visibility = Visibility.Visible;
            ExtraButtonsOS.Bend_4_1.Visibility = Visibility.Visible;
            
            /*
            s.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            s.currentBuildTile = new Road(start, end);
            */
        }

        private void CrossRoad_Checked(object sender, RoutedEventArgs e)
        {
            //s.eraser = false;
            //s.currentBuildTile = new Crossroad();
        }

        private void Fork_Checked(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int notdirection = 1; //=1 vervangen door variabele, variabele waar geen weg naartoe loopt
            s.currentBuildTile = new Fork(notdirection);
             */
        }

        private void Spawner_Checked(object sender, RoutedEventArgs e)
        {
            /*
            s.eraser = false;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            s.currentBuildTile = new Spawner(direction);
            */
        }
        

    }
}
