using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;

namespace TrafficSimulation
{
    public abstract class Vehicle
    {
        public Point position;
        protected Point destination;
        protected Bitmap bitmap;
        protected Size size;
        protected int width;
        protected int speed;
        protected int direction;
        protected int lane;
        protected static System.Security.Cryptography.RNGCryptoServiceProvider rnd;

        private Point tilePoint;
        private Point[] turnPosition;
        private int turnPoint;

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            turnPoint = 0;
            position = pos;
            destination = dest;
            size = new Size(10, len);
            this.speed = speed;
            this.direction = direction;
            this.lane = lane;
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
        }

        public Point Destination { get { return destination; } }
        public int Direction { get { return direction; } }
        public int Lane { get { return lane; } }
        public Bitmap Bitmap { get { return bitmap; } }
        public Size Size { get { return size; } }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public void Update(Point endPosition)
        {
            if (turnPoint == 0)
            {
                turnInit(endPosition);
            }

            for (int i = 0; i < speed && turnPoint < turnPosition.Length - 1; i++)
            {
                this.position.X += turnPosition[turnPoint].X;
                this.position.Y += turnPosition[turnPoint].Y;
                turnPoint++;
            }

            if (turnPoint >= turnPosition.Length)
                switch (direction)
                {
                    case 1:
                        position.Y -= 1;
                        break;
                    case 2:
                        position.X += 1;
                        break;
                    case 3:
                        position.Y += 1;
                        break;
                    case 4:
                        position.X -= 1;
                        break;
                }

            turnPoint++;
        }

        private void turnInit(Point endPosition)
        {
            turnPosition = Curves.GetCurves(this.direction, new Point(endPosition.X -
                (this.position.X / 100 * 100), endPosition.Y - (this.position.Y / 100 * 100)));

            switch (direction)
            {
                case 1:
                    turnPoint = (int)((100-position.Y % 100) / turnPosition.Length);
                    break;
                case 2:
                    break;
                case 3:
                    turnPoint = (int)(position.Y % 100 / turnPosition.Length);
                    break;
                case 4:
                    break;
            }

            turnPosition[turnPosition.Length - 1] = endPosition;
        }

        protected void createBitmap(int bmDirection)
        {
            Graphics gr;
            Rectangle destCar;
            Bitmap carBitmap;
            {
                Byte[] random;
                random = new Byte[1];
                rnd.GetBytes(random);
                string carName = "car" + (((int)random[0] % 5) + 1);
                carBitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject(carName);
                bitmap = new Bitmap(carBitmap.Width + speed, carBitmap.Height);
                gr = Graphics.FromImage(bitmap);
                if (bmDirection == 2)
                {
                    destCar = new Rectangle(new Point(speed, 0), carBitmap.Size);
                }
                else if (bmDirection == 4)
                {
                    carBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    destCar = new Rectangle(new Point(0, 0), carBitmap.Size);
                }
                else if (bmDirection == 1)
                {
                    carBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    bitmap = new Bitmap(carBitmap.Width, carBitmap.Height + speed);
                    gr = Graphics.FromImage(bitmap);
                    destCar = new Rectangle(new Point(0, 0), carBitmap.Size);
                }
                else // bmDirection == 3
                {
                    carBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    bitmap = new Bitmap(carBitmap.Width, carBitmap.Height + speed);
                    gr = Graphics.FromImage(bitmap);
                    destCar = new Rectangle(new Point(0, 0), carBitmap.Size);
                }
            }
            gr.DrawImage(carBitmap, destCar, new Rectangle(new Point(), carBitmap.Size), GraphicsUnit.Pixel);

        }
    }

    public class NormalCar : Vehicle
    {
        public NormalCar(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {
            switch (direction)
            {
                case 1:
                    createBitmap(1);
                    position.X += 52 + (17 * lane);
                    position.Y += 70 - 15;
                    break;
                case 2:
                    createBitmap(2);
                    position.Y += 53 + 17 * lane;
                    position.X += 30;
                    break;
                case 3:
                    createBitmap(3);
                    position.X += (37 - (17 * lane));
                    position.Y += 30;
                    break;
                case 4:
                    createBitmap(4);
                    position.X += 70 - 15;
                    position.Y += (37 - (17 * lane));
                    break;
            }
        }
    }

    public class Truck : Vehicle
    {
        Truck(Point pos, Point dest, int len, int speed, int direction, int lane)
            : base(pos, dest, len, speed, direction, lane)
        {
            Bitmap tempBitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("truck1");
        }
    }
}
