using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class Trafficlight
    {
        int color;
        Point Position;
        Crossroad road;
        SimControl sc;

        //brushes
        Brush zwart = new SolidBrush(Color.Black);
        Brush groen = new SolidBrush(Color.Green);
        Brush rood = new SolidBrush(Color.Red);
        Brush oranje = new SolidBrush(Color.Orange);

        public Trafficlight(SimControl sim, Crossroad road,Point Position)
        {
            this.road = road;
            this.Position = Position;
            sc = sim;
            DrawTrafficlight(Color.Red);
        }

        public void UpdateColor(int kleur)
        {
            //hierin kunnen nog meer acties worden gedaan als de kleur wordt veranderd, zoals andere positie van de lichten t.o.v. elkaar.
            DrawTrafficlight(kleur);
        }

        public void DrawTrafficlight(int kleur)
        {
            Graphics gr = sc.trafficlightMap.GetBitmapGraphics;
            //teken hier het zwarte vierkant van het stoplicht
            gr.FillRectangle(zwart, Position.X, Position.Y, 15, 40);
            //teken hier de kleur van het rondje
            switch (kleur)
            {
                case 0:
                    gr.FillEllipse(groen, Position.X + 2, Position.Y + 2, 10, 10);
                    break;
                case 1:
                    gr.FillEllipse(oranje, Position.X + 2, Position.Y + 12, 10, 10);
                    break;
                case 2:
                    gr.FillEllipse(rood, Position.X + 2, Position.Y + 22, 10, 10);
                    break;
            }
        }
    }
}
