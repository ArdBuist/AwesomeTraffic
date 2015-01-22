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
        private int nextDirection;
        private int lastDirection;
        private int updatePoint;
        private Point beginPoint, endPoint;
        private double tempX, tempY;
        private Size updateSize;
        private double updateLength;

        public Vehicle(Point pos, Point dest, int len, int speed, int direction, int lane)
        {
            updatePoint = 999;
            position = pos;
            destination = dest;
            size = new Size(10, len);
            this.speed = speed;
            this.direction = direction;
            nextDirection = direction;
            this.lane = lane;
            rnd = new System.Security.Cryptography.RNGCryptoServiceProvider();

            switch (direction)
            {
                case 1:
                    Instantiate(new Point(this.position.X, this.position.Y - 100));
                    break;
                case 2:
                    Instantiate(new Point(this.position.X + 100, this.position.Y));
                    break;
                case 3:
                    Instantiate(new Point(this.position.X, this.position.Y + 100));
                    break;
                case 4:
                    Instantiate(new Point(this.position.X - 100, this.position.Y));
                    break;
            }
        }

        public Point Destination { get { return destination; } }
        public int Direction { get { return direction; } set { value = direction; } }
        public int Lane { get { return lane; } }
        public Bitmap Bitmap { get { return bitmap; } }
        public Size Size { get { return size; } }
        public int UpdatePoint { get { return updatePoint; } }
        public int Speed { get { return speed; } set { speed = value; } }
        public int NextDirection { get { return nextDirection; } set { nextDirection = value; } }
        public int LastDirection { get { return lastDirection; } set { lastDirection = value; } }

        public void Update(Tile t, Point endPosition)
        {
            if (updatePoint == 0)
            {
                Instantiate(endPosition);
                getEndDirection();

                //switch (direction)
                //{
                //    case 1:
                //        position.Y -= speed;
                //        break;
                //    case 2:
                //        position.X += speed;
                //        break;
                //    case 3:
                //        position.Y += speed;
                //        break;
                //    case 4:
                //        position.X -= speed;
                //        break;
                //}
            }

            // de case voor elke mogelijkheid Direction --> Direction
            // de direction waar de auto vandaan komt en waar hij naar toe gaat gescheiden door een pijl


            //nodig omdat een update niet altijd een heel getal is.
            tempX += ((double)updateSize.Width / updateLength) * speed;
            tempY += ((double)updateSize.Height / updateLength) * speed;

            switch (direction + "-->" + nextDirection)
            {
                case "1-->1":
                    //aanpassing in de x richting die positief of negatief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY <= -1)
                    {
                        position.Y--;
                        tempY++;
                    }
                    break;
                case "1-->2":
                    //aanpassing in de x richting die altijd positief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY <= -1)
                    {
                        position.Y--;
                        tempY++;
                    }
                    break;
                case "1-->4":
                    //aanpassing in de x richting die altijd negatief is
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd positief is
                    while (tempY >= 1)
                    {
                        position.Y++;
                        tempY--;
                    }
                    break;

                case "2-->1":
                    //aanpassing in de x richting die altijd negatief is
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY <= -1)
                    {
                        position.Y--;
                        tempY++;
                    }
                    break;
                case "2-->2":
                    //aanpassing in de x richting die altijd negatief is
                    while (tempX >= 1)
                    {
                        position.X++;
                        tempX--;
                    }
                    //aanpassing in de y richting die positief of negatief is
                    while (tempY >= 1)
                    {
                        this.position.Y++;
                        tempY--;
                    }
                    while (tempY <= -1)
                    {
                        this.position.Y--;
                        tempY++;
                    }
                    break;
                case "2-->3":
                    //aanpassing in de x richting die altijd negatief is
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd positief is
                    while (tempY >= 1)
                    {
                        position.Y++;
                        tempY--;
                    }
                    break;

                case "3-->2":
                    //aanpassing in de x richting die altijd positief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY >= 1)
                    {
                        position.Y++;
                        tempY--;
                    }
                    break;
                case "3-->3":
                    //aanpassing in de x richting die positief of negatief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY >= 1)
                    {
                        position.Y++;
                        tempY--;
                    }
                    break;
                case "3-->4":
                    //aanpassing in de x richting die altijd negatief is
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY <= -1)
                    {
                        position.Y--;
                        tempY++;
                    }
                    break;

                case "4-->1":
                    //aanpassing in de x richting die altijd positief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    //aanpassing in de y richting die altijd negatief is
                    while (tempY <= -1)
                    {
                        position.Y--;
                        tempY++;
                    }
                    break;
                case "4-->4":
                    //aanpassing in de x richting die altijd positief is
                    while (tempX <= -1)
                    {
                        this.position.X--;
                        tempX++;
                    }
                    //aanpassing in de y richting die positief of negatief is
                    while (tempY >= 1)
                    {
                        this.position.Y++;
                        tempY--;
                    }
                    while (tempY <= -1)
                    {
                        this.position.Y--;
                        tempY++;
                    }
                    break;
                case "4-->3":
                    //aanpassing in de x richting die altijd positief is
                    while (tempX >= 1)
                    {
                        this.position.X++;
                        tempX--;
                    }
                    //aanpassing in de y richting die altijd positief is
                    while (tempY >= 1)
                    {
                        position.Y++;
                        tempY--;
                    }
                    break;
            }
            updatePoint++;
        }

        private void Instantiate(Point endPosition)
        {
            //als de Vehicle een nieuwe tile oprijd dan is deze 0 en worden deze variabelen geinstantieerd
            tilePoint = new Point(this.position.X / 100 * 100, this.position.Y / 100 * 100);
            //beginpunt van de beweging
            beginPoint = this.position;
            //eindpunt van de beweging
            endPoint = endPosition;
            //de breedte en de hoogte van de beweging, hoever de auto beweegt over de x- en y-as
            updateSize = new Size(endPoint.X - beginPoint.X, endPoint.Y - beginPoint.Y);
            //met pythagoras word de lengte van de beweging uitgerekend
            updateLength = Math.Ceiling(Math.Sqrt(updateSize.Width * updateSize.Width + updateSize.Height * updateSize.Height));

            getEndDirection();
        }

        public void getEndDirection()
        {
            if (endPoint.X - tilePoint.X == 0)
            {
                nextDirection = 4;
            }
            else if (endPoint.X - tilePoint.X == 100)
            {
                nextDirection = 2;
            }
            else if (endPoint.Y - tilePoint.Y == 0)
            {
                nextDirection = 1;
            }
            else if (endPoint.Y - tilePoint.Y == 100)
            {
                nextDirection = 3;
            }
        }

        public void reset()
        {
            //variabele die hier word gereset als de auto naar een andere tile rijd (nu nog ongebruikt)
            updatePoint = 0;
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
