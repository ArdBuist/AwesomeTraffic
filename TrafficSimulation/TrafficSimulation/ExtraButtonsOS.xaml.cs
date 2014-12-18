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
    /// Interaction logic for ExtraButtonsOnderScherm.xaml
    /// </summary>
    public partial class ExtraButtonsOS : UserControl
    {
        SimControl s;
        SimWindow simwindow;
        InfoBalk Infobalk;
        public Boolean bend, road, spawner, fork;

        public ExtraButtonsOS(SimWindow sim, InfoBalk info)
        {
            simwindow = sim;
            s = simwindow.sim; ;
            Infobalk = info;

            this.Visibility = Visibility.Hidden;

            
            InitializeComponent();
        }
    }
}
