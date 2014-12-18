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
        public SimControl sim; 
        public ElementHost BovenHostLinks, BovenHostRechts, OnderHost, InfoHost, ExtraButtonsHost;
       	public BovenSchermLinks Bovenschermlinks;
        public BovenSchermRechts Bovenschermrechts;
        public InfoBalk Infobalk;
	    public ExtraButtonsOS ExtrabuttonsOS;
        public OnderScherm Onderscherm;

        StartControl start;
        

        
        public SimWindow()
        {
            sim = new SimControl(this.ClientSize, this);
            // Scherm maximaliseren
            this.WindowState = FormWindowState.Maximized;

            // Alle schermranden weghalen
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //test
	
		// Maak de infobalk, onderscherm en bovenscherm

            Infobalk = new InfoBalk(sim);
            ExtrabuttonsOS = new ExtraButtonsOS(sim, Infobalk);
            Onderscherm = new OnderScherm(sim, Infobalk, ExtrabuttonsOS);
            Bovenschermlinks = new BovenSchermLinks(this, Infobalk, Onderscherm);
            Bovenschermrechts = new BovenSchermRechts(this, Infobalk, Onderscherm);


            int HoogteBovenBalk, HoogteOnderbalk, BreedteInfoBalk, HoogteInfobalk, BreedteScherm, HoogteScherm, YLocatieOnderbalk, BreedteSchermLink, BreedteSchermRechts;
            //Variable om de elementhosten afhankelijk te maken van het scherm en andere elementhosten
            
		    BreedteScherm = Screen.PrimaryScreen.Bounds.Width;
            HoogteScherm = Screen.PrimaryScreen.Bounds.Height;
            BreedteSchermLink = (BreedteScherm / 4);
            BreedteSchermRechts = (BreedteScherm / 4);
            HoogteBovenBalk = 100;
            HoogteOnderbalk = 100;
            YLocatieOnderbalk = (HoogteScherm - HoogteOnderbalk);
            HoogteInfobalk = (HoogteScherm - (HoogteBovenBalk + HoogteOnderbalk));
            BreedteInfoBalk = 300;

            BovenHostLinks = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteSchermRechts,
                Location = new Point(0,0),
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

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(0, YLocatieOnderbalk),
                Height = HoogteOnderbalk,
                Width = BreedteScherm,
                Child = Onderscherm,
            };
            this.Controls.Add(OnderHost);

	ExtraButtonsHost = new ElementHost()
            {
                Height = 200,
                Width = 200,
                Visible = false,
                Location = new Point(200,200),
                Child = ExtrabuttonsOS,
            };
            this.Controls.Add(ExtraButtonsHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Height = HoogteInfobalk,
                Width = BreedteInfoBalk,
                Child = Infobalk,
            };
            this.Controls.Add(InfoHost);

            Invalidate();

            // Laad het beginscherm
            Start();
        }

        public void Start()
        {
            this.Controls.Remove(sim);
            // Deze screensizing verkloot mijn scherm
            Size Screensize = new Size(1920,1080);
            
            // Openen van het startscherm
            start = new StartControl(Screensize, this);
            this.BackColor = Color.Green;
            start.Size = Screensize;
            start.Location = new Point(0, 0);
            this.Controls.Add(start);
            

            //start.Location = new Point(((WidthStartScreen-300)/2), ((HeightStartScreen -300)/2));
            //start.Left = ((WidthStartScreen - 300) / 2);
            //start.Top = ((HeightStartScreen - 300) / 2);
            
        }

        public void New()
        {
            // Verwijder start menu
            this.Controls.Remove(start);

            // Open simcontrol

            sim = new SimControl(Screen.PrimaryScreen.Bounds.Size, this);

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
