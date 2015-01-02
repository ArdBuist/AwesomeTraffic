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

            
        }

        public void StartSim()
        {
            if (simStarted == false)
            {
                foreach (Tile t in simControl.tileList)
                {
                    if (t != null && t.name.Equals("Spawner"))
                    {
                        spawnerList.Add((Spawner)t);
                    }
                }
                ThreadStart threadDelegate = new ThreadStart(Update);
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

        public void Update()
        {
            while (true)
            {
                if (simControl.InvokeRequired)
                    simControl.Invoke(new MethodInvoker(UpdateGame));
                else
                    UpdateGame();
                
                foreach (Spawner spawn in spawnerList)
                {
                    spawn.Tick();
                }
                //standaardtijd ingebouwd voor een mooiere weergave.
                Thread.Sleep(1000/60);
            }
        }
        public void UpdateGame()
        {
            //de vehiclemap wordt weer helemaal leeg gemaakt zodat de auto's maar 1 keer getekend worden
            Graphics g = Graphics.FromImage((Image)simControl.vehicleBC.bitmap);
            g.Clear(Color.Transparent);
            //alle auto's updaten en weer tekenen
            foreach (Tile t in simControl.tileList)
            {

                if (t != null)
                {
                    //List<List<Vehicle>> tileVehicles = new List<List<Vehicle>>();
                    //List<ICloneable> oldList = new List<ICloneable>();
                    //List<ICloneable> newList = new List<ICloneable>(oldList.Count);

                    //t.vehicles.ForEach((List<Vehicle> item) =>
                    //{
                    //    tileVehicles.Add(new List<Vehicle> item);
                    //});
                    List<List<Vehicle>> tileVehicles = new List<List<Vehicle>>();
                    foreach(List<Vehicle> list in t.vehicles)
                    {
                        tileVehicles.Add(new List<Vehicle>(list));
                    }
                    foreach (List<Vehicle> list in tileVehicles)
                    {
                        foreach (Vehicle v in list)
                        {
                            v.Update();
                            //t.changeDirection(v);
                            simControl.vehicleBC.AddObject(v.Bitmap, v.position.X, v.position.Y);
                            if (v.position.X - v.Speed >= t.position.X && v.position.X + v.Speed <= t.position.X + t.size.Width - v.Size.Width - v.Speed &&
                                v.position.Y - v.Speed >= t.position.Y && v.position.Y + v.Speed <= t.position.Y + t.size.Height - v.Size.Height - v.Speed)
                            {
                            }
                            else
                            {
                                t.vehicles[v.Lane].Remove(v);
                                Tile nextTile = t.GetOtherTile(simControl, v.Direction);
                                if(nextTile!=null)
                                    nextTile.vehicles[v.Lane].Add(v);
                                break;
                            }
                        }
                    }
                }
            }
            //updaten van de trafficlights
            foreach(TrafficlightControl tL in simControl.controlList)
            {
                tL.Run();
            }
            simControl.Invalidate();
        }
    }
}