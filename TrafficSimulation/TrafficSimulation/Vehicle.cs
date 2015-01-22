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
        private int updatePoint;
        private Point beginPoint, endPoint;
        private double tempX, tempY;
        private Size updateSize;
        private double updateLength;

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            updatePoint = 0;
            position = pos;
            destination = dest;
            size = new Size(10, len);
            this.speed = speed;
            this.direction = direction;
            this.lane = lane;
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();
        }

        public Point Destination { get { return destination; } }
        public int Direction { get { return direction; } set { value = direction; } }
        public int Lane { get { return lane; } }
        public Bitmap Bitmap { get { return bitmap; } }
        public Size Size { get { return size; } }
        public int UpdatePoint { get { return updatePoint; } }
        public int Speed { get { return speed; } set { speed = value; } }

        public void Update(Point endPosition)
        {
            if (updatePoint == 0)
            {
                tilePoint = new Point(this.position.X / 100 * 100, this.position.Y / 100 * 100);
                beginPoint = this.position;
                endPoint = endPosition;
                updateSize = new Size(endPoint.X - beginPoint.X, endPoint.Y - beginPoint.Y);
                updateLength = Math.Ceiling(Math.Sqrt(updateSize.Width * updateSize.Width + updateSize.Height * updateSize.Height));
            }
            
            int test1 =GetEndDirection(new Point(endPosition.X-tilePoint.X,endPosition.Y-tilePoint.Y));
            switch (direction + "-->" + GetEndDirection(new Point(endPosition.X-tilePoint.X,endPosition.Y-tilePoint.Y)))
            {
                case "1-->1":
                    tempX += ((double)updateSize.Width / updateLength) * speed;
                    while (tempX >= 1)
                    {
                        if (endPosition.X < position.X)
                        {
                            position.X--;
                        }
                        else if (endPosition.X > position.X)
                        {
                            position.X++;
                        }
                        tempX--;
                    }

                    tempY += ((double)updateSize.Height / updateLength) * speed;
                    while (tempY >= 1)
                    {
                        position.Y--;
                        tempY--;
                    }
                    break;
                case "1-->2":
                    break;
                case "1-->4":
                    break;

                case "2-->1":
                    break;
                case "2-->2":
                    position.X += speed;
                    break;
                case "2-->3":
                    break;

                case "3-->2":
                    break;
                case "3-->3":
                    position.Y += speed;
                    break;
                case "3-->4":
                    break;

                case "4-->1":
                    break;
                case "4-->4":
                    position.X += speed;
                    break;
                case "4-->3":
                    break;
            }
            updatePoint++;
        }

        public void reset()
        {
            updatePoint = 0;
        }


        private int GetEndDirection(Point end)
        {
            if (end.X == 0)
            {
                return 4;
            }
            else if (end.X == 100)
            {
                return 2;
            }
            else if (end.Y == 0)
            {
                return 1;
            }
            else if (end.Y == 100)
            {
                return 3;
            }
            else
            {
                return 0;
            }
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
