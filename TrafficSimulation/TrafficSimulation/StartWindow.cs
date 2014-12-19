using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
    public partial class StartWindow : UserControl
    {
        ElementHost StartHost;
        WindowSelect windowselect;
        InterfaceStart StartScherm;

        public StartWindow(Size size, WindowSelect sim)
        {
            this.Size = size;
            windowselect = sim;

            StartScherm = new InterfaceStart(this);

            StartHost = new ElementHost()
            {
                Height = 350,
                Width = 300,
                Child = StartScherm
            };
            this.Controls.Add(StartHost);

        }

        public void New_Click()
        {
            // Open simcontrol
            windowselect.New();
        }

        public void Option_Click()
        {

        }

        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}
