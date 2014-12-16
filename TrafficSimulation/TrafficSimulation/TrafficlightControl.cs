using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    public class TrafficlightControl
    {
        List<LaneTrafficlight> trafficlightList;
        int NumberOfDirections;
        int turn = 0;

        //constructor
        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes)
        {
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;

            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i * 2]));
                }
            }
        }

        //constructor
        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes, Point position)
        {
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;

            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i * 2]));
                }
            }

            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position);
            }
        }

        /*
         * 
         */
        public void Run()
        {
            Update(turn % 2);
            turn++;
        }

        /*
         * 
         */
        private void Update(int turn)
        {
            //Zet nu telkens een andere kant op groen.
            for (int i = 0; i < NumberOfDirections; i++)
            {
                Color kleur;
                LaneTrafficlight laneTrafficlight = (LaneTrafficlight)trafficlightList[i];
                if (i == turn || i - 2 == turn)
                {
                    kleur = Color.Green;
                }
                else
                {
                    kleur = Color.Red;
                }
                laneTrafficlight.ChangeColor(kleur);
            }
            //Geeft nog helemaal niks door aan omliggende tiles. Dat moet hier nog.
        }

        /*
         * 
         */
        public void ChangeValues(Point position)
        {
            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position);
            }
        }
    }
}