using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;

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

        public Color Kleur
        {
            get { return color; }
        }

        public void UpdateColor(Color kleur)
        {
            //hierin kunnen nog meer acties worden gedaan als de kleur wordt veranderd, zoals andere positie van de lichten t.o.v. elkaar.
            DrawTrafficlight(kleur);
        }

        public void DrawTrafficlight(Color kleur)
        {
            Graphics gr = sc.trafficlightBC.GetBitmapGraphics;
            gr.SmoothingMode = SmoothingMode.AntiAlias;

            //daadwerkelijke positie stoplichten
            Point TruePos = new Point(Position.X + road.position.X, Position.Y + road.position.Y);

            //teken hier het zwarte vierkant van het stoplicht
            gr.FillRectangle(zwart, TruePos.X, TruePos.Y, 10, 10);
            //teken hier de kleur van het rondje
            if (kleur == Color.Green)
            {
                gr.FillEllipse(groen, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
            else if (kleur == Color.Red)
            {
                gr.FillEllipse(rood, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
            else if (kleur == Color.Orange)
            {
                gr.FillEllipse(oranje, TruePos.X + 1, TruePos.Y + 1, 8, 8);
            }
        }
    }
}