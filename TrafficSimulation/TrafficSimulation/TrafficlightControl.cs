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
        long lastTime = 0;
        int secondsPerUpdate = 3;
        int startTime;
        //geeft aan welke strategie wordt gebruikt
        public int strat = 1;
        //timers
        int[] timer = new int[12];
        //locks for the timers
        bool[] locks = new bool[12];
        //prioriteit strategieën
        int[] prio = new int[7];

        public TrafficlightControl(SimControl sim, Tile road, int Directions, int NotDirection, int[] NumberOfLanes)
        {
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;
            this.road = road;
            this.simcontrol = sim;

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
            trafficlightList = new List<LaneTrafficlight>();

            NumberOfDirections = Directions;
            this.road = road;
            this.simcontrol = sim;
            for (int i = 0; i < 4; i++)
            {
                if (i != NotDirection - 1)
                {
                    trafficlightList.Add(new LaneTrafficlight(sim, road, i, NumberOfLanes[i * 2]));
                }
            }

            foreach (LaneTrafficlight lane in trafficlightList)
            {
                lane.ChangeValues(position);
            }
        }

        public void Run(int extraSpeed, double extraTime)
        {
            switch (strat)
            {
                case 0: timerStrat(extraSpeed, extraTime);
                    break;
                case 1: waitingStrat();
                    break;
                case 2: randomStrat();
                    break;
            }
        }

        //stoplicht-strategieën
        public void timerStrat(int extraSpeed, double extraTime)
        {
            if (Environment.TickCount - lastTime > (secondsPerUpdate * 1000) - extraTime)
            {
                AllRed();
                if (Environment.TickCount - lastTime > 1500 + (secondsPerUpdate * 1000) - extraTime)
                {
                    lastTime = Environment.TickCount;
                    Update(turn % NumberOfDirections);
                    turn++;
                }
            }
        }

        public void waitingStrat()
        {
            /*LaneType werkt nog niet goed, ik moet nog uitzoeken hoe de stoplichttimers gaan werken,
             en de berekeningen zouden bij gebrek aan 3 banen nog kapot kunnen gaan*/

            //hier tijdelijke variabelen voor de wachttijd
            //moet nog daadwerkelijk de wachttijd van de stoplichten worden

            waitingCheck();

            if (Environment.TickCount - lastTime > secondsPerUpdate * 1000)
            {
                //alle stoplichten reset naar rood
                AllRed();

                if (Environment.TickCount - lastTime > 1500 + secondsPerUpdate * 1000)
                {
                    lastTime = Environment.TickCount;
                    int allRight = timer[0] + timer[3] + timer[6] + timer[9];
                    int forwardRight1 = timer[0] + timer[6] + timer[1] + timer[7];
                    int forwardRight2 = timer[3] + timer[9] + timer[4] + timer[10];
                    int leftRightForward1 = timer[0] + timer[1] + timer[2];
                    int leftRightForward2 = timer[3] + timer[4] + timer[5];
                    int leftRightForward3 = timer[6] + timer[7] + timer[8];
                    int leftRightForward4 = timer[9] + timer[10] + timer[11];

                    //get lowest value
                    int lowest = Math.Min(allRight, Math.Min(forwardRight1, Math.Min(forwardRight2, Math.Min(leftRightForward1, Math.Min(leftRightForward2, Math.Min(leftRightForward3, leftRightForward4))))));

                    /*Dit zijn alle strategieën voor de stoplichten, in de juiste volgorde. De wachttijden kunnen nooit clashen
                     omdat de eerste keuzes die in de if-loops voorbij komen de hoogste value hebben (links/rechtdoor/rechts).
                     Daarna komen de opties met 2 richtingen (rechtdoor/rechts), en daarna pas 4 richtingen (alleen rechts)
                     Als er minder dan drie banen per weg zijn zal er nooit een niet-bestaand stoplicht op groen worden gezet door
                     deze volgorde*/
                    if (leftRightForward1 != leftRightForward2 && leftRightForward2 != leftRightForward3
                        && leftRightForward3 != leftRightForward4)
                    {
                        if (lowest == leftRightForward1)
                            LRF1();
                        else if (lowest == leftRightForward2)
                            LRF2();
                        else if (lowest == leftRightForward3)
                            LRF3();
                        else if (lowest == leftRightForward4)
                            LRF4();
                        else if (lowest == forwardRight1)
                            FR1();
                        else if (lowest == forwardRight2)
                            FR2();
                        else if (lowest == allRight)
                            R();
                    }
                        //DIT WERKT NOG NIET
                    else
                    {
                        int prioMax = prio.Max();
                        if (prioMax == prio[0])
                            LRF1();
                        else if (prioMax == prio[1])
                            LRF2();
                        else if (prioMax == prio[2])
                            LRF3();
                        else if (prioMax == prio[3])
                            LRF4();
                        else if (prioMax == prio[4])
                            FR1();
                        else if (prioMax == prio[5])
                            FR2();
                        else if (prioMax == prio[6])
                            R();
                    }

                    //alle timers weer naar 0
                    for (int i = 0; i < 12; i++)
                    {
                        timer[i] = 0;
                        locks[i] = false;
                    }
                }
            }
        }
        public void LRF1()
        {
            for (int i = 1; i < 6; i++)
                StratUpdate(1, i);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[0] = 0;
        }
        public void LRF2()
        {
            for (int i = 1; i < 6; i++)
                StratUpdate(2, i);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[1] = 0;
        }
        public void LRF3()
        {
            for (int i = 1; i < 6; i++)
                StratUpdate(3, i);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[2] = 0;
        }
        public void LRF4()
        {
            for (int i = 1; i < 6; i++)
            {
                StratUpdate(4, i);
            }
            prio[3] = 0;
        }
        public void FR1()
        {
            StratUpdate(1, 2);
            StratUpdate(1, 3);
            StratUpdate(1, 4);
            StratUpdate(3, 2);
            StratUpdate(3, 3);
            StratUpdate(3, 4);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[4] = 0;
        }
        public void FR2()
        {
            StratUpdate(2, 2);
            StratUpdate(2, 3);
            StratUpdate(2, 4);
            StratUpdate(4, 2);
            StratUpdate(4, 3);
            StratUpdate(4, 4);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[5] = 0;
        }
        public void R()
        {
            for (int i = 1; i < 5; i++)
                StratUpdate(i, 3);

            for (int i = 0; i < 7; i++)
                prio[i]++;
            prio[6] = 0;
        }

        public void waitingCheck()
        {
            //assigns all the roads to variables except for the notDirection (in case of Fork)
            Tile[] sides = simcontrol.simulationMap.GetSurroundingTiles(road.position);
            int[] lanes = new int[4];
            for (int i = 0; i < 4; i++)
            {
                if (road.notDirection != i + 1)
                {
                    lanes[i] = sides[i].GetLanesOut(((i + 2) % 4) + 1);
                    foreach (List<Vehicle> lane in sides[i].vehicles[(i + 2) % 4])
                    {
                        foreach (Vehicle veh in lane)
                        {
                            if (veh.Speed == 0)
                            {
                                switch (lanes[i])
                                {
                                    case 1:
                                        locks[(i * 3)] = true;
                                        locks[(i * 3) + 1] = true;
                                        locks[(i * 3) + 2] = true;
                                        break;
                                    case 2:
                                        if (veh.Lane == 0)
                                            locks[(i * 3)] = true;
                                        if (veh.Lane == 1)
                                        {
                                            locks[(i * 3) + 1] = true;
                                            locks[(i * 3) + 2] = true;
                                        }
                                        break;
                                    case 3:
                                        if (veh.Lane == 0)
                                            locks[(i * 3)] = true;
                                        if (veh.Lane == 1)
                                            locks[(i * 3) + 1] = true;
                                        if (veh.Lane == 2)
                                            locks[(i * 3) + 2] = true;
                                        break;
                                }
                            }
                        }
                        for (int j = 0; j < 3; j++)
                        {
                            if (!locks[(i * 3) + j])
                                timer[(i * 3) + j]++;
                        }
                    }
                }
            }

            if (road.name == "Fork")
            {
                for (int i = 0; i < 3; i++)
                {
                    timer[(road.notDirection - 1) * 3 + i] = 99999;
                }
            }
        }

        public void randomStrat()
        {
            //yolo
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
                for (int j = 1; j < 6; j++)
                {
                    laneTrafficlight.ChangeColor(kleur, j);
                }
            }
        }

        private void AllRed()
        {
            for (int i = 0; i < NumberOfDirections; i++)
            {
                LaneTrafficlight lanetrafficlight = (LaneTrafficlight)trafficlightList[i];
                for (int j = 0; j < 5; j++)
                {
                    lanetrafficlight.ChangeColor(Color.Red, j + 1);
                }
            }
            startTime = Environment.TickCount;
        }

        private void StratUpdate(int Direction, int LaneType)
        {
            if (road.name == "Crossroad")
            {
                LaneTrafficlight l = (LaneTrafficlight)trafficlightList[Direction - 1];
                l.ChangeColor(Color.Green, LaneType);
            }
            else
            {
                if (Direction < road.notDirection)
                {
                    LaneTrafficlight l = (LaneTrafficlight)trafficlightList[Direction - 1];
                    l.ChangeColor(Color.Green, LaneType);
                }
                else
                {
                    LaneTrafficlight l = (LaneTrafficlight)trafficlightList[Direction - 2];
                    l.ChangeColor(Color.Green, LaneType);
                }
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