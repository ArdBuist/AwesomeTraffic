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
            for (int i = 0; i<trafficlights.Count; i++)
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

            switch(Direction)
            {
                case 1: Position.X = 100 - (NumberTrafficlight * 10);
                        Position.Y = 50;
                    break;
                case 2: Position.X = 150;
                    Position.Y = 100 - (NumberTrafficlight * 10);
                    break;
                case 3: Position.X = 100 + (NumberTrafficlight * 10);
                    Position.Y = 150;
                    break;
                case 4: Position.X = 50;
                    Position.Y = 100 + (NumberTrafficlight * 10);
                    break;
            }
            return Position;
        }
    }
}