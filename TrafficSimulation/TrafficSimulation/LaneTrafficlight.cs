﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class LaneTrafficlight
    {
        SimControl simcontrol;
        public List<Trafficlight> trafficlights;
        Tile road;
        public int direction;

        public LaneTrafficlight(SimControl sim, Tile road, int Direction, int Lanes)
        {
            trafficlights = new List<Trafficlight>();
            this.road = road;
            simcontrol = sim;
            this.direction = Direction + 1;
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
                Trafficlight Light = (Trafficlight)trafficlights[i];
                Light.UpdateColor(kleur);
                UpdateTileAccess(i, kleur);
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
                    Position.Y = 1;
                    break;
                case 1:
                    Position.X = 89;
                    Position.Y = 37 - (NumberTrafficlight * 16);
                    break;
                case 2:
                    Position.X = 53 + (NumberTrafficlight * 16);
                    Position.Y = 89;
                    break;
                case 3:
                    Position.X = 1;
                    Position.Y = 53 + (NumberTrafficlight * 16);
                    break;
            }

            return Position;
        }

        public void ChangeValues(Point position)
        {
            foreach (Trafficlight light in trafficlights)
            {
                light.DrawTrafficlight(Color.Red);
            }
        }
        private void UpdateTileAccess(int lane, Color kleur)
        {
            Tile Othertile = Methods.GetOtherTile(simcontrol,road,direction);
            if (Othertile != null)
            {
                int tileDirection = (direction + 1) % 4 + 1;
                if (kleur == Color.Green)
                    Othertile.Access[tileDirection-1, lane] = true;
               else
                    Othertile.Access[tileDirection-1, lane] = false;
            }
        }
    }
}