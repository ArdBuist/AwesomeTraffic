using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace TrafficSimulation
{
       public partial class SimWindow : Form
    {
        SimControl s;
        //MenuStrip menu;
        ElementHost host; 

        public SimWindow()
        {
            //scherm maximaliseren
            //this.WindowState = FormWindowState.Maximized;
            //alle schermranden weghalen
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            host = new ElementHost();
            //host.Dock = DockStyle.Fill;
            host.Height = 300;
            host.Width = 300;
            Interface scherm = new Interface();
            host.Child = scherm;
            this.Controls.Add(host);

            this.BackColor = Color.Green;
            s = new SimControl(this.ClientSize);
            s.Location = new Point(0, 0);
            this.Controls.Add(s);

        }
        private void New()
        {

        }
        private void Save()
        {

        }
        private void Open()
        {

        }
        private void Close()
        {
            this.Close();
        }

        private void SimWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
