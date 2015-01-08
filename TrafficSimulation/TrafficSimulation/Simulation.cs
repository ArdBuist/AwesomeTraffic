﻿using System;
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
        public bool simStarted;

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
                //spawners verzamelen
                if (Methods.CheckValidConnections(simControl))
                {
                    spawnerList.Clear();
                    simControl.controlList.Clear();
                    foreach (Tile t in simControl.tileList)
                    {
                        if (t != null && t.name.Equals("Spawner"))
                        {
                            spawnerList.Add((Spawner)t);
                        }
                    }
                    simControl.MakeTrafficControlList();

                    ThreadStart threadDelegate = new ThreadStart(Update);
                    thread = new Thread(threadDelegate);
                    thread.Start();
                    simStarted = true;
                }
                else
                {
                    simControl.simwindow.windowselect.ShowMessage("niet alle tiles liggen aan elkaar");

                }
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
            Tile[] tiles =simControl.tileList;
            Array.Copy(simControl.tileList, tiles, simControl.tileList.Count());
            //alle auto's updaten en weer tekenen
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    List<List<Vehicle>> tileVehicles = new List<List<Vehicle>>();
                        foreach(List<List<Vehicle>> list in t.vehicles)
                        {
                            foreach(List<Vehicle> vehiclelist in list)
                            tileVehicles.Add(new List<Vehicle>(vehiclelist));
                        }
                    foreach (List<Vehicle> list in tileVehicles)
                    {
                        foreach (Vehicle v in list)
                        {

                            if (VehicleIsOnEndSpawner(v,t))
                            {
                                simControl.tileList[t.listPlace].RemoveVehicle(v, v.Direction, v.Lane);
                                break;
                            
                            }
                            simControl.vehicleBC.AddObject(v.Bitmap, v.position.X, v.position.Y);
                            if (v.position.X-v.Speed>= t.position.X && v.position.X+v.Speed<= t.position.X + t.size.Width&&
                                v.position.Y-v.Speed>= t.position.Y && v.position.Y+v.Speed<= t.position.Y + t.size.Height )
                            {
                                v.Update();
                            }
                            else
                            {
                                //if (VehicleIsOnSpawner(v, t)==false)
                                {
                                    
                                }
                                //else
                                {
                                    v.Update();
                                    Tile nextTile = simControl.tileList[t.listPlace].GetOtherTile(simControl, v.Direction);
                                    if (nextTile != null)
                                        nextTile.AddVehicle(v, v.Direction, v.Lane);
                                    simControl.tileList[t.listPlace].RemoveVehicle(v, v.Direction, v.Lane);
                                }
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
        private bool VehicleIsOnEndSpawner(Vehicle v, Tile t)
        {
            if(t.name == "Spawner" && t.Directions.Contains((v.Direction+1)%4+1))
            {
                
                switch (v.Direction)
                {
                    case 1: if (v.position.Y - v.Speed <= t.position.Y + 30)
                            return true;
                        break;
                    case 2: if (v.position.X + v.Speed+15 >= t.position.X + 70)
                            return true;
                        break;
                    case 3: if (v.position.Y + v.Speed+15 >= t.position.Y + 70)
                            return true;
                        break;
                    case 4: if (v.position.X - v.Speed <= t.position.X + 30)
                            return true;
                        break;

                }
                              
               {

               }
            }
            return false;
        }
    }
}