using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace TrafficSimulation
{
    public static class DrawTile
    {
        //roadW is de breedte van de weg
        const int roadW = 16;


        //aanmaken pen die in 1 lijn streepjes zet van 5 px per stuk
        public static Pen strPen()
        {
            float[] stripesLine = new float[20];

            for (int t = 0; t < stripesLine.Length; t++)
            {
                stripesLine[t] = 5;
            }

            Pen stripesPen = new Pen(Color.White);
            stripesPen.DashPattern = stripesLine;
            return stripesPen;
        }

        /*
         * Deze methode tekent een rechte of een kromme weg. De parameters zijn: sideIn(welke kant de weg binnenkomt), 
         * sideOut(welke kant de weg uitgaat), lanesIn(hoeveel wegen er in gaan bij sideIn),
         * lanesOut(hoeveel wegen er uit gaan bij sideIn). sideIn is altijd het laagste getal, sideOut het hoogste.
         */
        public static Graphics drawRoad(Graphics gr, int lanesIn, int lanesOut, int sideIn, int sideOut)
        {
            Graphics road = gr;
            road.SmoothingMode = SmoothingMode.AntiAlias;
            road.FillRectangle(Brushes.Green, 0, 0, 100, 100);

            int sideTotal = sideIn + sideOut;
            int lanesTotal = lanesIn + lanesOut;
            Pen stripesPen = strPen();

            road.FillRectangle(Brushes.Green, 0, 0, 100, 100);

            //variabelen voor mogelijke wegen
            int r = 50 - roadW * lanesOut;
            int r2 = 50 + roadW * lanesOut;
            int r3 = 50 - roadW * lanesIn;
            int r4 = 50 + roadW * lanesIn;

            //bij 0 is het een rechte weg, anders is het een kromme weg
            if (sideTotal % 2 == 0)
            {
                //bij 4 loopt de weg verticaal, anders horizontaal
                if (sideTotal == 4)
                {
                    road.FillRectangle(Brushes.Gray, r3, 0, lanesTotal * roadW, 100);
                    road.DrawLine(Pens.White, r3, 0, r3, 100);
                    road.DrawLine(Pens.White, r2, 0, r2, 100);

                    //als er maar 1 weg in en 1 weg uit, dan moet er een streepjeslijn in het midden
                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawLine(stripesPen, 50, 0, 50, 100);
                    }
                    else
                    {
                        road.DrawLine(Pens.White, 50, 0, 50, 100);
                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawLine(stripesPen, 50 - t * roadW, 0, 50 - t * roadW, 100);
                        }
                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawLine(stripesPen, 50 + i * roadW, 0, 50 + i * roadW, 100);
                        }
                    }
                }
                //horizontale weg
                else
                {
                    road.FillRectangle(Brushes.Gray, 0, r3, 100, lanesTotal * roadW);
                    road.DrawLine(Pens.White, 0, r3, 100, r3);
                    road.DrawLine(Pens.White, 0, r2, 100, r2);

                    //als er maar 1 weg in en 1 weg uit, dan moet er een streepjeslijn in het midden
                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawLine(stripesPen, 0, 50, 100, 50);
                    }
                    else
                    {
                        road.DrawLine(Pens.White, 0, 50, 100, 50);
                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawLine(stripesPen, 0, 50 - t * roadW, 100, 50 - t * roadW);
                        }
                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawLine(stripesPen, 0, 50 + i * roadW, 100, 50 + i * roadW);
                        }
                    }
                }
            }

            //alle mogelijke kromme wegen
            else
            {
                //bij 3 loopt de weg van kant 1 naar kant 2
                if (sideTotal == 3)
                {
                    road.FillEllipse(Brushes.Gray, r3, -1 * r4, 2 * r4, 2 * r4);
                    road.FillEllipse(Brushes.Green, r2, -1 * r, 2 * r, 2 * r); //groen ipv transparant
                    road.DrawArc(Pens.White, r2, -1 * r, 2 * r, 2 * r, 90, 90);
                    road.DrawArc(Pens.White, r3, -1 * r4, 2 * r4, 2 * r4, 90, 90);

                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawArc(stripesPen, 50, -50, 100, 100, 90, 90);
                    }
                    else
                    {
                        road.DrawArc(Pens.White, 50, -50, 100, 100, 90, 90);
                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawArc(stripesPen, 50 - roadW * t, -1 * 50 - roadW * t, 2 * (50 + roadW * t), 2 * (50 + roadW * t), 90, 90);
                        }

                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawArc(stripesPen, 50 + roadW * i, -1 * 50 + roadW * i, 2 * (50 - roadW * i), 2 * (50 - roadW * i), 90, 90);
                        }
                    }
                }

                //bij 5 en 1 loopt de weg van kant 1 naar kant 4
                else if (sideTotal == 5 && sideIn == 1)
                {
                    road.FillRectangle(Brushes.Green, 0, 0, 100, 100);
                    road.FillEllipse(Brushes.Gray, -1 * r2, -1 * r2, 2 * r2, 2 * r2);
                    road.FillEllipse(Brushes.Green, -1 * r3, -1 * r3, 2 * r3, 2 * r3); //groen ipv transparant
                    road.DrawArc(Pens.White, -1 * r3, -1 * r3, 2 * r3, 2 * r3, 0, 90);
                    road.DrawArc(Pens.White, -1 * r2, -1 * r2, 2 * r2, 2 * r2, 0, 90);

                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawArc(stripesPen, -50, -50, 100, 100, 0, 90);
                    }
                    else
                    {
                        road.DrawArc(Pens.White, -50, -50, 100, 100, 0, 90);

                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawArc(stripesPen, -1 * (50 - roadW * t), -1 * (50 - roadW * t), 2 * (50 - roadW * t), 2 * (50 - roadW * t), 0, 90);
                        }

                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawArc(stripesPen, -1 * (50 + roadW * i), -1 * (50 + roadW * i), 2 * (50 + roadW * i), 2 * (50 + roadW * i), 0, 90);
                        }
                    }
                }

                //bij 5 en 2 loopt de weg van kant 2 naar kant 3
                else if (sideTotal == 5 && sideIn == 2)
                {
                    road.FillEllipse(Brushes.Gray, r3, r3, 2 * r4, 2 * r4);
                    road.FillEllipse(Brushes.Green, r2, r2, 2 * r, 2 * r); //groen ipv transparant
                    road.DrawArc(Pens.White, r2, r2, 2 * r, 2 * r, 180, 90);
                    road.DrawArc(Pens.White, r3, r3, 2 * r4, 2 * r4, 180, 90);

                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawArc(stripesPen, 50, 50, 100, 100, 180, 90);
                    }
                    else
                    {
                        road.DrawArc(Pens.White, 50, 50, 100, 100, 180, 90);

                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawArc(stripesPen, 50 - roadW * t, 50 - roadW * t, 2 * (50 + roadW * t), 2 * (50 + roadW * t), 180, 90);
                        }

                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawArc(stripesPen, 50 + roadW * i, 50 + roadW * i, 2 * (50 - roadW * i), 2 * (50 - roadW * i), 180, 90);
                        }
                    }
                }

                //de weg loopt van kant 3 naar kant 4
                else
                {
                    road.FillEllipse(Brushes.Gray, -1 * r4, r3, 2 * r4, 2 * r4);
                    road.FillEllipse(Brushes.Green, -1 * r, r2, 2 * r, 2 * r); //groen ipv transparant
                    road.DrawArc(Pens.White, -1 * r, r2, 2 * r, 2 * r, 270, 90);
                    road.DrawArc(Pens.White, -1 * r4, r3, 2 * r4, 2 * r4, 270, 90);


                    if (lanesIn == 1 && lanesOut == 1)
                    {
                        road.DrawArc(stripesPen, -50, 50, 100, 100, 270, 90);
                    }
                    else
                    {
                        road.DrawArc(Pens.White, -50, 50, 100, 100, 270, 90);
                        for (int t = 1; t < lanesIn; t++)
                        {
                            road.DrawArc(stripesPen, -1 * (50 + roadW * t), 50 - roadW * t, 2 * (50 + roadW * t), 2 * (50 + roadW * t), 270, 90);
                        }

                        for (int i = 1; i < lanesOut; i++)
                        {
                            road.DrawArc(stripesPen, -1 * (50 - roadW * i), 50 + roadW * i, 2 * (50 - roadW * i), 2 * (50 - roadW * i), 270, 90);
                        }
                    }
                }
            }

            return road;
        }

        //Deze methode tekent een t-splitsing m.b.v. parameters die aangeven hoeveel wegen er in en uit gaan bij elke zijde.
        public static Graphics drawForkroad(Graphics gr, int[] lanes)
        {
            Graphics fork = gr;
            fork.SmoothingMode = SmoothingMode.AntiAlias;
            fork.FillRectangle(Brushes.Green, 0, 0, 100, 100);

            int upIn = lanes[0]; int upOut = lanes[1];
            int rightIn = lanes[2]; int rightOut = lanes[3];
            int downIn = lanes[4]; int downOut = lanes[5];
            int leftIn = lanes[6]; int leftOut = lanes[7];

            /*Er wordt een array aangemaakt met de vier inkomende wegen als elementen. (Elke kant heeft een inkomende en uitgaande weg of geen wegen
             * dus hoeft alleen in- of output gecheckt te worden.) Vervolgens wordt met een forloop gekeken aan welke kant er geen wegen zijn.
             * In count komt het nummer te staan van de kant die geen wegen heeft (1 is up, 2 is right, 3 is down, 4 is left.)
             */
            int[] sides = { upIn, rightIn, downIn, leftIn };
            int count = 0;
            for (int t = 0; t < sides.Length; t++)
            {
                if (sides[t] == 0)
                {
                    count = t + 1;
                    break;
                }
            }

            // Er worden 2 van de 4 booglijnen getekend: lineRU hoort bij de lijn rechtsboven, lineRD bij rechtsonder, lineLD bij linksonder, lineLU bij linksboven
            int lineRUx = 50 + (roadW * upOut);
            int lineRUy = -1 * (50 - (roadW * rightIn));
            int lineRUheight = 2 * (50 - (roadW * rightIn));
            int lineRUwidth = 2 * (50 - (roadW * upOut));

            int lineRDx = 50 + (roadW * downIn);
            int lineRDy = 50 + (roadW * rightOut);
            int lineRDheight = 2 * (50 - (roadW * rightOut));
            int lineRDwidth = 2 * (50 - (roadW * downIn));

            int lineLDx = -1 * (50 - (roadW * downOut));
            int lineLDy = 50 + (roadW * leftIn);
            int lineLDheight = 2 * (50 - (roadW * leftIn));
            int lineLDwidth = 2 * (50 - (roadW * downOut));

            int lineLUx = -1 * (50 - (roadW * upIn));
            int lineLUy = -1 * (50 - (roadW * leftOut));
            int lineLUheight = 2 * (50 - (roadW * leftOut));
            int lineLUwidth = 2 * (50 - (roadW * upIn));

            //Afhankelijk van welke kant geen wegen heeft, worden er 2 bogen en een lijn getekend.
            if (count == 1)
            {
                fork.FillRectangle(Brushes.Gray, 0, 50 - leftOut * roadW, 100, 50 * leftOut * roadW);
                fork.FillEllipse(Brushes.Green, lineLDx, lineLDy, lineLDwidth, lineLDheight);
                fork.FillEllipse(Brushes.Green, lineRDx, lineRDy, lineRDwidth, lineRDheight);
                fork.DrawArc(Pens.White, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
                fork.DrawArc(Pens.White, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawLine(Pens.White, 0, (50 - roadW * leftOut), 100, (50 - roadW * rightIn));
            }
            else if (count == 2)
            {
                fork.FillRectangle(Brushes.Gray, 0, 0, 50 + upOut * roadW, 100);
                fork.FillEllipse(Brushes.Green, lineLDx, lineLDy, lineLDwidth, lineLDheight);
                fork.FillEllipse(Brushes.Green, lineLUx, lineLUy, lineLUwidth, lineLUheight);
                fork.DrawArc(Pens.White, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
                fork.DrawArc(Pens.White, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);
                fork.DrawLine(Pens.White, (50 + roadW * upOut), 0, (50 + roadW * downIn), 100);
            }
            else if (count == 3)
            {
                fork.FillRectangle(Brushes.Gray, 0, 0, 100, 50 + leftIn * roadW);
                fork.FillEllipse(Brushes.Green, lineLUx, lineLUy, lineLUwidth, lineLUheight);
                fork.FillEllipse(Brushes.Green, lineRUx, lineRUy, lineRUwidth, lineRUheight);
                fork.DrawArc(Pens.White, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);
                fork.DrawArc(Pens.White, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
                fork.DrawLine(Pens.White, 0, (50 + roadW * leftIn), 100, (50 + roadW * rightOut));
            }
            else
            {
                fork.FillRectangle(Brushes.Gray, 50 - upIn * roadW, 0, 50 + upIn * roadW, 100);
                fork.FillEllipse(Brushes.Green, lineRUx, lineRUy, lineRUwidth, lineRUheight);
                fork.FillEllipse(Brushes.Green, lineRDx, lineRDy, lineRDwidth, lineRDheight);
                fork.DrawArc(Pens.White, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
                fork.DrawArc(Pens.White, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
                fork.DrawLine(Pens.White, (50 - roadW * upIn), 0, (50 - roadW * downOut), 100);
            }

            return fork;
        }

        //Deze methode tekent het kruispunt m.b.v. parameters die aangeven hoeveel wegen er in en uit gaan bij elke zijde.
        public static Graphics drawCrossroad(Graphics gr, int[] lanes)
        {
            Graphics crossRoad = gr;
            crossRoad.SmoothingMode = SmoothingMode.AntiAlias;
            crossRoad.FillRectangle(Brushes.Green, 0, 0, 100, 100);

            int upIn = lanes[0]; int upOut = lanes[1];
            int rightIn = lanes[2]; int rightOut = lanes[3];
            int downIn = lanes[4]; int downOut = lanes[5];
            int leftIn = lanes[6]; int leftOut = lanes[7];

            // Er worden vier lijnen getekend: lineRU hoort bij de lijn rechtsboven, lineRD bij rechtsonder, lineLD bij linksonder, lineLU bij linksboven
            int lineRUx = 50 + (roadW * upOut);
            int lineRUy = -1 * (50 - (roadW * rightIn));
            int lineRUheight = 2 * (50 - (roadW * rightIn));
            int lineRUwidth = 2 * (50 - (roadW * upOut));

            int lineRDx = 50 + (roadW * downIn);
            int lineRDy = 50 + (roadW * rightOut);
            int lineRDheight = 2 * (50 - (roadW * rightOut));
            int lineRDwidth = 2 * (50 - (roadW * downIn));

            int lineLDx = -1 * (50 - (roadW * downOut));
            int lineLDy = 50 + (roadW * leftIn);
            int lineLDheight = 2 * (50 - (roadW * leftIn));
            int lineLDwidth = 2 * (50 - (roadW * downOut));

            int lineLUx = -1 * (50 - (roadW * upIn));
            int lineLUy = -1 * (50 - (roadW * leftOut));
            int lineLUheight = 2 * (50 - (roadW * leftOut));
            int lineLUwidth = 2 * (50 - (roadW * upIn));

            crossRoad.FillRectangle(Brushes.Gray, 0, 0, 100, 100);
            crossRoad.FillEllipse(Brushes.Green, lineRUx, lineRUy, lineRUwidth, lineRUheight);
            crossRoad.FillEllipse(Brushes.Green, lineRDx, lineRDy, lineRDwidth, lineRDheight);
            crossRoad.FillEllipse(Brushes.Green, lineLDx, lineLDy, lineLDwidth, lineLDheight);
            crossRoad.FillEllipse(Brushes.Green, lineLUx, lineLUy, lineLUwidth, lineLUheight);
            crossRoad.DrawArc(Pens.White, lineRUx, lineRUy, lineRUwidth, lineRUheight, 90, 90);
            crossRoad.DrawArc(Pens.White, lineRDx, lineRDy, lineRDwidth, lineRDheight, 180, 90);
            crossRoad.DrawArc(Pens.White, lineLDx, lineLDy, lineLDwidth, lineLDheight, 270, 90);
            crossRoad.DrawArc(Pens.White, lineLUx, lineLUy, lineLUwidth, lineLUheight, 0, 90);

            return crossRoad;
        }

        public static Graphics drawSpawner(Graphics gr, int side, int lanesIn, int lanesOut)
        {
            Graphics bmSpawner = gr;
            bmSpawner.FillRectangle(Brushes.Green, 0, 0, 100, 100);
            Pen stripesPen = strPen();
            int lanesTotal = lanesIn + lanesOut;
            //bmSpawner.FillRectangle(Brushes.Green, 0, 0, 100, 100);

            //variabelen voor mogelijke wegen
            int r = 50 - roadW * lanesOut;
            int r2 = 50 + roadW * lanesOut;
            int r3 = 50 - roadW * lanesIn;
            int r4 = 50 + roadW * lanesIn;
            int width = roadW * lanesIn + roadW * lanesOut + 10;

            /*Verschillende plaatjes voor verschillende kanten. Spawner is 30 px hoog
             * en 5px breder aan beide kanten van de wegen. De side is waar de
             * spawner staat.
             **/
            if (side == 3)
            {
                bmSpawner.FillRectangle(Brushes.Gray, r3, 0, lanesTotal * roadW, 100);
                bmSpawner.DrawLine(Pens.White, r3, 0, r3, 100);
                bmSpawner.DrawLine(Pens.White, r2, 0, r2, 100);

                //als er maar 1 weg in en 1 weg uit, dan moet er een streepjeslijn in het midden
                if (lanesIn == 1 && lanesOut == 1)
                {
                    bmSpawner.DrawLine(stripesPen, 50, 0, 50, 100);
                }
                else
                {
                    bmSpawner.DrawLine(Pens.White, 50, 0, 50, 100);
                    for (int t = 1; t < lanesIn; t++)
                    {
                        bmSpawner.DrawLine(stripesPen, 50 - t * roadW, 0, 50 - t * roadW, 100);
                    }
                    for (int i = 1; i < lanesOut; i++)
                    {
                        bmSpawner.DrawLine(stripesPen, 50 + i * roadW, 0, 50 + i * roadW, 100);
                    }
                }

                bmSpawner.FillRectangle(Brushes.Black, r3 - 5, 0, width, 30);
            }
            else if (side == 4)
            {
                bmSpawner.FillRectangle(Brushes.Gray, 0, r3, 100, lanesTotal * roadW);
                bmSpawner.DrawLine(Pens.White, 0, r3, 100, r3);
                bmSpawner.DrawLine(Pens.White, 0, r2, 100, r2);

                //als er maar 1 weg in en 1 weg uit, dan moet er een streepjeslijn in het midden
                if (lanesIn == 1 && lanesOut == 1)
                {
                    bmSpawner.DrawLine(stripesPen, 0, 50, 100, 50);
                }
                else
                {
                    bmSpawner.DrawLine(Pens.White, 0, 50, 100, 50);
                    for (int t = 1; t < lanesIn; t++)
                    {
                        bmSpawner.DrawLine(stripesPen, 0, 50 - t * roadW, 100, 50 - t * roadW);
                    }
                    for (int i = 1; i < lanesOut; i++)
                    {
                        bmSpawner.DrawLine(stripesPen, 0, 50 + i * roadW, 100, 50 + i * roadW);
                    }
                }
                bmSpawner.FillRectangle(Brushes.Black, 70, r3 - 5, 30, width);
            }
            else if (side == 1)
            {
                bmSpawner.FillRectangle(Brushes.Gray, r, 0, lanesTotal * roadW, 100);
                bmSpawner.DrawLine(Pens.White, r, 0, r, 100);
                bmSpawner.DrawLine(Pens.White, r4, 0, r4, 100);

                if (lanesIn == 1 && lanesOut == 1)
                {
                    bmSpawner.DrawLine(stripesPen, 50, 0, 50, 100);
                }
                else
                {
                    bmSpawner.DrawLine(Pens.White, 50, 0, 50, 100);
                    for (int t = 1; t < lanesIn; t++)
                    {
                        bmSpawner.DrawLine(stripesPen, 50 + t * roadW, 0, 50 + t * roadW, 100);
                    }
                    for (int i = 1; i < lanesOut; i++)
                    {
                        bmSpawner.DrawLine(stripesPen, 50 - i * roadW, 0, 50 - i * roadW, 100);
                    }
                }

                bmSpawner.FillRectangle(Brushes.Black, r - 5, 70, width, 30);

            }
            else
            {

                bmSpawner.FillRectangle(Brushes.Gray, 0, r, 100, lanesTotal * roadW);
                bmSpawner.DrawLine(Pens.White, 0, r, 100, r);
                bmSpawner.DrawLine(Pens.White, 0, r4, 100, r4);

                //als er maar 1 weg in en 1 weg uit, dan moet er een streepjeslijn in het midden
                if (lanesIn == 1 && lanesOut == 1)
                {
                    bmSpawner.DrawLine(stripesPen, 0, 50, 100, 50);
                }
                else
                {
                    bmSpawner.DrawLine(Pens.White, 0, 50, 100, 50);
                    for (int t = 1; t < lanesIn; t++)
                    {
                        bmSpawner.DrawLine(stripesPen, 0, 50 + t * roadW, 100, 50 + t * roadW);
                    }
                    for (int i = 1; i < lanesOut; i++)
                    {
                        bmSpawner.DrawLine(stripesPen, 0, 50 - i * roadW, 100, 50 - i * roadW);
                    }
                }
                bmSpawner.FillRectangle(Brushes.Black, 0, r - 5, 30, width);
            }

            return bmSpawner;
        }

        public static Graphics drawSelectTile(Graphics gr)
        {
            Graphics selectedTile = gr;           
            Pen selectPen = new Pen(Color.LightBlue, 8);
            gr.DrawRectangle(selectPen, 5,5, 90, 90);
            return selectedTile;
        }

        public static Graphics drawSelectGreenWaveTile(Graphics gr)
        {
            Graphics selectedTile = gr;
            Pen selectPen = new Pen(Color.LightGreen, 8);
            gr.DrawRectangle(selectPen, 5, 5, 90, 90);
            return selectedTile;
        }

        public static Graphics drawRemoveTile(Graphics gr)
        {
            Graphics selectedTile = gr;
            gr.FillRectangle(Brushes.Green, 0, 0, 100, 100);
            return selectedTile;
        }
        public static Graphics drawSpawnerBlock(Graphics gr, int side, int lanesIn, int lanesOut)
        {
            Graphics bmSpawner = gr;
            bmSpawner.FillRectangle(Brushes.Green, 0, 0, 100, 100);
            int lanesTotal = lanesIn + lanesOut;

            //variabelen voor mogelijke wegen
            int r = 50 - roadW * lanesOut;
            int r2 = 50 + roadW * lanesOut;
            int r3 = 50 - roadW * lanesIn;
            int r4 = 50 + roadW * lanesIn;
            int width = roadW * lanesIn + roadW * lanesOut + 10;

            /*Verschillende plaatjes voor verschillende kanten. Spawner is 30 px hoog
             * en 5px breder aan beide kanten van de wegen. De side is waar de
             * spawner staat.
             **/
            if (side == 3)
                bmSpawner.FillRectangle(Brushes.Black, r3 - 5, 0, width, 30);
            else if (side == 4)
                bmSpawner.FillRectangle(Brushes.Black, 70, r3 - 5, 30, width);
            else if (side == 1)
                bmSpawner.FillRectangle(Brushes.Black, r - 5, 70, width, 30);
           else
                bmSpawner.FillRectangle(Brushes.Black, 0, r - 5, 30, width);
            return bmSpawner;
        }
         
    }
}
