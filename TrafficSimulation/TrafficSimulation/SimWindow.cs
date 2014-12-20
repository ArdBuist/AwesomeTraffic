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
        public ElementHost BovenHostLinks, BovenHostRechts, OnderHost, InfoHost, ExtraButtonsHost;
        public BovenSchermLinks Bovenschermlinks;
        public BovenSchermRechts Bovenschermrechts;
        public InfoBalk Infobalk;
        public ExtraButtonsOS ExtrabuttonsOS;
        public OnderScherm Onderscherm;
        private int HoogteBovenBalk, HoogteOnderbalk, BreedteInfoBalk, HoogteInfobalk, BreedteScherm, HoogteScherm, YLocatieOnderbalk, BreedteSchermLink, BreedteSchermRechts, XLocatieOnderbalk, BreedteOnderbalk;

        public SimWindow(Size size, WindowSelect windowselect)
        {
            this.Size = size;
            this.windowselect = windowselect;
            sim = new SimControl(this.ClientSize, this);
            sim.Location = new Point(0, 0);

            Infobalk = new InfoBalk(windowselect);
            ExtrabuttonsOS = new ExtraButtonsOS(windowselect, Infobalk);
            Onderscherm = new OnderScherm(windowselect, Infobalk, ExtrabuttonsOS, ExtraButtonsHost);
            Bovenschermlinks = new BovenSchermLinks(windowselect, Infobalk, Onderscherm);
            Bovenschermrechts = new BovenSchermRechts(windowselect, Infobalk, Onderscherm);

            
           
            //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            BreedteScherm = Screen.PrimaryScreen.Bounds.Width;
            HoogteScherm = Screen.PrimaryScreen.Bounds.Height;
            HoogteBovenBalk = 100;
            HoogteOnderbalk = 100;
            YLocatieOnderbalk = (HoogteScherm - HoogteOnderbalk);
            HoogteInfobalk = (HoogteScherm - (HoogteBovenBalk + HoogteOnderbalk));
            BreedteInfoBalk = 300;
            BreedteSchermLink = (BreedteScherm / 4);
            BreedteSchermRechts = (BreedteScherm / 4);
            XLocatieOnderbalk = (BreedteScherm / 4);
            BreedteOnderbalk = ((BreedteScherm / 4) * 2);
	        
            BovenHostLinks = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteSchermRechts,
                Location = new Point(0, 0),
                Child = Bovenschermlinks,
            };
            this.Controls.Add(BovenHostLinks);

            BovenHostRechts = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteSchermLink,
                Location = new Point((BreedteScherm - BreedteSchermLink), 0),
                Child = Bovenschermrechts,
            };
            this.Controls.Add(BovenHostRechts);
           
            ExtraButtonsHost = new ElementHost()
            {
                Height = 200,
                Width = 200,
                Location = new Point(this.Size),
                Child = ExtrabuttonsOS,
            };
            this.Controls.Add(ExtraButtonsHost);

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(XLocatieOnderbalk, YLocatieOnderbalk),
                Height = HoogteOnderbalk,
                Width = BreedteOnderbalk,
                Child = Onderscherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                //Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Location = new Point(this.Size),
                Height = HoogteInfobalk,
                Width = BreedteInfoBalk,
                Child = Infobalk,
            };
            this.Controls.Add(InfoHost);
            this.Controls.Add(sim);
            
        }
        public SimControl simcontrol { get { return sim; } }
    }
}
