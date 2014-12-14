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
    /// Interaction logic for InfoBalk.xaml
    /// </summary>
    public partial class InfoBalk : UserControl
    {
        SimControl s;
        public InfoBalk(SimControl s)
        {
            this.s = s;
            s.InfoVisible = false;
            if (s.InfoVisible == true)
            {
                this.Visibility = Visibility.Visible;
            }
            else
            {
                this.Visibility = Visibility.Hidden;
            }

            s.Invalidate();
            InitializeComponent();

        }


    }
}
