﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.IO;
using static PacMan.Variables;

// todo : finish this game before 2021
// todo : master in potato recolting
// todo : don't go crazy, don't go stupid
/*
 * 
 * 
 *  
 * TODO : EVENT HANDLER TAKING MEMORY NOT GOOD OH HELL NO
 * PS : FIX THEM MATHAIS PLEASE
 */ 

namespace PacMan
{
    public partial class PacMan : IDisposable
    {
        private Panel _body;
        
        private Timer _onAnimationUpdate;
        private Mouth.Position _lastAuthorizedDirection = Mouth.Position.North;
        private Mouth.Position _actualMouthDirection = Mouth.Position.North;
        private bool _disposed = false;
        private Graphics _pacManGraphics;
        //private bool _boolIsPacManMouthOpen = false;
        private bool _boolCanPacManEatGhost = false;
        private ulong _playerScore { get; set; } = 0;

        private static readonly sbyte _speedOfPacMan = 10; // in px
        private Vector2 _deplacementPacMan;// = new Vector2(0, 0);


        public Panel Body { get => _body; }

        private Point PacManLocation
        {
            get => _body.Location;
            set => _body.Location = new Point(value.X, value.Y);
        }

        public Point GetPacManLocation => PacManLocation;

        public void SetPacManLocation(int x, int y)
        {
            PacManLocation = new Point(x, y);
        }

        public static sbyte SpeedOfPacMan { get => _speedOfPacMan; }

        public Vector2 GetDeplacementPacMan { get => _deplacementPacMan; }

        public void SetPacManDeplacement(int x, int y)
        {
            _deplacementPacMan.X = x;
            _deplacementPacMan.Y = y;
        }

        public Mouth.Position ActualMouthDirection => _actualMouthDirection;

        public ulong PlayerScore { get => _playerScore; }

        private void AddPlayerPoints(int points)
        {
            _playerScore += (ulong)points;
        }

        public PacMan(int x, int y)
        {
            _body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE),
                BackColor = Color.Black
            };

            _pacManGraphics = _body.CreateGraphics();

            // IT WORK BECAUSE ITS A EVENT HANDLER BUT NOT WITH GLOBAL GRAPHICS, WHY PLEASE EXPLAIN
            _body.Paint += CreatePacMan;

            _deplacementPacMan = new Vector2(0, 0);

            //StartPacManAnimation();
        }

        private void CreatePacMan(object sender, PaintEventArgs e)
        {
            DrawPacMan_body();

            DrawPacManMouth(_lastAuthorizedDirection);
        }

        public void StartPacManAnimation()
        {
            if (_onAnimationUpdate == null)
            {
                _onAnimationUpdate = new Timer()
                {
                    Interval = G_BYTETIMEBETWENGAMETICK
                };

                _onAnimationUpdate.Tick += PacManAnimation;

                _onAnimationUpdate.Start();
            }
        }

        private void PacManAnimation(object sender, EventArgs e)
        {
            if (_body != null)
            {
                
            }
        }

        private void DrawPacMan_body()
        {
            _pacManGraphics.DrawEllipse(new Pen(Color.Yellow, 1), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);

            _pacManGraphics.FillEllipse(new SolidBrush(Color.Yellow), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);
        }

        private void DrawPacManMouth(Mouth.Position position)
        {
            _pacManGraphics.DrawPolygon(new Pen(Color.Black, 1), Mouth.relation[position]);

            _pacManGraphics.FillPolygon(new SolidBrush(Color.Black), Mouth.relation[position]);
        }

        /// <summary>
        /// Change the mouth direction
        /// TODO : WITH DEGREE
        /// </summary>
        /// <param name="position"> in what position do you want the packman mout to be </param>
        public void RotatePacMan_body(Mouth.Position direction)
        {
            DrawPacMan_body();

            DrawPacManMouth(direction);

            if (!CheckIfPackManCanMoveWhenRotaded(direction))
            {
                _deplacementPacMan.X = 0;
                _deplacementPacMan.Y = 0;
            }
            else
            {
                _lastAuthorizedDirection = direction;
            }

            _actualMouthDirection = direction;
        }

        private bool CheckIfPackManCanMoveWhenRotaded(Mouth.Position nextDirection)
        {
            byte xPosition = (byte)((PacManLocation.X / G_BYTESIZEOFSQUARE));
            byte yPosition = (byte)((PacManLocation.Y / G_BYTESIZEOFSQUARE));
            sbyte xFuturePosition = (sbyte)(_deplacementPacMan.X / _speedOfPacMan);
            sbyte yFuturePosition = (sbyte)(_deplacementPacMan.Y / _speedOfPacMan);

            if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
            {
                return true;
            }

            if (CheckIfOnGrid() && (Map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL))
            {
                _lastAuthorizedDirection = nextDirection;
                return true;
            }

            if (_lastAuthorizedDirection + 2 == nextDirection)
            {
                if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
                {
                    return true;
                }

                if (Map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL &&
                Map.GameMap[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != Map.MapMeaning.WALL)
                {
                    _lastAuthorizedDirection = nextDirection;
                    return true;
                }
            }
            else if (_lastAuthorizedDirection - 2 == nextDirection)
            {
                if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
                {
                    return true;
                }

                if (Map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL &&
                    Map.GameMap[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != Map.MapMeaning.WALL)
                {
                    _lastAuthorizedDirection = nextDirection;
                    return true;
                }
            }
            
            return false;
        }

        public bool CheckIfOnGrid()
        {
            return PacManLocation.Y % G_BYTESIZEOFSQUARE == 0 && PacManLocation.X % G_BYTESIZEOFSQUARE == 0;
        }



        /// <summary>
        /// Update the pacman location
        /// </summary>
        /// <returns> return if need to update the teleportation zone </returns>
        public bool Move()
        {
            if (_body != null && !_disposed)
            {
                switch (OnWichCaseIsPacMan())
                {
                    /*
                    case Map.MapMeaning.EMPTY:
                        break;
                    case Map.MapMeaning.WALL:
                        break;
                    case Map.MapMeaning.ROAD:
                        break;
                    case Map.MapMeaning.FOOD:
                        break;
                    case Map.MapMeaning.BIGFOOD:
                        break;
                        */
                    case Map.MapMeaning.TELEPORT:
                        if (CheckIfOnGrid())
                        {
                            // TODO : UPDATE MAP QUAND PACMAN SE TP, comment ? aucune idée
                            // enfaite j'ai trouvé
                            TeleportPacMan();

                            return true;
                        }
                        break;
                    default:
                        break;
                }

                int[] tab_intFutureLocation = CheckIfPacManCanMove();

                Eat();

                SetPacManLocation(tab_intFutureLocation[0], tab_intFutureLocation[1]);

            }

            return false;
        }

        private int[] CheckIfPacManCanMove()
        {
            // check if on the grid
            if (CheckIfOnGrid())
            {
                // check if the block in front of him is a wall
                if (Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE + _deplacementPacMan.Y / _speedOfPacMan, PacManLocation.X / G_BYTESIZEOFSQUARE + _deplacementPacMan.X / _speedOfPacMan] == Map.MapMeaning.WALL)
                {
                    return new int[2] { PacManLocation.X, PacManLocation.Y };
                }
            }

            return new int[2] { PacManLocation.X + _deplacementPacMan.X, PacManLocation.Y + _deplacementPacMan.Y };
        }

        public void UpdateMap(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            // TODO : NEED TO RE-DRAW THE FOOD WHEN WE EAT IT HALF WAY
            // TODO : NEED TO RE-DRAW THE FOOD WHEN WE EAT IT HALF WAY
            // TODO : NEED TO RE-DRAW THE FOOD WHEN WE EAT IT HALF WAY
            switch (_lastAuthorizedDirection)
            {
                case Mouth.Position.North:

                    // TODO : explain why
                    Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE) + 1, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y + G_BYTESIZEOFSQUARE);
                    break;

                case Mouth.Position.East:

                    // TODO : explain why
                    //
                    //
                    //
                    //


                    //Map.DrawMapRectangle(graphics, Map.MapMeaning.DEBUG, PacManLocation.X - G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    //Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X - G_BYTESIZEOFSQUARE, PacManLocation.Y);

                    if (Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1] != Map.MapMeaning.WALL && Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1] != Map.MapMeaning.TELEPORT)
                    {
                        // Debug.WriteLine($"checking {PacManLocation.Y / G_BYTESIZEOFSQUARE} {(PacManLocation.X / G_BYTESIZEOFSQUARE)}");
                        Map.DrawMapRectangle(graphics, Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1], PacManLocation.X - G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    }
                    else
                    {
                        // Debug.WriteLine($"checking {PacManLocation.Y / G_BYTESIZEOFSQUARE} {(PacManLocation.X / G_BYTESIZEOFSQUARE)}");
                        //Map.DrawMapRectangle(graphics, Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y);
                        Map.DrawMapRectangle(graphics, Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X - PacManLocation.X % G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    }
                    return;

                case Mouth.Position.South:

                    // TODO : explain why
                    //
                    //
                    //
                    //
                    Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y - G_BYTESIZEOFSQUARE);
                    Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE) - 1, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X - G_BYTESIZEOFSQUARE, PacManLocation.Y);

                    return;

                case Mouth.Position.West:

                    // TODO : explain why
                    Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE) + 1], PacManLocation.X + G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    break;
                default:
                    break;
            }

            Map.DrawMapRectangle(graphics, Map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y);
            //Map.DrawMapRectangle(graphics, Map.MapMeaning.DEBUG, 0, 0);

        }

        public void UpdateTeleportation(object sender, PaintEventArgs e)
        {
            Point buffer = TeleportRelation.WhereToTeleportPacMan.FirstOrDefault(x => x.Value == PacManLocation).Key;

            e.Graphics.FillRectangle(new SolidBrush(Map.MapDictionary[Map.GameMap[buffer.Y / G_BYTESIZEOFSQUARE, buffer.X / G_BYTESIZEOFSQUARE]]), new Rectangle(buffer.X, buffer.Y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE));

            //e.Graphics.DrawRectangle(new Pen(Map.MapDictionary[Map.MapMeaning.DEBUG], 5), item);

            //e.Graphics.DrawRectangle(new Pen(Map.MapDictionary[Map.GameMap[item.Location.Y / G_BYTESIZEOFSQUARE, item.Location.X / G_BYTESIZEOFSQUARE]], 5), item);


            //Map.MapDictionary[Map.GameMap[item.Location.Y / G_BYTESIZEOFSQUARE, item.Location.X / G_BYTESIZEOFSQUARE]];
            
        }
        

        public void TeleportPacMan()
        {
            Point buffer = TeleportRelation.WhereToTeleportPacMan[PacManLocation];
            SetPacManLocation(buffer.X, buffer.Y);
        }
            
        private Map.MapMeaning OnWichCaseIsPacMan()
        {
            return Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE];
        }

        

        public void Eat()
        {
            if (CheckIfOnGrid())
            {
                if (Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.ROAD && Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.TELEPORT)
                {
                    AddPlayerPoints(FoodRelation.PointForFoods[(Food.FoodMeaning)Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE]]);
                    Map.FoodMap.tab_foods[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE].Dispose();
                    Map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] = Map.MapMeaning.ROAD;
                }
            }
        }



        /// <summary>
        /// Disposing
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _body.Dispose();
                _onAnimationUpdate.Dispose();
            }

            GC.SuppressFinalize(this);

            _disposed = true;
        }

        internal static class TeleportRelation
        {
            // work with XY
            private static Point FirstLocation = new Point(40, 360);
            private static Point FirstLocationEnd = new Point(640, 360);
            private static Point SecondLocation = new Point(680, 360);
            private static Point SecondLocationEnd = new Point(80, 360);

            // work with XY
            public static readonly Dictionary<Point, Point> WhereToTeleportPacMan = new Dictionary<Point, Point>(2)
            {
                {FirstLocation, FirstLocationEnd },
                {SecondLocation, SecondLocationEnd}
            };
        }

        public static class Mouth
        {
            public enum Position
            {
                North,
                East,
                South,
                West
            }

            private static Point[] North = new Point[3]
            {
            new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, 0),
            new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, 0),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            private static Point[] South = new Point[3]
            {
            new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            private static Point[] East = new Point[3]
            {
            new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
            new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            private static Point[] West = new Point[3]
            {
            new Point(0, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
            new Point(0, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
            new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            public static readonly Dictionary<Position, Point[]> relation = new Dictionary<Position, Point[]>(4)
            {
                {Position.North, North },
                {Position.East, South },
                {Position.South, East },
                {Position.West, West }
            };
        }
    }
}
