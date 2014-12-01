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
        public Trafficlight(Crossroad road,Point Position)
        {
            this.road = road;
            this.Position = Position;
            DrawTrafficlight(Color.Red);
        }
        public void UpdateColor(Color kleur)
        {
            //hierin kunnen nog meer acties worden gedaan als de kleur wordt veranderd, zoals andere positie van de lichten t.o.v. elkaar.
            DrawTrafficlight(kleur);
        }
        public void DrawTrafficlight(Color kleur)
        {
            Graphics gr = road.Graphics;
            //de road heeft nog geen Graphics, dus stoplicht wordt nog niet getekend.
            gr.DrawEllipse(new Pen(kleur), Position.X,Position.Y, 10, 10);



        }
    }
}
