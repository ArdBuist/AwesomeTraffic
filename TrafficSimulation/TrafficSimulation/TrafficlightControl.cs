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

        public TrafficlightControl(Crossroad road, int Directions, int NotDirection, List<int> NumberOfLanes)
        {
            this.NumberOfDirections = Directions;
            List<LaneTrafficlight> trafficlightList = new List<LaneTrafficlight>();
            for(int i = 1; i<Directions+1; i++)
            {
                if (i != NotDirection)
                {
                    trafficlightList.Add(new LaneTrafficlight(road, i, NumberOfLanes[i]));
                }
            }
        }
        
        private void Run()
        {
            int i = 0;
            //Zorgt dat elke 5 seconden Update wordt aangeroepen met als variabele 1-4/1-3 in volgorde
            while (1!=0)
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
    }
}