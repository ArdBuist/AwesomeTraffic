using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TrafficSimulation
{
    public static class Curves
    {
        public static int length = (int)(25 * Math.PI);
        private static int r = 50;

        private static Point[] rightToUp;
        private static Point[] upToLeft;
        private static Point[] leftToDown;
        private static Point[] downToRight;
        private static Point[] upToDown;
        private static Point[] leftToRight;

        private static Point[] upToRight;
        private static Point[] leftToUp;
        private static Point[] downToLeft;
        private static Point[] rightToDown;
        private static Point[] downToUp;
        private static Point[] rightToLeft;

        public static void Instantiate()
        {
            RightToUp();
            UpToLeft();
            LeftToDown();
            DownToRight();
            LeftToRight();
            UpToDown();
        }
        /*
         * straal r = 50
         * omtrek o = 2*pi*r = 100*Math.PI
         * een bocht is 1/4 cirkel = 25*Math.PI
         * een bocht is 1/2pi rad dus een stukje bocht is (1/25*Math.PI)*0.5*Math.PI = 1/50
         * dit tel je op bij waar je begint, als je rechts begint is dan 0pi en als je links begint is dat 1pi
         * dus 1 stap in de bocht van rechts naar boven is Math.PI+1/50*1 en die daarna Math.PI+1/50*2
         * 
         */
        private static void RightToUp()
        {
            rightToUp = new Point[(int)length];
            for (int i = 0; i < rightToUp.Length; i++)
            {
                //rightToUp[i].X = 100+(int)(r*Math.Cos(Math.PI+((1.0/(25*Math.PI))*Math.PI*0.5*i)));
                rightToUp[i].X = 100 + (int)(r * Math.Cos(Math.PI + (1.0 / 50.0) * i));
                //rightToUp[i].Y=(int)(r*Math.Sin(Math.PI+((1.0/(25*Math.PI))*Math.PI*0.5*i)));
                rightToUp[i].Y = (int)(r * Math.Sin(Math.PI + (1.0 / 50.0) * i));
            }
            upToRight = CorrectedArray(ReverseArray(rightToUp));
            rightToUp = CorrectedArray(rightToUp);

            for (int i = 0; i < rightToUp.Length; i++)
            {
                rightToUp[i].X *= -1;
                rightToUp[i].Y *= -1;
            }

            Point[] test1 = upToRight;
            Point[] test2 = rightToUp;
        }

        private static void UpToLeft()
        {
            upToLeft = new Point[(int)length];

            for (int i = 0; i < rightToUp.Length; i++)
            {
                //rightToUp[i].X = 100+(int)(r*Math.Cos(Math.PI+((1.0/(25*Math.PI))*Math.PI*0.5*i)));
                rightToUp[i].X = (int)(r * Math.Cos(Math.PI + ((1.0 / (25 * Math.PI)) * Math.PI * 0.5 * i)));
                //rightToUp[i].Y=(int)(r*Math.Sin(Math.PI+((1.0/(25*Math.PI))*Math.PI*0.5*i)));
                rightToUp[i].Y = (int)(r * Math.Sin(0));
            }

            leftToUp = ReverseArray(upToLeft);
        }

        private static void LeftToDown()
        {
            leftToDown = new Point[(int)length];

            for (int i = 0; i < rightToUp.Length; i++)
            {
                double hoek = ((90.0 / length) * i) + 180;
                rightToUp[i].X = (int)(r * Math.Cos(hoek));
                rightToUp[i].Y = (int)(100 + r * Math.Sin(hoek));
            }

            downToLeft = ReverseArray(leftToDown);
        }

        private static void DownToRight()
        {
            downToRight = new Point[(int)length];

            for (int i = 0; i < rightToUp.Length; i++)
            {
                double hoek = ((90.0 / length) * i) + 270;
                rightToUp[i].X = (int)(100 + r * Math.Cos(hoek));
                rightToUp[i].Y = (int)(100 + r * Math.Sin(hoek));
            }

            rightToDown = ReverseArray(downToRight);
        }

        private static void UpToDown()
        {
            upToDown = new Point[100];
            for (int i = 0; i < upToDown.Length; i++)
            {
                upToDown[i] = new Point(50, i);
            }

            downToUp = CorrectedArray(ReverseArray(upToDown));
            upToDown = CorrectedArray(upToDown);
            for (int i = 0; i < downToUp.Length; i++)
            {
                downToUp[i].Y *= -1;
            }
        }

        private static void LeftToRight()
        {
            leftToRight = new Point[100];
            for (int i = 0; i < leftToRight.Length; i++)
            {
                leftToRight[i] = new Point(i, 50);
            }

            rightToLeft = ReverseArray(leftToRight);
        }

        private static Point[] ReverseArray(Point[] original)
        {
            Point[] reverse = new Point[original.Length];

            int j = original.Length - 1;
            foreach (Point p in original)
            {
                reverse[j] = p;
                j--;
            }

            return reverse;
        }

        private static Point[] CorrectedArray(Point[] original)
        {
            Point[] corrected = new Point[original.Length];

            for (int i = 0; i < original.Length - 1; i++)
            {
                corrected[i] = new Point(Math.Abs(original[i].X - original[i + 1].X), Math.Abs(original[i].Y - original[i + 1].Y));
            }
            corrected[corrected.Length - 1] = corrected[corrected.Length - 2];
            return corrected;
        }

        public static Point[] GetCurves(int direction, Point endPosition)
        {
            if (direction == 1 && endPosition.X == 0)
            {
                return downToLeft;
            }
            else if (direction == 1 && endPosition.X == 100)
            {
                return downToRight;
            }
            else if (direction == 1 && endPosition.Y == 0)
            {
                return downToUp;
            }

            else if (direction == 2 && endPosition.X == 100)
            {
                return leftToRight;
            }
            else if (direction == 2 && endPosition.Y == 0)
            {
                return leftToUp;
            }
            else if (direction == 2 && endPosition.Y == 100)
            {
                return leftToDown;
            }

            else if (direction == 3 && endPosition.X == 0)
            {
                return upToLeft;
            }
            else if (direction == 3 && endPosition.X == 100)
            {
                return upToRight;
            }
            else if (direction == 3 && endPosition.Y == 100)
            {
                return upToDown;
            }

            else if (direction == 4 && endPosition.X == 0)
            {
                return rightToLeft;
            }
            else if (direction == 4 && endPosition.Y == 0)
            {
                return rightToDown;
            }
            else if (direction == 4 && endPosition.Y == 100)
            {
                return rightToDown;
            }

            else
            {
                return null;
            }
        }
    }
}
