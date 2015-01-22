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
        protected int waitingCars;
        Boolean simPause;
        protected int simSleep;
        public int extraSpeed;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            this.simStarted = false;
            spawnerList = new List<Spawner>();
            waitingCars = 0;
            simSleep = 50;
            extraSpeed = 0;

        }
        public int WaitingCars
        {
            get { return waitingCars; }
            set { waitingCars = value; }
        }
        public int PauseSeconds
        {
            get { return simSleep; }
            set { if (simSleep > 10 && simSleep < 800)simSleep = value; }
        }
        public bool StartSim()
        {
            if (simStarted == false)
            {
                //spawners verzamelen
                if (Methods.CheckValidConnections(simControl))
                {
                    spawnerList.Clear();
                    simControl.controlList.Clear();

                    foreach (Tile t in simControl.simulationMap.GetMap())
                    {
                        t.Initialize();
                        if (t.name.Equals("Spawner"))
                            spawnerList.Add((Spawner)t);
                    }
                    simControl.MakeTrafficControlList();
                    ThreadStart threadDelegate = new ThreadStart(Update);
                    thread = new Thread(threadDelegate);
                    thread.Start();
                    simStarted = true;
                    return true;
                }
                else
                {
                    simControl.simwindow.windowselect.ShowMessage("niet alle tiles liggen aan elkaar");
                    return false;

                }
            }
            else
            {
                thread.Abort();
                simStarted = false;
                simControl.UpdateInfoBalkSimulatie();
                return false;
            }
        }

        public void StartSimknop()
        {
            if (!simStarted)
                StartSim();
            else
                simPause = false;

        }
        public void PauseSimknop()
        {
            simPause = true;
        }
        public void Update()
        {
            while (true)
            {
                if (!simPause)
                {
                    if (simControl.InvokeRequired)
                        simControl.Invoke(new MethodInvoker(UpdateGame));
                    else
                        UpdateGame();
                }
                //simControl.DrawSelectLine(simControl.selectedTile.position);
                //standaardtijd ingebouwd voor een mooiere weergave.
                Thread.Sleep(simSleep);
            }
        }

        public void UpdateGame()
        {
            //de vehiclemap wordt weer helemaal leeg gemaakt zodat de auto's maar 1 keer getekend worden
            Graphics g = Graphics.FromImage((Image)simControl.vehicleBC.bitmap);
            g.Clear(Color.Transparent);
            Tile[] tiles = simControl.simulationMap.GetMap().ToArray();
            Array.Copy(simControl.simulationMap.GetMap().ToArray(), tiles, simControl.simulationMap.GetMap().Count());
            //alle auto's updaten en weer tekenen
            foreach (Tile t in tiles)
            {
                if (t != null)
                {
                    List<List<Vehicle>> tileVehicles = new List<List<Vehicle>>();
                    foreach (List<List<Vehicle>> list in t.vehicles)
                    {
                        foreach (List<Vehicle> vehiclelist in list)
                            tileVehicles.Add(new List<Vehicle>(vehiclelist));
                    }
                    foreach (List<Vehicle> list in tileVehicles)
                    {
                        foreach (Vehicle v in list)
                        {
                            UpdateVehicle(t, v);
                            simControl.vehicleBC.AddObject(v.Bitmap, v.position);
                        }
                    }
                }
            }
            //updaten van de trafficlights
            foreach (TrafficlightControl tL in simControl.controlList)
            {
                tL.Run(extraSpeed, (double)simSleep - (double)50);
            }
            //updaten van de spawners
            foreach (Spawner spawn in spawnerList)
            {
                spawn.Tick(simControl, extraSpeed, (double)simSleep - (double)50);
            }
            if (simControl.selectedTile != null)
                simControl.DrawSelectLine(simControl.selectedTile.position);
            simControl.UpdateInfoBalkSimulatie();
            simControl.Invalidate();
        }

        private void UpdateVehicle(Tile t, Vehicle v)
        {
            if (v.Speed == 0)
                waitingCars--;
            v.Speed = t.MaxSpeed + extraSpeed;
            //if vehicle has to dissapear ----- moet worden vervangen door zwart vlak over de spawner-----
            if (VehicleIsOnEndSpawner(v, t))
            {
                simControl.simulationMap.GetTileMea(t.position.X, t.position.Y).RemoveVehicle(simControl, v, v.Direction, v.Lane);
                simControl.totalCars--;
            }
            if (StaysOnTile(t, v))//if vehicle is still on the tile 
            {
                if (DistanceFromCars(t, v))//if there are other cars standing in front
                    v.Update(GetEndPosition(t, v));
                else
                {
                    v.Speed = 0;
                    waitingCars++;
                }
            }
            else
            {
                if (t.Access[v.Direction - 1, v.Lane])//if the next tile is accessible
                {
                    //remove vehicle from old tile and add vehicle to new tile
                    Tile nextTile = simControl.simulationMap.GetSurroundingTilesSim(t.position)[v.Direction - 1];
                    if (nextTile != null)
                    {
                        v.Speed = nextTile.maxSpeed;
                        nextTile.AddVehicle(simControl, v, v.Direction, v.Lane);
                        v.reset();
                        simControl.totalCars++;
                        v.Update(GetEndPosition(t, v));
                    }
                    simControl.simulationMap.GetTileMea(t.position.X, t.position.Y).RemoveVehicle(simControl, v, v.Direction, v.Lane);
                    simControl.totalCars--;
                }
                else
                {
                    v.Speed = 0;
                    waitingCars++;
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
            List<List<Vehicle>> vehicleList = simControl.simulationMap.GetTileMea(t.position.X, t.position.Y).vehicles[v.Direction - 1];
            distance = vehicleList[v.Lane].IndexOf(v) * 16;
            return CorrectDistance(t, v, distance);
        }

        //calculates the places and returns if the vehicle is allowed to drive
        private bool CorrectDistance(Tile t, Vehicle v, int CarSpace)
        {
            switch (v.Direction)
            {
                case 1: if (v.position.Y - t.MaxSpeed - 1 >= t.position.Y + CarSpace)
                        return true;
                    break;
                case 2: if (v.position.X + t.MaxSpeed + v.Size.Width + t.maxSpeed + 5 <= t.position.X + t.size.Width - CarSpace)
                        return true;
                    break;
                case 3: if (v.position.Y + t.MaxSpeed + v.Size.Width + t.maxSpeed + 3 <= t.position.Y + t.size.Height - CarSpace)
                        return true;
                    break;
                case 4: if (v.position.X - t.MaxSpeed - 1 >= t.position.X + CarSpace)
                        return true;
                    break;
            }
            return false;
        }

        private bool VehicleIsOnEndSpawner(Vehicle v, Tile t)
        {
            if (t.name == "Spawner" && t.Directions.Contains((v.Direction + 1) % 4 + 1))
            {
                switch (v.Direction)
                {
                    case 1: if (v.position.Y - v.Speed <= t.position.Y + 30)
                            return true;
                        break;
                    case 2: if (v.position.X + v.Speed + 15 >= t.position.X + 70)
                            return true;
                        break;
                    case 3: if (v.position.Y + v.Speed + 15 >= t.position.Y + 70)
                            return true;
                        break;
                    case 4: if (v.position.X - v.Speed <= t.position.X + 30)
                            return true;
                        break;

                }
            }
            return false;
        }

        private Point GetEndPosition(Tile tile, Vehicle v)
        {
            int endDirection = GetRandomOutDirection(tile, v);
            int lane = v.Lane;
            //Tile endTile = simControl.simulationMap.GetConnectingTiles(tile.position)[endDirection - 1];
            //int tileLanes = endTile.GetLanesIn(endDirection);
            //int randomLane = Math.Abs(Guid.NewGuid().GetHashCode()) % tileLanes;

            //TESTCODE
            int randomLane = 0;
            //EINDE TESTCODE

            switch (endDirection)
            {
                case 1:
                    return new Point(tile.position.X + 53 + (randomLane * 17), tile.position.Y);
                case 2:
                    return new Point(tile.position.X + 100, tile.position.Y + 53 + 17 * lane);
                case 3:
                    return new Point(tile.position.X + 37 - (17 * lane), tile.position.Y + 100);
                case 4:
                    return new Point(tile.position.X, tile.position.Y + 37 - (17 * lane));
                default:
                    return new Point(0, 0);
            }
        }

        private int GetRandomOutDirection(Tile tile, Vehicle v)
        {
            Tile newTile = simControl.simulationMap.GetSurroundingTiles(tile.position)[v.Direction - 1];
            int newDirection = 0;
            switch (newTile.name)
            {
                case "Spawner":
                    newDirection = v.Direction;
                    break;
                case "Road":
                    foreach (int i in newTile.Directions)
                    {
                        if (i != ((v.Direction + 2) % 4))
                        {
                            newDirection = i;
                        }
                    }
                    break;
            }
            if (v.UpdatePoint == 0)
            {
                v.Direction = newDirection;
            }

            return newDirection;
        }
    }
}