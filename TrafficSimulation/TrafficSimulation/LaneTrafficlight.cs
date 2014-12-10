using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class LaneTrafficlight
    {
        List<Trafficlight> trafficlights;
        Tile road;

        public LaneTrafficlight(SimControl sim, Tile road, int Direction, int Lanes)
        {
            trafficlights = new List<Trafficlight>();
            this.road = road;
            

            for (int i = 0; i < Lanes; i++)
            {
                Point Position = GetPosition(Direction, i);
                trafficlights.Add(new Trafficlight(sim, road, Position));
            }
        }

        public void ChangeColor(Color kleur)
        {
            for (int i = 0; i < trafficlights.Count; i++)
            {
                Trafficlight Light =  (Trafficlight)trafficlights[i];
                Light.UpdateColor(kleur);
            }
        }

        //uitrekenen van de relatieve positie van het stoplicht t.o.v. de linkerbovenhoek van de tile.
        //Dit moet nog een stuk beter en mooier.
        private Point GetPosition(int Direction, int NumberTrafficlight)
        {
            Point Position = new Point(0, 0);

            switch (Direction)
            {
                case 0:
                    Position.X = 37 - (NumberTrafficlight * 16);
                    Position.Y = 0;
                    break;
                case 1:
                    Position.X = 90;
                    Position.Y = 37 - (NumberTrafficlight * 16);
                    break;
                case 2:
                    Position.X = 53 + (NumberTrafficlight * 16);
                    Position.Y = 90;
                    break;
                case 3:
                    Position.X = 0;
                    Position.Y = 53 + (NumberTrafficlight * 16);
                    break;
            }
            return Position;
        }

        public void ChangeValues(Point position, int ListPlace)
        {
            foreach (Trafficlight light in trafficlights)
            {
                light.DrawTrafficlight(Color.Red);
            }
        }
    }
}