using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Resources;

namespace TrafficSimulation
{
    class Simulation
    {
        //begin test code
        private int testSpawn = 0;
        //einde test code

        public const int ticksPerSec = 25;
        private bool playing;
        private List<Spawner> spawnerList;
        private SimControl simControl;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            playing = false;
        }

        public void Start()
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

            playing = true;

            while (playing)
            {
                int start = Environment.TickCount;
                UpdateVariables();
                UpdateGame();
                simControl.vehicle.Invalidate();
                simControl.vehicle.Update();
                //Sleep(Environment.TickCount - start);
            }
        }

        public void Stop()
        {
            playing = false;
        }

        private void UpdateVariables()
        {
            if (false)//wanneer er op stop wordt geklikt
            {
                Stop();
            }

            UpdateCars();
        }

        private void UpdateGame()
        {
            foreach (Vehicle v in simControl.vehicleList)
            {
                simControl.vehicleMap.AddObject(v.Bitmap, v.position.X, v.position.Y);
                v.Update();
            }
        }

        private void Sleep(int timePassed)
        {
            System.Threading.Thread.Sleep(1000);//1000 / ticksPerSec - timePassed);
        }

        private void UpdateCars()
        {

            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick();

                if (spawn.CurrentSpawn >= 1)
                {
                    Vehicle v = createVehicle(spawn);
                    //spawn.AddVehicle(v, spawn.SpawnLane);
                    simControl.vehicleList.Add(v);
                    spawn.Spawn();
                }
            }
        }

        private Vehicle createVehicle(Spawner spawn)
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new NormalCar(new Point(spawn.position.X+30, spawn.position.Y+50-16*1+3//aantal rijbanen
                ), new Point(spawn.position.X, spawn.position.Y), 10, spawn.maxSpeed, spawn.direction, 1);
        }
    }
}