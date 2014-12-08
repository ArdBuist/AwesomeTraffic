using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSimulation
{
    class Simulation
    {
        public Tile[] tiles;
        private const int ticksPerSec = 25;
        bool playing = false;

        public Simulation(Tile[] t)
        {
            this.tiles = t;
        }

        public void Start()
        {
            playing = true;

            while (playing == true)
            {
                int start = Environment.TickCount;
                UpdateVariables();
                UpdateGame();
                Sleep(Environment.TickCount - start);
            }
        }

        public void Stop()
        {
            playing = false;
        }

        private void UpdateVariables()
        {
            if (true)//wanneer er op stop wordt geklikt
            {
                Stop();
            }
        }

        private void UpdateGame()
        {
            UpdateCars();
        }

        private void Sleep(int timePassed)
        {
            System.Threading.Thread.Sleep(1000 / ticksPerSec - timePassed);
        }

        private void UpdateCars()
        {
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    t.CarUpdate();
                }
            }
        }
    }
}
