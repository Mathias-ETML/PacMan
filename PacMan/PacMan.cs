using System;
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
 * TODO : EXCEPTIONS WHEN SOMETHING GOES WRONG
 * TODO : OPTIMISATION OF CHECKING WHEN PACMAN MOVE 
 * TODO : EVENT HANDLER TAKING MEMORY NOT GOOD OH HELL NO
 * TODO : ANIMATION
 * TODO : OPTIMISE FOR BAD GRAPHICAL COMPUTER ( like intel with intergrated chips )
 * PS : FIX THEM MATHAIS PLEASE
 */ 

namespace PacMan
{
    /// <summary>
    /// PacMan class
    /// </summary>
    public partial class PacMan : IDisposable
    {
        #region variables
        /// <summary>
        /// Attributs
        /// </summary>
        private Panel _body;
        private Map _map;
        private Graphics _pacManGraphics;
        private int _pacManAnimationInterval = G_BYTETIMEBETWENGAMETICK * 2;
        private Timer _onAnimationUpdate;
        private Color _pacManBodyColor = Color.Yellow;
        private Mouth.Position _lastAuthorizedDirection = Mouth.Position.North;
        private Mouth.Position _actualMouthDirection = Mouth.Position.North;
        private bool _disposed = false;
        private bool _isPacManMouthOpen = true;
#pragma warning disable CS0414
        private bool _canPacManEatGhost = false;
#pragma warning restore CS0414
        private ulong _playerScore = 0;
        private static readonly sbyte _speedOfPacMan = 10; // in px
        private Vector2 _deplacementPacMan;// = new Vector2(0, 0);
        #endregion variables

        #region propriety
        /// <summary>
        /// Propriety
        /// </summary>
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

        public Mouth.Position ActualMouthDirection { get =>_actualMouthDirection; }
        public Mouth.Position LastAutorizedDirection { get => _lastAuthorizedDirection; }

        public ulong PlayerScore { get => _playerScore; }
        private void AddPlayerPoints(int points)
        {
            _playerScore += (ulong)points;
        }
        #endregion propriety

        #region PacMan code
        #region custom constructor
        /// <summary>
        /// Custom contructor
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        public PacMan(int x, int y, Map map)
        {
            _body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE),
                BackColor = Color.Black
            };

            // getting the map
            this._map = map;

            // get the grpahics of the panel
            this._pacManGraphics = _body.CreateGraphics();

            // it work only because it's a event handler, you can't just use DrawPacManBody, you need the event handler
            this._body.Paint += CreatePacMan;

            //Body.BackgroundImage = Properties.Resources.pacman;

            // create a vector for the mouvment of the pacman body
            this._deplacementPacMan = new Vector2(0, 0);

            StartPacManAnimation();
        }
        #endregion custom construtor

        #region PacMan graphics
        /// <summary>
        /// Create the pacman body, to user with event handler
        /// </summary>
        /// <param name="sender">for this user, a panel</param>
        /// <param name="e">the graphics</param>
        private void CreatePacMan(object sender, PaintEventArgs e)
        {
            DrawPacManBody();

            DrawPacManMouth(_lastAuthorizedDirection);
        }

        /// <summary>
        /// Draw the body of the PacMan, to user with the create
        /// </summary>
        private void DrawPacManBody()
        {
            _pacManGraphics.DrawEllipse(new Pen(_pacManBodyColor, 1), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);

            _pacManGraphics.FillEllipse(new SolidBrush(_pacManBodyColor), 0, 0, G_BYTESIZEOFSQUARE - 1, G_BYTESIZEOFSQUARE - 1);
        }

        /// <summary>
        /// Draw the pacman mouth in the North, East, South, West
        /// </summary>
        /// <param name="position">the direction</param>
        private void DrawPacManMouth(Mouth.Position position)
        {
            _pacManGraphics.DrawPolygon(new Pen(Color.Black, 1), Mouth.relation[position]);

            _pacManGraphics.FillPolygon(new SolidBrush(Color.Black), Mouth.relation[position]);
        }

        #region PacMan animation
        /// <summary>
        /// Create the timer for the pacman animation
        /// </summary>
        public void StartPacManAnimation()
        {
            if (_onAnimationUpdate == null)
            {
                _onAnimationUpdate = new Timer()
                {
                    Interval = _pacManAnimationInterval
                };

                // add the method to the timer
                _onAnimationUpdate.Tick += PacManAnimation;

                // start the timer
                _onAnimationUpdate.Start();
            }
        }

        /// <summary>
        /// Animate the pacman with the mouth
        /// </summary>
        /// <param name="sender">the timer</param>
        /// <param name="e"></param>
        private void PacManAnimation(object sender, EventArgs e)
        {
            if (_body != null)
            {
                if (_isPacManMouthOpen)
                {
                    DrawPacManBody();
                    _isPacManMouthOpen = false;
                }
                else
                {
                    DrawPacManMouth(_actualMouthDirection);
                    _isPacManMouthOpen = true;
                }
            }
        }
        #endregion PacMan animation
        #endregion PacMan graphics

        #region PacMan update
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
                    case Map.MapMeaning.TELEPORT:

                        // here what we do is we check if it's on the grid, so we are sure that the pacman is on a teleporation case
                        if (CheckIfOnGrid())
                        {
                            TeleportPacMan();

                            return true;
                        }
                        break;
                    default:
                        break;
                }

                // get the future location of the pacman
                int[] tab_intFutureLocation = CheckIfPacManCanMove();

                // eating food
                Eat();

                // and setting the pacman location
                SetPacManLocation(tab_intFutureLocation[0], tab_intFutureLocation[1]);
            }

            return false;
        }

        #region PacMan body update
        /// <summary>
        /// Change the mouth direction
        /// TODO : WITH DEGREE
        /// </summary>
        /// <param name="position"> in what position do you want the packman mout to be </param>
        public void RotatePacManBody(Mouth.Position direction)
        {
            DrawPacManBody();

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
        #endregion PacMan body update
        #endregion PacMan update

        #region PacMan direction update
        /// <summary>
        /// check if the pacman can move in the direcion he rotaded
        /// </summary>
        /// <param name="nextDirection">the futre direction of the pacman</param>
        /// <returns></returns>
        private bool CheckIfPackManCanMoveWhenRotaded(Mouth.Position nextDirection)
        {
            // check if it's the opposit
            if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
            {
                return true;
            }

            // get the location, not memory frienly but processor friendly because you do it once
            byte xPosition = (byte)((PacManLocation.X / G_BYTESIZEOFSQUARE));
            byte yPosition = (byte)((PacManLocation.Y / G_BYTESIZEOFSQUARE));
            sbyte xFuturePosition = (sbyte)(_deplacementPacMan.X / _speedOfPacMan);
            sbyte yFuturePosition = (sbyte)(_deplacementPacMan.Y / _speedOfPacMan);

            // check if the future location is not a wall
            if (CheckIfOnGrid() && (_map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL))
            {
                _lastAuthorizedDirection = nextDirection;
                return true;
            }
            
            if (_lastAuthorizedDirection + 2 == nextDirection)
            {
                // check if its the opposit direction or the same as the pacman was
                if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
                {
                    return true;
                }

                // check if the case in the front of the future location is not a wall
                if (_map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL &&
                    _map.GameMap[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != Map.MapMeaning.WALL)
                {
                    _lastAuthorizedDirection = nextDirection;
                    return true;
                }
            }

            // check if its the opposit
            else if (_lastAuthorizedDirection - 2 == nextDirection)
            {
                // check if its the opposit direction or the same as the pacman was
                if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
                {
                    return true;
                }

                // check if the case in the front of the future location is not a wall
                if (_map.GameMap[yPosition + yFuturePosition, xPosition + xFuturePosition] != Map.MapMeaning.WALL &&
                    _map.GameMap[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != Map.MapMeaning.WALL)
                {
                    _lastAuthorizedDirection = nextDirection;
                    return true;
                }
            }
            
            return false;
        }
        #endregion PacMan direction update;

        #region PacMan map update
        /// <summary>
        /// Update the graphics of the map
        /// </summary>
        /// <param name="sender">in our case, the panel of the map</param>
        /// <param name="e"></param>
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
                    Map.DrawMapRectangle(graphics, _map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE) + 1, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y + G_BYTESIZEOFSQUARE);
                    break;

                case Mouth.Position.East:

                    // TODO : explain why
                    if (_map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1] != Map.MapMeaning.WALL && _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1] != Map.MapMeaning.TELEPORT)
                    {
                        Map.DrawMapRectangle(graphics, _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE) - 1], PacManLocation.X - G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    }
                    else
                    {
                        Map.DrawMapRectangle(graphics, _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X - PacManLocation.X % G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    }
                    return;

                case Mouth.Position.South:

                    // TODO : explain why
                    if (_map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE) - 1, (PacManLocation.X / G_BYTESIZEOFSQUARE)] != Map.MapMeaning.WALL)
                    {
                        Map.DrawMapRectangle(graphics, _map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE) - 1, (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y - G_BYTESIZEOFSQUARE);
                    }
                    else
                    {
                        Map.DrawMapRectangle(graphics, _map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y - PacManLocation.Y % G_BYTESIZEOFSQUARE);
                    }

                    return;

                case Mouth.Position.West:

                    // TODO : explain why
                    Map.DrawMapRectangle(graphics, _map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE) + 1], PacManLocation.X + G_BYTESIZEOFSQUARE, PacManLocation.Y);
                    break;
                default:
                    break;
            }

            // update the actual case of the map
            Map.DrawMapRectangle(graphics, _map.GameMap[(PacManLocation.Y / G_BYTESIZEOFSQUARE), (PacManLocation.X / G_BYTESIZEOFSQUARE)], PacManLocation.X, PacManLocation.Y);
        }
        #endregion PacMan map update

        #region PacMan teleportation
        /// <summary>
        /// Update the teleportation case
        /// </summary>
        /// <param name="sender">in our case, the panel of the map</param>
        /// <param name="e"></param>
        public void UpdateTeleportation(object sender, PaintEventArgs e)
        {
            // get the original point with some magic code
            Point buffer = TeleportRelation.WhereToTeleportPacMan.FirstOrDefault(x => x.Value == PacManLocation).Key;

            // redraw the teleportation pad on the original point
            e.Graphics.FillRectangle(new SolidBrush(Map.MapDictionary[_map.GameMap[buffer.Y / G_BYTESIZEOFSQUARE, buffer.X / G_BYTESIZEOFSQUARE]]), 

            // create a new rectangle
            new Rectangle(buffer.X, buffer.Y, G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE));
        }
        
        /// <summary>
        /// Teleport the pacman in a relation with the teleportation pad
        /// </summary>
        public void TeleportPacMan()
        {
            Point buffer = TeleportRelation.WhereToTeleportPacMan[PacManLocation];
            SetPacManLocation(buffer.X, buffer.Y);
        }
        #endregion PacMan teleportation

        #region PacMan miscellaneous
        /// <summary>
        /// check if the pacman is on the grid
        /// </summary>
        /// <returns>a boolean if the pacman is on te grid</returns>
        public bool CheckIfOnGrid()
        {
            return PacManLocation.Y % G_BYTESIZEOFSQUARE == 0 && PacManLocation.X % G_BYTESIZEOFSQUARE == 0;
        }

        /// <summary>
        /// Check if the pacman future location is not a wall
        /// </summary>
        /// <returns>return the future location</returns>
        private int[] CheckIfPacManCanMove()
        {
            // check if on the grid
            if (CheckIfOnGrid())
            {
                // check if the block in front of him is a wall
                if (_map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE + _deplacementPacMan.Y / _speedOfPacMan, PacManLocation.X / G_BYTESIZEOFSQUARE + _deplacementPacMan.X / _speedOfPacMan] == Map.MapMeaning.WALL)
                {
                    return new int[2] { PacManLocation.X, PacManLocation.Y };
                }
            }

            // if not on the grid the pacman will not hit a wall
            return new int[2] { PacManLocation.X + _deplacementPacMan.X, PacManLocation.Y + _deplacementPacMan.Y };
        }

        /// <summary>
        /// Get you on wich case the pacman is
        /// </summary>
        /// <returns>on wich case the pacman is</returns>
        private Map.MapMeaning OnWichCaseIsPacMan()
        {
            return _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE];
        }

        /// <summary>
        /// Eat food
        /// </summary>
        public void Eat()
        {
            // check if on grid, else there will be no food
            if (CheckIfOnGrid())
            {
                // check if there is food on the map
                if (_map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.ROAD && _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.TELEPORT)
                {
                    // adding point
                    AddPlayerPoints(Food.Points.PointForFoods[(Food.FoodMeaning)_map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE]]);

                    // removing the food
                    _map.FoodsMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE].Dispose();

                    // chanching the type of case
                    _map.GameMap[PacManLocation.Y / G_BYTESIZEOFSQUARE, PacManLocation.X / G_BYTESIZEOFSQUARE] = Map.MapMeaning.ROAD;
                }
            }
        }
        #endregion PacMan miscellaneous

        #region memory managment
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

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">if you want to dispose object in the class</param>
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
        #endregion memory managment
        #endregion PacMan code

        #region PacMan classes
        #region PacMan teleportation
        /// <summary>
        /// Class for the teleportation relation
        /// </summary>
        internal static class TeleportRelation
        {
            // work with XY
            // point to where to teleport the pacman
            private static Point FirstLocation = new Point(40, 360);
            private static Point FirstLocationEnd = new Point(640, 360);
            private static Point SecondLocation = new Point(680, 360);
            private static Point SecondLocationEnd = new Point(80, 360);

            // get you on wich location to teleport pacman
            public static readonly Dictionary<Point, Point> WhereToTeleportPacMan = new Dictionary<Point, Point>(2)
            {
                {FirstLocation, FirstLocationEnd },
                {SecondLocation, SecondLocationEnd}
            };
        }
        #endregion PacMan teleportation

        #region PacMan mouth
        /// <summary>
        /// The class for the mouth of the pacman
        /// wich is basicaly a triangle
        /// </summary>
        public static class Mouth
        {
            #region enum
            /// <summary>
            /// Enum for the direction
            /// </summary>
            public enum Position
            {
                North,
                East,
                South,
                West
            }
            #endregion enum

            #region mouth position
            // Points for the mouht for the north
            private static Point[] North = new Point[3]
            {
                new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, 0),
                new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, 0),
                new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            // Points for the mouht for the South
            private static Point[] South = new Point[3]
            {
                new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
                new Point(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
                new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            // Points for the mouht for the East
            private static Point[] East = new Point[3]
            {
                new Point(G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
                new Point(G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4, G_BYTESIZEOFSQUARE),
                new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };

            // Points for the mouht for the West
            private static Point[] West = new Point[3]
            {
                new Point(0, G_BYTESIZEOFSQUARE / 2 - G_BYTESIZEOFSQUARE / 4),
                new Point(0, G_BYTESIZEOFSQUARE / 2 + G_BYTESIZEOFSQUARE / 4),
                new Point(G_BYTESIZEOFSQUARE / 2, G_BYTESIZEOFSQUARE / 2)
            };
            #endregion mouth position

            #region mouth dictionary
            /// <summary>
            /// Dictionnary for what mouth to draw with wich direction
            /// </summary>
            public static readonly Dictionary<Position, Point[]> relation = new Dictionary<Position, Point[]>(4)
            {
                {Position.North, North },
                {Position.East, South },
                {Position.South, East },
                {Position.West, West }
            };
            #endregion mouth dictionary
        }
        #endregion PacMan mouth
        #endregion PacMan classes
    }
}
