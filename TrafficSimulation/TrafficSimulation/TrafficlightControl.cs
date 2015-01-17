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
        SimControl simcontrol;
        int NumberOfDirections;
        Tile road;
        int turn = 0;
        public int strat = 1;
        long lastTime = 0;
        int secondsPerUpdate = 2;

        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes)
        {
            this.road = road;
            this.simcontrol = sim;
            RemoveOldTrafficlights();
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;
           

            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection - 1)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i * 2]));
                }
            }
        }

        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes, Point position)
        {
            this.road = road;
            this.simcontrol = sim;
            RemoveOldTrafficlights();
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;
           
            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection-1)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i * 2]));
                }
            }

            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position);
            }
        }
        private void RemoveOldTrafficlights()
        {
            for (int i = road.position.X; i < road.position.X + 101; i++)
                for (int j = road.position.Y; j < road.position.Y + 101; j++)
                    simcontrol.trafficlightBC.bitmap.SetPixel(i, j, Color.Transparent);
            simcontrol.trafficlightPB.Invalidate();
        }
        public void Run(int extraSpeed,double extraTime)
        {
            if (Environment.TickCount - lastTime> (secondsPerUpdate*1000)-extraTime)
            {
                lastTime = Environment.TickCount;
                Update(turn % NumberOfDirections);
                turn++;
            }
        }

        private void Update(int turn)
        {
            //Zet nu telkens een andere kant op groen.
            for (int i = 0; i < NumberOfDirections; i++)
            {
                Color kleur;
                LaneTrafficlight laneTrafficlight = (LaneTrafficlight)trafficlightList[i];
                if (i == turn)
                {
                    kleur = Color.Green;
                }
                else
                {
                    kleur = Color.Red;
                }
                laneTrafficlight.ChangeColor(kleur);
            }
        }

        public void ChangeValues(Point position)
        {
            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position);
            }
        }
    }
}