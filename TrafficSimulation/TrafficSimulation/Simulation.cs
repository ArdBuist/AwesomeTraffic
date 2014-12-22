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
        private SimControl simControl;
        private List<Spawner> spawnerList;

        public delegate void MethodInvoker();
        public Thread thread;
        private bool simStarted;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            this.simStarted = false;
            spawnerList = new List<Spawner>();

            foreach (Tile t in simControl.tileList)
            {
                if (t != null && t.name.Equals("Spawner"))
                {
                    spawnerList.Add((Spawner)t);
                }
            }
        }

        public void StartSim()
        {
            if (simStarted == false)
            {
                ThreadStart threadDelegate = new ThreadStart(UpdateVehicles);
                thread = new Thread(threadDelegate);
                thread.Start();
                simStarted = true;
            }
            else
            {
                thread.Abort();
                simStarted = false;
            }
        }

        public void UpdateVehicles()
        {
            while (true)
            {
                foreach (Tile t in simControl.tileList)
                {
                    if (t != null)
                    {
                        foreach (List<Vehicle> list in t.vehicles)
                        {
                            foreach (Vehicle v in list)
                            {
                                v.Update();
                                simControl.vehicleBC.AddObject(v.Bitmap, v.position.X, v.position.Y);

                                if (simControl.InvokeRequired)
                                    simControl.Invoke(new MethodInvoker(simControl.Invalidate));
                                else
                                    simControl.Invalidate();
                            }
                        }
                    }
                }
                foreach (Spawner spawn in spawnerList)
                {
                    spawn.Tick();
                }
            }
        }
    }
}