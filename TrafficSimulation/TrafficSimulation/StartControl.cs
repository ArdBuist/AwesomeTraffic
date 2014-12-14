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
    public partial class StartControl : UserControl
    {
        ElementHost StartHost;
        SimWindow simwindow;
        InterfaceStart StartScherm;

        public StartControl(Size size, SimWindow sim)
        {
            this.Size = size;

            StartScherm = new InterfaceStart(this);  

            StartHost = new ElementHost() { Child = StartScherm };
            StartHost.Size = new Size(300, 300);
            this.Controls.Add(StartHost);

            simwindow = sim;
        }

        public void New_Click()
        {
            // Open simcontrol
            simwindow.New();          
        }

        public void Exit_Click()
        {
            // Sluit applicatie
            Application.Exit();
        }
    }
}
