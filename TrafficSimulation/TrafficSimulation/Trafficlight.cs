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
        Color color;
        Point Position;
        Tile road;
        SimControl sc;

        //brushes
        Brush zwart = new SolidBrush(Color.Black);
        Brush groen = new SolidBrush(Color.Green);
        Brush rood = new SolidBrush(Color.Red);
        Brush oranje = new SolidBrush(Color.Orange);

        public Trafficlight(SimControl sim, Tile road, Point Position)
        {
            this.road = road;
            this.Position = Position;
            sc = sim;
            //DrawTrafficlight(Color.Red);
        }

        public void UpdateColor(Color kleur)
        {
            //hierin kunnen nog meer acties worden gedaan als de kleur wordt veranderd, zoals andere positie van de lichten t.o.v. elkaar.
            DrawTrafficlight(kleur);
        }

        public void DrawTrafficlight(Color kleur)
        {
            Graphics gr = sc.trafficlightMap.GetBitmapGraphics;
            //teken hier het zwarte vierkant van het stoplicht
            gr.FillRectangle(zwart, Position.X, Position.Y, 15, 40);
            //teken hier de kleur van het rondje
            if (kleur == Color.Green)
            {
                gr.FillEllipse(zwart, Position.X + 5, Position.Y + 25, 10, 10);
            }
            else if (kleur == Color.Red)
            {
                gr.FillEllipse(zwart, Position.X + 5, Position.Y + 5, 10, 10);
            }
            else if (kleur == Color.Orange)
            {
                gr.FillEllipse(zwart, Position.X + 5, Position.Y + 15, 10, 10);
            }
        }
    }
}