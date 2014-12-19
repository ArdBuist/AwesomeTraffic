﻿using System;
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
        StartWindow start;

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
	        
            BovenHostLinks = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteSchermRechts,
                Location = new Point(0, 0),
                Visible = false,
                Child = Bovenschermlinks,
            };
            this.Controls.Add(BovenHostLinks);

            BovenHostRechts = new ElementHost()
            {
                BackColor = Color.Transparent,
                Height = HoogteBovenBalk,
                Width = BreedteSchermLink,
                Location = new Point((BreedteScherm - BreedteSchermLink), 0),
                Visible = false,
                Child = Bovenschermrechts,
            };
            this.Controls.Add(BovenHostRechts);
           
            ExtraButtonsHost = new ElementHost()
            {
                Height = 200,
                Width = 200,

                Location = new Point(200, 200),
                Child = ExtrabuttonsOS,
            };
            this.Controls.Add(ExtraButtonsHost);

            OnderHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point(XLocatieOnderbalk, YLocatieOnderbalk),
                Height = HoogteOnderbalk,
                Width = BreedteOnderbalk,
                Visible = false,
                Child = Onderscherm,
            };
            this.Controls.Add(OnderHost);

            InfoHost = new ElementHost()
            {
                BackColor = Color.Transparent,
                Location = new Point((BreedteScherm - BreedteInfoBalk), HoogteBovenBalk),
                Height = HoogteInfobalk,
                Width = BreedteInfoBalk,
                Visible = false,
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
            Size Screensize = new Size(1920, 1080);

            // Openen van het startscherm
            start = new StartWindow(Screensize, windowselect);
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
            OnderHost.Visible = true;
            BovenHostLinks.Visible = true;
            BovenHostRechts.Visible = true;
            InfoHost.Visible = true;
            ExtraButtonsHost.Visible = true;
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
    }
}
