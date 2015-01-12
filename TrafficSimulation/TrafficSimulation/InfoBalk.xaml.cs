using System;
using System.Collections.Generic;
using System.Data;
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

        private void Lanes_SelectionChanged(object sender, EventArgs e)
        {
            if (lanes.SelectedItem != null)
            {
                string kant = (string)lanes.Tag;
                string[] kantEnPlaats = kant.Split('_');
                string ob = ((ComboBoxItem)lanes.SelectedItem).ToString();
                string[] array = ob.Split(' ');
                if (windowselect.simwindow != null)
                {
                    if (windowselect.simwindow.simcontrol.selectedTile != null && windowselect.simwindow.simcontrol.selectedTile.name != "Fork" && windowselect.simwindow.simcontrol.selectedTile.name!="Crossroad")
                    {
                        if(int.Parse(kantEnPlaats[1])==0)
                            windowselect.simwindow.simcontrol.selectedTile.UpdateLanes(windowselect.simwindow.simcontrol, int.Parse(kantEnPlaats[0]), int.Parse(array[1]), windowselect.simwindow.simcontrol.selectedTile.GetLanesOut(int.Parse(kantEnPlaats[0])));
                        else
                            windowselect.simwindow.simcontrol.selectedTile.UpdateLanes(windowselect.simwindow.simcontrol, int.Parse(kantEnPlaats[0]), windowselect.simwindow.simcontrol.selectedTile.GetLanesOut(int.Parse(kantEnPlaats[0])), int.Parse(array[1]));
                        //windowselect.simwindow.simcontrol.selectedTile.UpdateLanes(windowselect.simwindow.simcontrol, int.Parse((string)lanes.Tag), int.Parse(array[1]), int.Parse(array[1]));
                        windowselect.simwindow.simcontrol.selectedTile.UpdateOtherTiles(windowselect.simwindow.simcontrol, 0);
                    }
                    windowselect.simwindow.simcontrol.backgroundPB.Invalidate();
                }
            }
        }
        public void UpdateDesign(int[,] tileLanes, int maxSpeed)
        {
            lanes.SelectedValue = 2;
            speed.SelectedIndex = maxSpeed-1;
            //lane1 = tileLanes[0, 0];
            //lane2 = tileLanes[0, 1];
            //lane3 = tileLanes[1, 0];
            //lane4 = tileLanes[1, 1];
            //lane5 = tileLanes[2, 0];
            //lane6 = tileLanes[2, 1];
            //lane7 = tileLanes[3, 0];
            //lane8 = tileLanes[3, 1];

        }
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
}
