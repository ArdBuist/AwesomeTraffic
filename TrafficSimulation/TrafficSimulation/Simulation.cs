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
        public bool simStarted;
        protected int waitingCars;
        Boolean simPause;
        protected int simSleep;
        public int extraSpeed;
        public List<string[]> efficiencyNumbers;

        public Simulation(SimControl simControl)
        {
            this.simControl = simControl;
            this.simStarted = false;
            spawnerList = new List<Spawner>();
            waitingCars = 0;
            simSleep = 70;
            extraSpeed = 0;
            efficiencyNumbers = new List<string[]>();
            
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
                //clearing list for calculating efficiency
                efficiencyNumbers.Clear();
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
                            if(v.Direction == 2 && t.name=="Crossroad")
                            {

                            }
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
            string date = DateTime.Now.ToString("H:mm:ss");
            string[] efficiencyTime = new string[] { DateTime.Now.ToString("H:mm:ss:fff"), ""+simControl.simwindow.InfoBalk.labelEfficientieNumber.Content };
            efficiencyNumbers.Add(efficiencyTime);
            if (simControl.simwindow.InfoBalk.EfWindow != null)
                simControl.simwindow.InfoBalk.EfWindow.t_Tick(null, null);
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
                simControl.simulationMap.GetTileMea(t.position.X,t.position.Y).RemoveVehicle(simControl, v, v.Direction, v.Lane);
                simControl.totalCars--;
            }
            if (StaysOnTile(t, v))//if vehicle is still on the tile 
            {
                if (DistanceFromCars(t, v))//if there are other cars standing in front
                    v.Update();
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
                    Tile nextTile = simControl.simulationMap.GetSurroundingTilesSim(t.position)[v.Direction-1];
                    if (nextTile != null)
                    {
                        v.Speed = nextTile.maxSpeed;
                        nextTile.AddVehicle(simControl, v, v.Direction, v.Lane);
                        simControl.totalCars++;
                        v.Update();
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
            List<List<Vehicle>> vehicleList = simControl.simulationMap.GetTileMea(t.position.X,t.position.Y).vehicles[v.Direction - 1];
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
                    case 1: if (v.position.Y - v.Speed <= t.position.Y + 15)
                            return true;
                        break;
                    case 2: if (v.position.X + v.Speed + 15 >= t.position.X + 85)
                            return true;
                        break;
                    case 3: if (v.position.Y + v.Speed + 15 >= t.position.Y + 85)
                            return true;
                        break;
                    case 4: if (v.position.X - v.Speed <= t.position.X + 15)
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