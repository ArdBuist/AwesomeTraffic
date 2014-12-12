using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TrafficSimulation
{
    class Simulation
    {
        public const int ticksPerSec = 25;
        private bool playing;
        private List<Spawner> spawnerList;
        private SimControl simControl;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            playing = false;
            spawnerList = new List<Spawner>();

            foreach (Tile tile in simControl.tileList)
            {
                if (tile != null && tile.name.Equals("Spawner"))
                {
                    spawnerList.Add((Spawner)tile);
                }
            }
        }

        public void Start()
        {
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
        }

        private void UpdateGame()
        {
            UpdateCars();
        }

        private void Sleep(int timePassed)
        {
            System.Threading.Thread.Sleep(2000);//1000 / ticksPerSec - timePassed);
        }

        private void UpdateCars()
        {
            foreach (Vehicle v in simControl.vehicleList)
            {
                v.Update();
                simControl.vehicleMap.AddObject(v.Bitmap, v.position.X, v.position.Y);
            }
            /* eventueel permenente code, anders code hierboven gebruiken (doen nu nog hetzelfde)
            foreach (Tile t in simControl.tileList)
            {
                if (t != null)
                {
                    t.CarUpdate();
                    foreach(List<Vehicle> list in t.vehicles){
                        foreach (Vehicle v in list)
                        {
                            simControl.vehicleMap.AddObject(v.Bitmap, 150,150);
                        }
                    }
                }
            }
            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick(1 / ticksPerSec);
                if (spawn.CurrentSpawn > 1)
                {
                    Vehicle v = createVehicle(spawn);
                    spawn.AddVehicle(v, spawn.SpawnLane);
                    simControl.vehicleList.Add(v);
                }
            }*/
        }

        private Vehicle createVehicle(Spawner spawn)
        {
            //deze methode moet ingevuld worden, hier wordt een auto gegenereerd
            return new NormalCar(new Point(spawn.position.X + 70, spawn.position.Y + 50 - 8), new Point(spawn.position.X + 200, spawn.position.Y + 200), 10, 5, spawn.direction, 1);
        }
    }
} 