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

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            position = pos;
            destination = dest;
            size = new Size(10, len);
            this.speed = speed;
            this.direction = direction;
            this.lane = lane;
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

        public void Update()
        {
            switch (direction)
            {
                case 1:
                    position.Y -= speed;
                    break;
                case 2:
                    position.X += speed;
                    break;
                case 3:
                    position.Y += speed;
                    break;
                case 4:
                    position.X -= speed;
                    break;
            }
        }

        protected void createBitmap(int bmDirection)
        {
            Graphics gr;
            Rectangle destCar, destTrail;
            Bitmap trailBitmap, carBitmap;

            if (bmDirection == 1 || bmDirection == 3)
            {
                carBitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("car1R");
                bitmap = new Bitmap(carBitmap.Width, carBitmap.Height + speed);
                trailBitmap = new Bitmap(carBitmap.Width, speed);
                gr = Graphics.FromImage(bitmap);

                for (int i = 0; i < trailBitmap.Size.Width; i++)
                {
                    for (int j = 0; j < trailBitmap.Size.Height; j++)
                    {
                        trailBitmap.SetPixel(i, j, Color.Gray);
                    }
                }

                if (bmDirection == 1)
                {
                    destCar = new Rectangle(new Point(0, 0), carBitmap.Size);
                    destTrail = new Rectangle(new Point(0, carBitmap.Size.Height), trailBitmap.Size);
                }
                else
                {
                    carBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    destCar = new Rectangle(new Point(0, trailBitmap.Size.Height), carBitmap.Size);
                    destTrail = new Rectangle(new Point(0, 0), trailBitmap.Size);
                }
            }
            else
            {
                carBitmap = (Bitmap)Properties.Resources.ResourceManager.GetObject("car1");
                bitmap = new Bitmap(carBitmap.Width + speed, carBitmap.Height);
                trailBitmap = new Bitmap(speed, carBitmap.Height);
                gr = Graphics.FromImage(bitmap);

                for (int i = 0; i < trailBitmap.Size.Width; i++)
                {
                    for (int j = 0; j < trailBitmap.Size.Height; j++)
                    {
                        trailBitmap.SetPixel(i, j, Color.Gray);
                    }
                }

                if (bmDirection == 2)
                {

                    destCar = new Rectangle(new Point(trailBitmap.Size.Width, 0), carBitmap.Size);
                    destTrail = new Rectangle(new Point(0, 0), trailBitmap.Size);
                }
                else //if(bmDirection == 4)
                {
                    carBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    destCar = new Rectangle(new Point(0, 0), carBitmap.Size);
                    destTrail = new Rectangle(new Point(carBitmap.Size.Width, 0), trailBitmap.Size);
                }
            }

            gr.DrawImage(carBitmap, destCar, new Rectangle(new Point(), carBitmap.Size), GraphicsUnit.Pixel);
            gr.DrawImage(trailBitmap, destTrail, new Rectangle(new Point(), trailBitmap.Size), GraphicsUnit.Pixel);
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
                    break;
                case 2:
                    createBitmap(2);
                    break;
                case 3:
                    createBitmap(3);
                    break;
                case 4:
                    createBitmap(4);
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
