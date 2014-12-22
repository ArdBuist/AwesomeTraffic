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
        public int hoogtebovenbalk, hoogteonderbalk, breedteinfobalk, hoogteinfobalk, breedtescherm, hoogtescherm, ylocatieonderbalk, breedteschermlink, breedteschermrechts, xlocatieonderbalk, breedteonderbalk, breedtesonderbalk;

        public SimWindow(Size size, WindowSelect windowselect)
        {
            this.Size = size;
            this.windowselect = windowselect;
            sim = new SimControl(this.ClientSize, this);
            sim.Location = new Point(0, 0);
          
            //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            breedtescherm = Screen.PrimaryScreen.Bounds.Width;
            hoogtescherm = Screen.PrimaryScreen.Bounds.Height;
            hoogtebovenbalk = 100;
            hoogteonderbalk = 100;
            hoogteinfobalk = (hoogtescherm - (hoogtebovenbalk + hoogteonderbalk));
            ylocatieonderbalk = (hoogtescherm - hoogteonderbalk);
            xlocatieonderbalk = (breedtescherm / 4);
            breedteinfobalk = breedtescherm/6;
            breedteschermlink = (breedtescherm / 4);
            breedteschermrechts = (breedtescherm / 4);
            breedteonderbalk = ((breedtescherm / 4) * 2);

            Infobalk = new InfoBalk(windowselect);
            ExtrabuttonsOS = new ExtraButtonsOS(windowselect, Infobalk);
            Onderscherm = new OnderScherm(windowselect, Infobalk, ExtrabuttonsOS, ExtraButtonsHost, breedteonderbalk, ylocatieonderbalk, xlocatieonderbalk);
            Bovenschermlinks = new BovenSchermLinks(windowselect, Infobalk, Onderscherm);
            Bovenschermrechts = new BovenSchermRechts(windowselect, Infobalk, Onderscherm);

            BovenHostLinks = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = hoogtebovenbalk,
                Width = breedteschermrechts,
                Location = new Point(0, 0),
                Child = Bovenschermlinks,
            };
            this.Controls.Add(BovenHostLinks);

            BovenHostRechts = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = hoogtebovenbalk,
                Width = breedteschermlink,
                Location = new Point((breedtescherm - breedteschermlink), 0),
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
                Location = new Point(xlocatieonderbalk, ylocatieonderbalk),
                Height = hoogteonderbalk,
                Width = breedteonderbalk,
                Child = Onderscherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                //Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Location = new Point(this.Size),
                Height = hoogteinfobalk,
                Width = breedteinfobalk,
                Child = Infobalk,
            };
            this.Controls.Add(InfoHost);
            this.Controls.Add(sim);
            
        }
        public SimControl simcontrol { get { return sim; } }
    }
}
