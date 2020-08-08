using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class PacMan
    {
        public Panel Body { get; }
        public static readonly byte SpeedOfPacMan = 10; // in px
        private Timer timer_PlayerAnimation { get; set; }

        public PacMan(int x, int y)
        {
            Body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE),
                BackColor = Color.Black
            };

            Body.Paint += Body_Paint;
            //Body.Paint += Test;

            //StartPacManAnimation();
        }
        
        /*
        private void Test(object sender, PaintEventArgs e)
        {
            Graphics graphics = Body.CreateGraphics();

            graphics.DrawEllipse(new Pen(Color.Yellow, 1), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);

            graphics.DrawPolygon(new Pen(Color.Black, 1), Mouth.South);

            graphics.FillPolygon(new SolidBrush(Color.Black), Mouth.South);
        }
        */

        private void Body_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = Body.CreateGraphics();

            // todo : graphic design for pacman

            Rectangle rectangle = new Rectangle(0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);

            graphics.DrawEllipse(new Pen(Color.Yellow, 1), rectangle);

            graphics.FillEllipse(new SolidBrush(Color.Yellow), rectangle);

            graphics.DrawPolygon(new Pen(Color.Black, 1), Mouth.North);

            graphics.FillPolygon(new SolidBrush(Color.Black), Mouth.North);
        }

        public void StartPacManAnimation()
        {
            if (timer_PlayerAnimation == null)
            {
                timer_PlayerAnimation = new Timer()
                {
                    Interval = G_BYTETIMEBETWENGAMETICK,
                };

                timer_PlayerAnimation.Tick += PacManAnimation;

                timer_PlayerAnimation.Start();
            }
        }

        public void StopPacManAnimation()
        {
            if (timer_PlayerAnimation != null)
            {
                timer_PlayerAnimation.Stop();
                timer_PlayerAnimation.Dispose();
            }
        }

        private bool boolIsPacManMouthOpen { get; set; } = false;

        private void PacManAnimation(object sender, EventArgs e)
        {
            if (Body != null)
            {
                
            }
        }

        /// <summary>
        /// Work clock-wise
        /// </summary>
        /// <param name="position"> in what position do you want the packman mout to be </param>
        public void RotatePacManBody(byte position)
        {
            // todo : rotate the packman mouth
            // first erase mouth by color it in yellow
            // then redraw it in black in other position


            //graphics.DrawPolygon(new Pen(Color.Yellow, 1), Mouth.relation[Mouth.position]);

            //graphics.FillPolygon(new SolidBrush(Color.Yellow), Mouth.position);
        }

        // int x etc.. where to move
        public int intPacManPosX
        {
            get => Body.Location.X;
            set
            {
                if (Body != null)
                {
                    Body.Location = new Point(value, Body.Location.Y);
                }

            }
        }

        public int intPacManPosY
        {
            get => Body.Location.Y;
            set
            {
                if (Body != null)
                {
                    Body.Location = new Point(value, Body.Location.Y);
                }

            }
        }

        public sbyte intPacManMovementX { get; set; } = 0;
        public sbyte intPacManMovementY { get; set; } = 0;

        public void Move()
        {
            if (Body != null)
            {
                int[] tab_intFutureLocation = new int[2];

                tab_intFutureLocation = CheckIfPacManCanMove();

                Eat();

                Body.Location = new Point(tab_intFutureLocation[0], tab_intFutureLocation[1]);
            }
        }

        private int[] CheckIfPacManCanMove()
        {
            int x = intPacManPosX + intPacManMovementX;
            int y = intPacManPosY + intPacManMovementY;

            // check if on the grid
            if (intPacManPosY % G_BYTESIZEOFSQUARE == 0 && intPacManPosX % G_BYTESIZEOFSQUARE == 0)
            {
                // check if the block in front of him is a wall
                if (Map.GameMap[intPacManPosY / G_BYTESIZEOFSQUARE + intPacManMovementY / SpeedOfPacMan, intPacManPosX / G_BYTESIZEOFSQUARE + intPacManMovementX / SpeedOfPacMan] == Map.MapMeaning.WALL)
                {
                    x = intPacManPosX;
                    y = intPacManPosY;
                }
            }
            /*else if((Map.GameMap[((intPacManPosY - (intPacManPosY % G_BYTESIZEOFSQUARE)) / G_BYTESIZEOFSQUARE) + (intPacManMovementY / SpeedOfPacMan), ((intPacManPosX - (intPacManPosX % G_BYTESIZEOFSQUARE)) / G_BYTESIZEOFSQUARE) + (intPacManMovementX / SpeedOfPacMan)] == Map.MapMeaning.WALL))
            {
                intPacManMovementX = 0;
                intPacManMovementY = 0;

                x = intPacManPosX;
                y = intPacManPosY;
            }*/

            return new int[2] { x, y };
        }

        public void Die()
        {
            if (Body != null && this != null)
            {
                StopPacManAnimation();
                Body.Dispose();
                ((IDisposable)this).Dispose();
            }
            else if (this != null)
            {
                ((IDisposable)this).Dispose();
            }
        }

        public bool boolCanPacManEatGhost { get; set; } = false;

        public void Eat()
        {
            if (intPacManPosX % G_BYTESIZEOFSQUARE == 0 && intPacManPosY % G_BYTESIZEOFSQUARE == 0)
            {
                if (Map.GameMap[intPacManPosY / G_BYTESIZEOFSQUARE, intPacManPosX / G_BYTESIZEOFSQUARE] != Map.MapMeaning.ROAD)
                {
                    AddPlayerPoints(FoodType.FoodRelation[(Food.FoodMeaning)Map.GameMap[intPacManPosY / G_BYTESIZEOFSQUARE, intPacManPosX / G_BYTESIZEOFSQUARE]]);
                    Map.GameMap[intPacManPosY / G_BYTESIZEOFSQUARE, intPacManPosX / G_BYTESIZEOFSQUARE] = Map.MapMeaning.ROAD;
                }
            }
        }

        private ulong U64PlayerScore { get; set; } = 0;

        public ulong PlayerScore { get => U64PlayerScore; }

        private void AddPlayerPoints(int points)
        {
            U64PlayerScore += (ulong)points;
        }
    }

    public static class Mouth
    {
        public enum Position
        {
            North,
            South,
            East,
            West
        }

        public static Position position = Position.North;
        
        public static Point[] North = new Point[3]
        {
            new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, 0),
            new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, 0),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
        };

        public static Point[] South = new Point[3]
        {
            new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
        };

        public static Point[] East = new Point[3]
        {
            new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
            new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
        };

        public static Point[] West = new Point[3]
        {
            new Point(0, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
            new Point(0, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
        };

        public static Dictionary<Position, Point[]> relation = new Dictionary<Position, Point[]>(4)
        {
            {Position.North, North.ToArray() },
            {Position.South, South.ToArray() },
            {Position.East, East.ToArray() },
            {Position.West, West.ToArray() }
        };
    }
}
