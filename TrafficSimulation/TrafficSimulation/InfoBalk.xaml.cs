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
        //SimControl s;
        //SimWindow simwindow;
        WindowSelect windowselect;

        public InfoBalk(WindowSelect ws)
        {
            windowselect = ws;
            //simwindow = sim;
            //s = simwindow.sim; ;
            
            InitializeComponent();

        }

        private void GreenWave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GreenWaveDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HideCombobox()
        {
            lane1.IsEnabled = true;
            lane2.IsEnabled = true;
            lane3.IsEnabled = true;
            lane4.IsEnabled = true;
            lane5.IsEnabled = true;
            lane6.IsEnabled = true;
            lane7.IsEnabled = true;
            lane8.IsEnabled = true;
            // alle comboboxen laten verdwijnen
        }
        public void ShowComboBox(List<int> Directions)
        {
            if(Directions.Contains(1))
            {
                //laat comboboxen bovenaan zien 

            }
            else if (Directions.Contains(2))
            {

            }
            else if (Directions.Contains(3))
            {

            }
            else if (Directions.Contains(4))
            {

            }
        }

        private void lane1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                        
            
               // = ((ComboBoxItem)myCombobox.SelectedItem).Content.ToString();
        }
      }
}
