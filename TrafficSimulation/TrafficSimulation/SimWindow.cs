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
        SimControl sim; 
         public ElementHost BovenHost, OnderHost, InfoHost;
        public BovenScherm BovenScherm;
        public InfoBalk InfoBalk;
        StartControl start;
        // test of mijn branch het doet
        public SimWindow()
        {
            sim = new SimControl(this.ClientSize, this);
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //test

             InfoBalk InfoBalk = new InfoBalk(sim);
            OnderScherm OnderScherm = new OnderScherm(sim);
            BovenScherm BovenScherm = new BovenScherm(this, sim, InfoBalk);
            int HoogteBovenBalk, HoogteOnderbalk, BreedteInfoBalk, HoogteInfobalk, BreedteScherm, HoogteScherm, YLocatieOnderbalk;
                        //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            BreedteScherm = Screen.PrimaryScreen.Bounds.Width;
            HoogteScherm = Screen.PrimaryScreen.Bounds.Height;
            HoogteBovenBalk = 100;
            HoogteOnderbalk = 100;
            YLocatieOnderbalk = (HoogteScherm - HoogteOnderbalk);
            HoogteInfobalk = (HoogteScherm - (HoogteBovenBalk + HoogteOnderbalk));
            BreedteInfoBalk = 300;

            BovenHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteScherm,
                Child = BovenScherm,
            };
            this.Controls.Add(BovenHost);

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(0, YLocatieOnderbalk),
                Height = HoogteOnderbalk,
                Width = BreedteScherm,
                Child = OnderScherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Height = HoogteInfobalk,
                Width = BreedteInfoBalk,
                Child = InfoBalk,
            };
            this.Controls.Add(InfoHost);

            Invalidate();
        



            // Laad het beginscherm
            Start();
        }

        public void Start()
        {
            this.Controls.Remove(sim);
            int WidthStartScreen = Screen.PrimaryScreen.Bounds.Width, HeightStartScreen = Screen.PrimaryScreen.Bounds.Height;
            Size screensize = new Size(WidthStartScreen, HeightStartScreen);

            // Openen van het startscherm
            start = new StartControl(screensize, this);
            this.BackColor = Color.Green;

            start.Left = ((WidthStartScreen - 300) / 2);
            start.Top = ((HeightStartScreen - 300) / 2);
            this.Controls.Add(start);
        }

        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(start);

            // Open simcontrol
            
            this.BackColor = Color.Green;
            sim.Location = new Point(0, 0);
            this.Controls.Add(sim);
        }

        public void Options()
        {

        }

        public void Save()
        {

        }

        public void Open()
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
