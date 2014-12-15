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
        SimWindow simwindow;
        InfoBalk infobalk;
        int visiblecount = 0;

        public BovenScherm(SimWindow sim, InfoBalk info)
        {
            simwindow = sim;
            infobalk = info;

            InitializeComponent();
        }

        public void Info_Click(object sender, RoutedEventArgs e)
        {
            if (visiblecount == 1)
            {
                infobalk.Visibility = Visibility.Visible;
                visiblecount = 0;
            }
            else
            {
                infobalk.Visibility = Visibility.Hidden;
                visiblecount = 1;
            }
        }

        public void Home_Click(object sender, RoutedEventArgs e)
        {
            simwindow.Start();
        }
    }
}
