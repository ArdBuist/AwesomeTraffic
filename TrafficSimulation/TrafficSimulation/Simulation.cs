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
                        if (t != null)
                        {
                            t.Initialize();
                            if (t.name.Equals("Spawner"))
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
                
                
                //standaardtijd ingebouwd voor een mooiere weergave.
                Thread.Sleep(1000/30);
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
                            UpdateVehicle(t, v);
                            simControl.vehicleBC.AddObject(v.Bitmap, v.position.X, v.position.Y);
                        }
                    }
                }
            }
            //updaten van de trafficlights
            foreach(TrafficlightControl tL in simControl.controlList)
            {
                tL.Run();
            }
            //updaten van de spawners
            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick(simControl);
            }
            simControl.Invalidate();
        }

        private void UpdateVehicle(Tile t, Vehicle v)
        {
            v.Speed = t.MaxSpeed;
            //if vehicle has to dissapear ----- moet worden vervangen door zwart vlak over de spawner-----
            if (VehicleIsOnEndSpawner(v, t))
            {
                simControl.tileList[t.listPlace].RemoveVehicle(simControl, v, v.Direction, v.Lane);
            }
            if (StaysOnTile(t, v))//if vehicle is still on the tile 
            {
                if (DistanceFromCars(t, v))//if there are other cars standing in front
                    v.Update();
                else
                    v.Speed = 0;
            }
            else
            {
                if (t.Access[v.Direction - 1, v.Lane])//if the next tile is accessible
                {
                    //remove vehicle from old tile and add vehicle to new tile
                    Tile nextTile = simControl.tileList[t.listPlace].GetOtherTile(simControl, v.Direction);
                    if (nextTile != null)
                    {
                        v.Speed = nextTile.maxSpeed;
                        nextTile.AddVehicle(simControl, v, v.Direction, v.Lane);
                        v.Update();
                    }
                    simControl.tileList[t.listPlace].RemoveVehicle(simControl, v, v.Direction, v.Lane);
                }
                else
                {
                    v.Speed = 0;
                }
            }
        }

        //returns if an object is still on the tile
        private bool StaysOnTile(Tile t, Vehicle v)
        {
            return CorrectDistance(t, v, 0);
        }

        //returns if there are no other cars standing in front
        private bool DistanceFromCars(Tile t, Vehicle v)
        {
            int distance = 0;//distance between the end of the tile and the last car standing still.
            List<List<Vehicle>> vehicleList = simControl.tileList[t.listPlace].vehicles[v.Direction - 1];
            distance = vehicleList[v.Lane].IndexOf(v) * 16;
            return CorrectDistance(t, v, distance);
        }

        //calculates the places and returns if the vehicle is allowed to drive
        private bool CorrectDistance(Tile t, Vehicle v, int CarSpace)
        {
            switch (v.Direction)
            {
                case 1: if (v.position.Y - t.MaxSpeed >= t.position.Y + CarSpace)
                        return true;
                    break;
                case 2: if (v.position.X + t.MaxSpeed + 15 <= t.position.X + t.size.Width - CarSpace)
                        return true;
                    break;
                case 3: if (v.position.Y + t.MaxSpeed + 10 <= t.position.Y + t.size.Height - CarSpace)
                        return true;
                    break;
                case 4: if (v.position.X - t.MaxSpeed >= t.position.X + CarSpace)
                        return true;
                    break;
            }
            return false;
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