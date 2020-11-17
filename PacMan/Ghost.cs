using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vector;
using static PacMan.Variables;

/*
 * TODO : EXCEPTIONS WHEN SOMETHING GOES WRONG
 * TODO : Comments
 * TODO : IA for ghosts
 * TODO : REGIONS
 * 
 * todo : controller
 */
namespace PacMan
{
    public class Ghost : AIRegroupgment, IDisposable
    {
        #region variables
        #region enum
        /// <summary>
        /// Enum for the type of ghost
        /// </summary>
        public enum Type : byte
        {
            YELLOW,
            BLUE,
            PINK,
            RED
        }
        #endregion enum
        public const int SPEED = 10;
        private Map _map;
        private Panel _body;
        private Graphics _ghostGraphics;
        private Graphics _windowGameGraphics;
        private bool _disposed = false;
        private readonly Type _type;
        private GhostAI _AI;
        private Vector2 _deplacementGhost;
        private readonly Color _color;
        #endregion variables

        #region proprieties
        public Panel Body { get => _body; }
        public Vector2 GhostDeplacment
        {
            get => _deplacementGhost;
            set
            {
                _deplacementGhost.X = value.X;
                _deplacementGhost.Y = value.Y; // not getting the pointer, but the value, so no need to clone
            }
        }
        public Map Map { get => _map; }

        public int X { get => _body.Location.X; }
        public int Y { get => _body.Location.Y; }

        public Point Location { get => _body.Location; set => _body.Location = new Point(value.X, value.Y); }
        #endregion proprieties

        #region Ghost construtor
        public Ghost(int x, int y, Type type, Graphics windowGame, Map map)
        {
            this._body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE), 
                BackColor = Color.Black
            };

            this._windowGameGraphics = windowGame;

            this._map = map;

            this._ghostGraphics = _body.CreateGraphics();

            this._deplacementGhost = new Vector2(0, 0);

            switch (type)
            {
                // this everywhere !
                case Type.YELLOW:
                    this._AI = new YellowAI(this);
                    this._color = YellowAI.Color;
                    break;
                case Type.BLUE:
                    this._AI = new BlueAI(this);
                    this._color = BlueAI.Color;
                    break;
                case Type.PINK:
                    this._AI = new PinkAI(this);
                    this._color = PinkAI.Color;
                    break;
                case Type.RED:
                    this._AI = new RedAI(this);
                    this._color = RedAI.Color;
                    break;
                default:
                    this._AI = null;
                    break;
            }

            this._type = type;

            this._body.Paint += CreateGhost;
        }

        private void CreateGhost(object sender, EventArgs e)
        {
            PrintGhostBody();
        }

        private void PrintGhostBody()
        {
            // 25 is rayon of cirle
            // the 7.5 is to offset the cirle in the center
            _ghostGraphics.DrawEllipse(new Pen(_color, 1), 7.5f, 7.5f, 25, 25);

            _ghostGraphics.FillEllipse(new SolidBrush(_color), 7.5f, 7.5f, 25, 25);// yes this is just random number, i could do the math but too heavy for each update

            if (!G_lightMode)
            {
                _ghostGraphics.DrawRectangle(new Pen(_color, 1), 7.5f, 7.5f * 3, 25, 10);

                _ghostGraphics.FillRectangle(new SolidBrush(_color), 7.5f, 7.5f * 3, 25, 10);

                _ghostGraphics.DrawEllipse(new Pen(_color, 1), 7.5f, 6.5f * 4, 10, 10);

                _ghostGraphics.FillEllipse(new SolidBrush(_color), 7.5f, 6.5f * 4, 10, 10);

                _ghostGraphics.DrawEllipse(new Pen(_color, 1), 7.5f * 3, 6.5f * 4, 10, 10);

                _ghostGraphics.FillEllipse(new SolidBrush(_color), 7.5f * 3, 6.5f * 4, 10, 10);
            }
        }
        #endregion Ghost constructor

        #region Ghost update
        public void Move()
        {
            if (_body != null && _AI != null)
            {
                _AI.OnUpdate();
            }
        }
        #endregion Ghost update

        #region Ghost deplacment
        public void SetGhostDeplacement(int futureX, int futureY)
        {
            this._deplacementGhost.X = futureX;
            this._deplacementGhost.Y = futureY;
        }
        #endregion Ghost deplacment

        #region MapUpdate
        public void UpdateMap()
        {
            DrawMap();
            DrawFood();
        }

        private void DrawMap()
        {
            // drawing of the map with delta XY
            // this is pure garbage
            switch (_AI.CurrentDirection)
            {
                case Direction.North:
                    // y + 1
                    //
                    Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE], this.X, this.Y + G_BYTESIZEOFSQUARE - this.Y % G_BYTESIZEOFSQUARE);
                    if (this.CheckIfOnGrid())
                    {
                        Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE + 1, this.X / G_BYTESIZEOFSQUARE], this.X, this.Y + G_BYTESIZEOFSQUARE);
                    }
                    break;
                case Direction.East:
                    // x - 1
                    // 
                    Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE], this.X - this.X % G_BYTESIZEOFSQUARE, this.Y);
                    //
                    if (this.CheckIfOnGrid())
                    {
                        Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE - 1], this.X - G_BYTESIZEOFSQUARE, this.Y);
                    }
                    break;
                case Direction.South:
                    // y - 1
                    //
                    Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE], this.X, this.Y - this.Y % G_BYTESIZEOFSQUARE);
                    //
                    if (this.CheckIfOnGrid())
                    {
                        Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE - 1, this.X / G_BYTESIZEOFSQUARE], this.X, this.Y - G_BYTESIZEOFSQUARE);
                    }
                    break;
                case Direction.West:
                    // x + 1
                    //
                    Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE], this.X + G_BYTESIZEOFSQUARE - this.X % G_BYTESIZEOFSQUARE, this.Y);
                    //
                    if (this.CheckIfOnGrid())
                    {
                        Map.DrawMapRectangle(this._windowGameGraphics, Map.GameMap[this.Y / G_BYTESIZEOFSQUARE, this.X / G_BYTESIZEOFSQUARE + 1], this.X + G_BYTESIZEOFSQUARE, this.Y);
                    }
                    break;
                default:
                    break;
            }
        }

        private void DrawFood()
        {
            int x = 0;
            int y = 0;
            
            // getting the point
            switch (_AI.CurrentDirection)
            {
                case Direction.North:
                    y = G_BYTESIZEOFSQUARE - this.Y % G_BYTESIZEOFSQUARE;
                    break;
                case Direction.East:
                    x = -this.X % G_BYTESIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        x -= G_BYTESIZEOFSQUARE;
                    }
                    break;
                case Direction.South:
                    y = -this.Y % G_BYTESIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        y -= G_BYTESIZEOFSQUARE;
                    }
                    break;
                case Direction.West:
                    x = G_BYTESIZEOFSQUARE - this.X % G_BYTESIZEOFSQUARE;
                    break;
                default:
                    break;
            }

            switch (Map.GameMap[(this.Y + y) / G_BYTESIZEOFSQUARE, (this.X + x) / G_BYTESIZEOFSQUARE])
            {
                case Map.MapMeaning.FOOD:
                    Food.DrawFood(_windowGameGraphics, Food.FoodMeaning.FOOD, (this.X + x), (this.Y + y));
                    break;
                case Map.MapMeaning.BIGFOOD:
                    break;
                case Map.MapMeaning.TELEPORT:
                    // hehe yes
                    break;
                default:
                    break;
            }
        }
        #endregion MapUpdate

        #region GhostMisc
        /// <summary>
        /// Check if on grid
        /// </summary>
        /// <returns>if on grid</returns>
        public bool CheckIfOnGrid()
        {
            return this.Y % G_BYTESIZEOFSQUARE == 0 && this.X % G_BYTESIZEOFSQUARE == 0;
        }

        public void SetGhostLocation(int x, int y)
        {
            this._body.Location = new Point(x, y);
        }
        #endregion GhostMisc

        #region memory managment
        /// <summary>
        /// Dispose
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
                _deplacementGhost.Dispose();
            }

            GC.SuppressFinalize(this);
            _disposed = true;
        }
        #endregion memory managment
    }

    #region Ghost AI
    /// <summary>
    /// Is this bad ? Idk, i never did abstract and override
    /// </summary>
    public abstract class AIRegroupgment : DirectionRelation
    {
        /*
         * YELLOW : He will chase after Pac-Man in Blinky's manner, but will wander off to his home corner when he gets too close.
         * BLUE : Random at each intersection
         * PINK : Scared, go to opposit of youself, but if you are in a radius of you, chase you for 5 sec max
         * RED : Chase you, try to go to the close way possible of you
         */
        protected class YellowAI : GhostAI
        {
            /// <summary>
            /// Attribut
            /// </summary>
            public const int RAYON = 75;

            /// <summary>
            /// Propriety
            /// </summary>
            public static Color Color { get => Color.Yellow; }

            /// <summary>
            /// Custom construcotr
            /// </summary>
            /// <param name="ghost">ghost</param>
            public YellowAI(Ghost ghost) : base(ghost)
            {
                base._rayon = RAYON;
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        }

        protected class BlueAI : GhostAI
        {
            /// <summary>
            /// Attribut
            /// </summary>
            private const int RAYON = 0;
            Random _rnd = new Random();

            /// <summary>
            /// Propriety
            /// </summary>
            public static Color Color { get => Color.MediumBlue; }

            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="ghost">ghost</param>
            public BlueAI(Ghost ghost) : base(ghost)
            {
                base._rayon = RAYON;
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                if (!GhostFutureLocation())
                {
                    FindRandomDirection();
                } 

                base.OnUpdate();
            }

            /// <summary>
            /// Find a random direction 
            /// </summary>
            private void FindRandomDirection()
            {
                List<Direction> directions = base.AvailableDirections();

                if (directions.Count == 1)
                {
                    base._currentDirection = directions[0];
                    base._ghost.GhostDeplacment = DirectionValue[directions[0]];
                }
                else
                {
                    base._currentDirection = directions[_rnd.Next(0, directions.Count)];
                    base._ghost.GhostDeplacment = DirectionValue[base._currentDirection]; 
                    // i made a big mistake, i got the pointer to the object in the dictionary
                    // sometime i want to managme myself the pointers
                }
            }
        }

        public class PinkAI : GhostAI
        {
            /// <summary>
            /// Attribut
            /// </summary>
            private const int RAYON = 150;

            /// <summary>
            /// Propriety
            /// </summary>
            public static Color Color { get => Color.Pink; }

            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="ghost">ghost</param>
            public PinkAI(Ghost ghost) : base(ghost)
            {
                base._rayon = RAYON;
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        }

        protected class RedAI : GhostAI
        {
            /// <summary>
            /// Attribut
            /// </summary>
            private const int RAYON = 0;

            /// <summary>
            /// Propriety
            /// </summary>
            public static Color Color { get => Color.Red; }

            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="ghost">ghost</param>
            public RedAI(Ghost ghost) : base(ghost)
            {
                base._rayon = RAYON;
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        }
    }

    public abstract class GhostAI : DirectionRelation
    {
        /// <summary>
        /// Attribut
        /// </summary>
        protected int _rayon;
        protected Ghost _ghost;
        protected Direction _currentDirection = Direction.East;

        /// <summary>
        /// Propriety
        /// </summary>
        public Direction CurrentDirection => _currentDirection;

        /// <summary>
        /// constructor
        /// </summary>
        public GhostAI(Ghost ghost)
        {
            this._ghost = ghost;
            this._ghost.GhostDeplacment.X = Ghost.SPEED;
            this._ghost.GhostDeplacment.Y = 0;
        }

        /// <summary>
        /// On update function
        /// </summary>
        public virtual void OnUpdate()
        {
            MoveGhost();
        }

        /// <summary>
        /// Move the ghost
        /// </summary>
        protected void MoveGhost()
        {
            _ghost.Location = new Point(_ghost.X + _ghost.GhostDeplacment.X, _ghost.Y + _ghost.GhostDeplacment.Y);
        }

        /// <summary>
        /// Check if the player is to near
        /// </summary>
        /// <returns>if player is to near</returns>
        protected bool IsPlayerToNear()
        {
            // this is basicly pythagor with DeltaX and DeltaY
            // thanks poland
            return Math.Sqrt( Math.Pow(Math.Abs(G_pacMans[0].X - _ghost.X), 2) + Math.Pow(Math.Abs(G_pacMans[0].Y - _ghost.Y), 2)) <= _rayon;
        }

        /// <summary>
        /// the future location of ghost
        /// </summary>
        /// <returns>if ghost can move</returns>
        protected bool GhostFutureLocation()
        {
            if (CheckIfOnGrid())
            {
                if (_ghost.Map.GameMap[_ghost.Y / G_BYTESIZEOFSQUARE + _ghost.GhostDeplacment.Y / Ghost.SPEED, _ghost.X / G_BYTESIZEOFSQUARE + _ghost.GhostDeplacment.X / Ghost.SPEED] == Map.MapMeaning.WALL)
                {
                    this._ghost.GhostDeplacment.X = 0;
                    this._ghost.GhostDeplacment.Y = 0; // bug here
                    return false;
                }
            }

            // no need to update vector
            return true;
        }

        /// <summary>
        /// Check if on grid
        /// </summary>
        /// <returns>if on grid</returns>
        protected bool CheckIfOnGrid()
        {
            return this._ghost.Y % G_BYTESIZEOFSQUARE == 0 && this._ghost.X % G_BYTESIZEOFSQUARE == 0;
        }

        /// <summary>
        /// Find you all the possible directions !
        /// 
        /// !!!!!!!!! EXCLUDE CURRENT DIRECTION !!!!!!!!!
        /// </summary>
        /// <returns>the directions</returns>
        protected List<Direction> AvailableDirections()
        {
            List<Direction> vs = new List<Direction>();

            if (_ghost.Map.GameMap[_ghost.Y / G_BYTESIZEOFSQUARE - 1, _ghost.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.WALL)
            {
                vs.Add(Direction.North);
            }

            if (_ghost.Map.GameMap[_ghost.Y / G_BYTESIZEOFSQUARE, _ghost.X / G_BYTESIZEOFSQUARE + 1] != Map.MapMeaning.WALL)
            {
                vs.Add(Direction.East);
            }

            if (_ghost.Map.GameMap[_ghost.Y / G_BYTESIZEOFSQUARE + 1, _ghost.X / G_BYTESIZEOFSQUARE] != Map.MapMeaning.WALL)
            {
                vs.Add(Direction.South);
            }

            if (_ghost.Map.GameMap[_ghost.Y / G_BYTESIZEOFSQUARE, _ghost.X / G_BYTESIZEOFSQUARE - 1] != Map.MapMeaning.WALL)
            {
                vs.Add(Direction.West);
            }

            // check the PacMan.CheckIfPackManCanMoveWhenRotaded function to see why i do this
            vs.Remove(_currentDirection - 2);
            vs.Remove(_currentDirection + 2);

            return vs;
        }
    }


/************************************************  END OF GHOST AI CLASS  *****************************************************************************************************************/


    /// <summary>
    /// Relation class  
    /// </summary>
    public abstract class DirectionRelation
    {
        /// <summary>
        /// Direction enum
        /// </summary>
        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        /// <summary>
        /// dictionary to where to go
        /// </summary>
        private static readonly Dictionary<Direction, Vector2> directionValue = new Dictionary<Direction, Vector2>(4)
        {
            {Direction.North, new Vector2(0, -Ghost.SPEED ) },
            {Direction.East, new Vector2(Ghost.SPEED, 0 ) },
            {Direction.South, new Vector2(0, Ghost.SPEED ) },
            {Direction.West, new Vector2(-Ghost.SPEED, 0 ) }
        };

        protected static Dictionary<Direction, Vector2> DirectionValue => directionValue;
    }
    #endregion Ghost AI
}
