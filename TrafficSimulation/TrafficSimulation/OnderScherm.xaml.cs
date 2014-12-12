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
    /// Interaction logic for OnderScherm.xaml
    /// </summary>
    public partial class OnderScherm : UserControl
    {
        SimControl sc;

        public OnderScherm(SimControl sc)
        {
            InitializeComponent();
        }

        public void selectTile(Object obj, MouseEventArgs mea)
        {
            sc.selected = true;
            //hierna moet dan een scherm verschijnen met info en aanpassingsmogelijkheden
        }

        public void removeTile(Object obj, MouseEventArgs mea)
        {
            sc.eraser = true;
        }

        public void spawnerClick(Object obj, MouseEventArgs mea)
        {
            sc.eraser = false;
            int direction = 4; //=4 vervangen door variabele, de kant waar de weg heen loopt
            sc.currentBuildTile = new Spawner(direction);
        }

        public void roadClick(Object obj, MouseEventArgs mea)
        {
            sc.eraser = false;
            int start = 1; //=1 vervangen door variabele
            int end = 4;   //=4 vervangen door variabele
            sc.currentBuildTile = new Road(start, end);
        }

        public void forkClick(Object obj, MouseEventArgs mea)
        {
            sc.eraser = false;
            int notdirection = 1; //=1 vervangen door variabele, variabele waar geen weg naartoe loopt
            sc.currentBuildTile = new Fork(notdirection);
        }

        public void crossRoadClick (Object obj, MouseEventArgs mea)
        {
            sc.eraser = false;
            sc.currentBuildTile = new Crossroad();
        }
    }
}
