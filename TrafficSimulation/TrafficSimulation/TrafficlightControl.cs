using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class TrafficlightControl
    {
        List<LaneTrafficlight> trafficlightList;
        int NumberOfDirections;

        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes)
        {
            trafficlightList = new List<LaneTrafficlight>();
            
            NumberOfDirections = Directions;
            
            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i]));
                }
            }
        }
        
        private void Run()
        {
            int i = 0;
            //Zorgt dat elke 5 seconden Update wordt aangeroepen met als variabele 1-4/1-3 in volgorde
            while (true)
            {
                i++;
                int turn = i % NumberOfDirections;
                Update(turn);
                Thread.Sleep(5000);
            }
        }

        private void Update(int turn)
        {
            //Zet nu telkens een andere kant op groen.
            for(int i = 0; i < NumberOfDirections;i++)
            {
                Color kleur;   
                LaneTrafficlight laneTrafficlight =(LaneTrafficlight) trafficlightList[i];
                if (i == turn)
                    kleur = Color.Green;
                else
                    kleur = Color.Red;
                laneTrafficlight.ChangeColor(kleur);
            }
            //Geeft nog helemaal niks door aan omliggende tiles. Dat moet hier nog.
        }

        public void ChangeValues(Point position, int ListPlace)
        {
            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position, ListPlace);
            }
        }
    }
}