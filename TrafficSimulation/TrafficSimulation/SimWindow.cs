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
    public partial class SimWindow : UserControl
    {
        SimControl sim;
        WindowSelect windowselect;
         public ElementHost BovenHost, OnderHost, InfoHost;
        public BovenScherm BovenScherm;
        public InfoBalk InfoBalk;
        // test of mijn branch het doet
        public SimWindow(Size size, WindowSelect windowselect)
        {
            this.Size = size;
            this.windowselect = windowselect;
            sim = new SimControl(this.ClientSize, this);
            sim.Location = new Point(0, 0);
             InfoBalk InfoBalk = new InfoBalk(sim);
            OnderScherm OnderScherm = new OnderScherm(sim);
            BovenScherm BovenScherm = new BovenScherm(windowselect, sim, InfoBalk);
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
            this.Controls.Add(sim);
            // Laad het beginscherm
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
    }
}
