using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;

namespace TrafficSimulation
{
    public class Simulation
    {
        //begin test code
        private int testSpawn = 0;
        //einde test code

        public const int ticksPerSec = 10;
        private bool playing;
        private List<Spawner> spawnerList;
        private SimControl simControl;
        int turn;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            playing = false;
        }

        public void Run()
        {
            //maakt een lijst met alle spawners zodat het spawnalgoritme makkelijk uitgevoerd kan worden
            spawnerList = new List<Spawner>();
            foreach (Tile tile in simControl.tileList)
            {
                if (tile != null && tile.name.Equals("Spawner"))
                {
                    spawnerList.Add((Spawner)tile);
                }
            }

            while (true)
            {
                //update de game
                //int start = Environment.TickCount;
                UpdateVariables();
                UpdateGame();
                //Sleep(Environment.TickCount - start);
            }
        }

        private void UpdateVariables()
        {
            turn++;
            UpdateSpawners();

            if (turn % 50 == 0)
            {
                UpdateTrafficlights();
            }
            UpdateCars();
        }

        private void UpdateTrafficlights()
        {
            foreach (TrafficlightControl t in simControl.controlList)
            {
                t.Run();
            }
        }

        private void UpdateGame()
        {
            //simControl.vehicle.Invalidate();
            //simControl.vehicle.Update();
        }

        private void Sleep(int timePassed)
        {
            //System.Threading.Thread.Sleep(1000);//1000 / ticksPerSec - timePassed);
        }

        private void UpdateSpawners()
        {
            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick();

                if (spawn.CurrentSpawn >= 1)
                {
                    Vehicle v = CreateVehicle(spawn);
                    //spawn.AddVehicle(v, spawn.SpawnLane);
                    spawn.Spawn(v);
                }
            }
        }

        private void UpdateCars()
        {
            foreach (Tile t in simControl.tileList)
            {
                if (t != null)
                {
                    t.CarUpdate(simControl);
                }
            }
        }

        private Vehicle CreateVehicle(Spawner spawn)
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new NormalCar(new Point(spawn.position.X + 30, spawn.position.Y + 50 + 16 * spawn.SpawnLane + 3),
                new Point(spawn.position.X, spawn.position.Y), 10, spawn.maxSpeed, spawn.direction, 1);
        }
    }
}