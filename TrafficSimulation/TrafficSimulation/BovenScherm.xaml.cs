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
        SimControl sc;

        public BovenScherm(SimControl sc)
        {
            InitializeComponent();
        }

        //klikmethode voor het vertragen van de simulatie
        public void slowDown(Object obj, MouseEventArgs mea)
        {
           //hier moet de variabele van de sleep veranderd worden, zodat de simulatie langzamer gaat
        }

        //klikmethode voor dag- of nachtsimulatie
        public void TimeofDay(Object obj, MouseEventArgs mea)
        {
            //1 is dag, 2 is nacht
            if (sc.TimeofDay == 1)
            {
                sc.TimeofDay = 2;
            }

            else
            {
                sc.TimeofDay = 1;
            }
        }
    }
}
