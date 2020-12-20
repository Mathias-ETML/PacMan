using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Vector.Vector2;
using PacManGame.Interfaces.IEntityNS;
using PacManGame.Map;
using PacManGame.GameView;
using PacManGame.Interfaces.IControllerNS;
using static PacManGame.Misc.Variables;

// TODO : GHOST EATING


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

namespace PacManGame.Entities
{
    /// <summary>
    /// PacMan class
    /// </summary>
    public class PacMan : Entity
    {
        #region variables
        /// <summary>
        /// Attributs
        /// </summary>
        private Panel _body;
        private GameMap _map;
        private Graphics _pacManGraphics;
        private Graphics _windowGameGraphics;
        private int _pacManAnimationInterval = global::PacManGame.Controllers.GameControllerNS.GameController.TIMETOUPDATE * 2;
        private Timer _onAnimationUpdate;
        private Timer _endGhostEating;
        private const int _GHOSTEATINGTIME = 7000; // in ms
        private readonly Color _pacManBodyColor = Color.Yellow;
        private Direction _lastAuthorizedDirection = Direction.North;
        private Direction _currentDirection = Direction.North;
        private bool _disposed = false;
        private bool _isPacManMouthOpen = true;
        private bool _canPacManEatGhost = false;
        private ulong _playerScore = 0;
        public const int SPEEDOFPACMAN = 10; // in px
        private Vector2 _deplacementPacMan;// = new Vector2(0, 0);
        private ObjectContainer _objectContainer;
        #endregion variables

        #region propriety
        /// <summary>
        /// Propriety
        /// </summary>
        public override Panel Body { get => _body; set => _body = value; }

        private Point PacManLocation
        {
            get => _body.Location;
            set => _body.Location = new Point(value.X, value.Y);
        }

        public Point GetPacManLocation { get => PacManLocation; }
        public void SetPacManLocation(int x, int y)
        {
            PacManLocation = new Point(x, y);
        }

        public static int SpeedOfPacMan { get => SPEEDOFPACMAN; }
        public override Vector2 EntityVector2
        {
            get => _deplacementPacMan;
            set
            {
                this._deplacementPacMan.X = value.X;
                this._deplacementPacMan.Y = value.Y;
            }
        }

        public override Direction CurrentDirection { get => _currentDirection; set => _currentDirection = value; }
        public Direction LastAutorizedDirection { get => _lastAuthorizedDirection; }

        public ulong PlayerScore { get => _playerScore; }
        private void AddPlayerPoints(int points)
        {
            _playerScore += (ulong)points;
        }

        public int X { get => _body.Location.X; }
        public int Y { get => _body.Location.Y; }

        public override ObjectContainer ObjectContainer { get => _objectContainer; set => _objectContainer = value; }
        #endregion propriety

        #region PacMan code
        #region custom constructor
        /// <summary>
        /// Custom contructor
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        public PacMan(int x, int y, ObjectContainer objectContainer)
        {
            this._body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE),
                BackColor = Color.Black
            };

            this.ObjectContainer = objectContainer;

            // getting the pointer for the graphics of the panel
            this._windowGameGraphics = ObjectContainer.GameFormPanelGraphics;

            // getting the map
            this._map = ObjectContainer.Map;

            // get the grpahics of the panel
            this._pacManGraphics = _body.CreateGraphics();

            OnStart();
        }

        public override void OnStart()
        {
            // it work only because it's a event handler, you can't just use DrawPacManBody, you need the event handler
            // maybe the memory problem is here
            this._body.Paint += CreatePacMan;

            //Body.BackgroundImage = Properties.Resources.pacman;

            // create a vector for the mouvment of the pacman body
            this._deplacementPacMan = new Vector2(0, 0);

            if (!G_lightMode)
            {
                StartPacManAnimation();
            }
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
            _pacManGraphics.DrawEllipse(new Pen(_pacManBodyColor, 1), 0, 0, GameForm.SIZEOFSQUARE - 1, GameForm.SIZEOFSQUARE - 1);

            _pacManGraphics.FillEllipse(new SolidBrush(_pacManBodyColor), 0, 0, GameForm.SIZEOFSQUARE - 1, GameForm.SIZEOFSQUARE - 1);
        }

        /// <summary>
        /// Draw the pacman mouth in the North, East, South, West
        /// </summary>
        /// <param name="position">the direction</param>
        private void DrawPacManMouth(Direction position)
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
                    DrawPacManMouth(_currentDirection);
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
        public override void OnUpdate()
        {
            if (_body != null && !_disposed)
            {
                switch (base.OnWichCaseIsEntity())
                {
                    case GameMap.MapMeaning.TELEPORT:

                        // here what we do is we check if it's on the grid, so we are sure that the pacman is on a teleporation case
                        if (CheckIfOnGrid())
                        {
                            base.TeleportEntity();
                        }
                        break;

                    case GameMap.MapMeaning.BIGFOOD:
                    case GameMap.MapMeaning.FOOD:
                        // eating food
                        Eat();
                        break;
                    
                    default:
                        break;
                }

                // get the future location of the pacman
                PacManFutureLocation();
            }

            //this.UpdateMap();
        }

        #region PacMan body update
        /// <summary>
        /// Change the mouth direction
        /// TODO : WITH DEGREE
        /// </summary>
        /// <param name="position"> in what position do you want the packman mout to be </param>
        public void RotatePacManBody(Direction direction)
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

            _currentDirection = direction;
        }
        #endregion PacMan body update
        #endregion PacMan update

        #region PacMan direction update
        /// <summary>
        /// check if the pacman can move in the direcion he rotaded
        /// </summary>
        /// <param name="nextDirection">the futre direction of the pacman</param>
        /// <returns></returns>
        private bool CheckIfPackManCanMoveWhenRotaded(Direction nextDirection)
        {
            // check if it's the opposit
            if (_lastAuthorizedDirection == nextDirection || _lastAuthorizedDirection == nextDirection - 2 || _lastAuthorizedDirection == nextDirection + 2)
            {
                return true;
            }

            // get the location, not memory frienly but processor friendly because you do it once
            // casting heavy
            int xPosition = PacManLocation.X / GameForm.SIZEOFSQUARE;
            int yPosition = PacManLocation.Y / GameForm.SIZEOFSQUARE;
            int xFuturePosition = _deplacementPacMan.X / SPEEDOFPACMAN;
            int yFuturePosition = _deplacementPacMan.Y / SPEEDOFPACMAN;

            // check if the future location is not a wall
            if (CheckIfOnGrid() && (_map.GameMapMeaning[yPosition + yFuturePosition, xPosition + xFuturePosition] != GameMap.MapMeaning.WALL))
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
                if (_map.GameMapMeaning[yPosition + yFuturePosition, xPosition + xFuturePosition] != GameMap.MapMeaning.WALL &&
                    _map.GameMapMeaning[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != GameMap.MapMeaning.WALL)
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
                if (_map.GameMapMeaning[yPosition + yFuturePosition, xPosition + xFuturePosition] != GameMap.MapMeaning.WALL &&
                    _map.GameMapMeaning[yPosition + yFuturePosition + xFuturePosition, xPosition + xFuturePosition + yFuturePosition] != GameMap.MapMeaning.WALL)
                {
                    _lastAuthorizedDirection = nextDirection;
                    return true;
                }
            }
            
            return false;
        }
        #endregion PacMan direction update;

        #region PacMan miscellaneous


        /// <summary>
        /// Check if the pacman future location is not a wall
        /// </summary>
        /// <returns>return the future location</returns>
        private void PacManFutureLocation()
        {
            // check if on the grid
            if (CheckIfOnGrid())
            {
                // check if the block in front of him is a wall
                if (_map.GameMapMeaning[PacManLocation.Y / GameForm.SIZEOFSQUARE + _deplacementPacMan.Y / SPEEDOFPACMAN, PacManLocation.X / GameForm.SIZEOFSQUARE + _deplacementPacMan.X / SPEEDOFPACMAN] == GameMap.MapMeaning.WALL)
                {
                    this._deplacementPacMan.X = 0;
                    this._deplacementPacMan.Y = 0;
                    return;
                }
            }

            // if not on the grid the pacman will not hit a wall
            SetPacManLocation(PacManLocation.X + _deplacementPacMan.X, PacManLocation.Y + _deplacementPacMan.Y);
        }

        /// <summary>
        /// Eat food
        /// </summary>
        public void Eat()
        {
            // check if on grid, else there will be no food
            if (CheckIfOnGrid())
            {
                // adding point
                AddPlayerPoints(Food.Points.PointForFoods[(Food.FoodMeaning)_map.GameMapMeaning[PacManLocation.Y / GameForm.SIZEOFSQUARE, PacManLocation.X / GameForm.SIZEOFSQUARE]]);

                // removing the food
                _map.FoodsMap[PacManLocation.Y / GameForm.SIZEOFSQUARE, PacManLocation.X / GameForm.SIZEOFSQUARE].Dispose();

                // chanching the type of case
                _map.GameMapMeaning[PacManLocation.Y / GameForm.SIZEOFSQUARE, PacManLocation.X / GameForm.SIZEOFSQUARE] = GameMap.MapMeaning.ROAD;
            }
        }

        /// <summary>
        /// Start of invicibility
        /// </summary>
        private void StartGhostEating()
        {
            this._canPacManEatGhost = true;
            this._endGhostEating = new Timer
            {
                Interval = _GHOSTEATINGTIME
            };
            this._endGhostEating.Tick += StopGhostEating;
        }

        /// <summary>
        /// end of invicibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopGhostEating(object sender, EventArgs e)
        {
            this._canPacManEatGhost = false;
            ((Timer)sender).Dispose();
        }

        public void EatGhost()
        {
            if (_canPacManEatGhost)
            {

            }
            else
            {
                // he die
            }
        }

        public void SetPacManDeplacement(int x, int y)
        {
            _deplacementPacMan.X = x;
            _deplacementPacMan.Y = y;
        }
        #endregion PacMan miscellaneous

        #region memory managment
        /// <summary>
        /// Disposing
        /// </summary>
        public new void Dispose()
        {
            if (!_disposed)
            {
                this.Dispose(true);
            }

            GC.SuppressFinalize(this);

        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">if you want to dispose object in the class</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _body.Dispose();
                _onAnimationUpdate.Dispose();
                _endGhostEating.Dispose();
                _deplacementPacMan.Dispose();
            }

            base.Dispose(disposing);

            _disposed = true;
        }

        public override void Spawn()
        {
            throw new NotImplementedException();
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }
        #endregion memory managment
        #endregion PacMan code

        #region PacMan classes
        #region PacMan teleportation

        #endregion PacMan teleportation

        #region PacMan mouth
        /// <summary>
        /// The class for the mouth of the pacman
        /// wich is basicaly a triangle
        /// </summary>
        public static class Mouth
        {
            #region mouth position
            // Points for the mouht for the north
            private static Point[] North = new Point[3]
            {
                new Point(GameForm.SIZEOFSQUARE / 2 - GameForm.SIZEOFSQUARE / 4, 0),
                new Point(GameForm.SIZEOFSQUARE / 2 + GameForm.SIZEOFSQUARE / 4, 0),
                new Point(GameForm.SIZEOFSQUARE / 2, GameForm.SIZEOFSQUARE / 2)
            };

            // Points for the mouht for the South
            private static Point[] South = new Point[3]
            {
                new Point(GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE / 2 - GameForm.SIZEOFSQUARE / 4),
                new Point(GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE / 2 + GameForm.SIZEOFSQUARE / 4),
                new Point(GameForm.SIZEOFSQUARE / 2, GameForm.SIZEOFSQUARE / 2)
            };

            // Points for the mouht for the East
            private static Point[] East = new Point[3]
            {
                new Point(GameForm.SIZEOFSQUARE / 2 - GameForm.SIZEOFSQUARE / 4, GameForm.SIZEOFSQUARE),
                new Point(GameForm.SIZEOFSQUARE / 2 + GameForm.SIZEOFSQUARE / 4, GameForm.SIZEOFSQUARE),
                new Point(GameForm.SIZEOFSQUARE / 2, GameForm.SIZEOFSQUARE / 2)
            };

            // Points for the mouht for the West
            private static Point[] West = new Point[3]
            {
                new Point(0, GameForm.SIZEOFSQUARE / 2 - GameForm.SIZEOFSQUARE / 4),
                new Point(0, GameForm.SIZEOFSQUARE / 2 + GameForm.SIZEOFSQUARE / 4),
                new Point(GameForm.SIZEOFSQUARE / 2, GameForm.SIZEOFSQUARE / 2)
            };
            #endregion mouth position

            #region mouth dictionary
            /// <summary>
            /// Dictionnary for what mouth to draw with wich direction
            /// </summary>
            public static readonly Dictionary<Direction, Point[]> relation = new Dictionary<Direction, Point[]>(4)
            {
                {Direction.North, North },
                {Direction.East, South },
                {Direction.South, East },
                {Direction.West, West }
            };
            #endregion mouth dictionary
        }
        #endregion PacMan mouth
        #endregion PacMan classes
    }
}
