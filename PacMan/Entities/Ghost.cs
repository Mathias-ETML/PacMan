using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vector.Vector2;
using PacManGame.Interfaces.IEntityNS;
using PacManGame.Map;
using PacManGame.GameView;
using PacManGame.Interfaces.IControllerNS;
using static PacManGame.Misc.Variables;

/*
 * TODO : EXCEPTIONS WHEN SOMETHING GOES WRONG
 * TODO : Comments
 * TODO : IA for ghosts
 * TODO : REGIONS
 * 
 */
namespace PacManGame.Entities
{
    public class Ghost : Entity
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
        private ObjectContainer _objectContainer;
        private GameMap _map;
        private Panel _body;
        private Graphics _ghostGraphics;
        private readonly Graphics _windowGameGraphics;
        private bool _disposed = false;
        private readonly Type _type;
        private GhostAI _AI;
        private Vector2 _deplacementGhost;
        private readonly Color _color;
        private Direction _currentDirection = Direction.East;
        #endregion variables

        #region proprieties
        public override Panel Body { get => _body; set => _body = value; }
        public override Vector2 EntityVector2
        {
            get => _deplacementGhost;
            set
            {
                _deplacementGhost.X = value.X;
                _deplacementGhost.Y = value.Y; // not getting the pointer, but the value, so no need to clone
            }
        }
        public GameMap Map { get => _map; }

        public override ObjectContainer ObjectContainer { get => _objectContainer; set => _objectContainer = value; }

        public int X { get => _body.Location.X; }
        public int Y { get => _body.Location.Y; }

        public Point Location { get => _body.Location; set => _body.Location = new Point(value.X, value.Y); }

        public override Direction CurrentDirection { get => _currentDirection; set => _currentDirection = value; }
        #endregion proprieties

        #region Ghost construtor
        public Ghost(int x, int y, Type type, ObjectContainer objectContainer)
        {
            this._body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE), 
                BackColor = Color.Black
            };

            this.ObjectContainer = objectContainer;

            this._windowGameGraphics = ObjectContainer.GameFormPanelGraphics;

            this._map = ObjectContainer.Map;

            this._ghostGraphics = _body.CreateGraphics();

            this._deplacementGhost = new Vector2(0, 0);

            switch (type)
            {
                // this everywhere !
                case Type.YELLOW:
                    this._AI = new AIRegroupgment.YellowAI(this);
                    this._color = AIRegroupgment.YellowAI.Color;
                    break;
                case Type.BLUE:
                    this._AI = new AIRegroupgment.BlueAI(this);
                    this._color = AIRegroupgment.BlueAI.Color;
                    break;
                case Type.PINK:
                    this._AI = new AIRegroupgment.PinkAI(this);
                    this._color = AIRegroupgment.PinkAI.Color;
                    break;
                case Type.RED:
                    this._AI = new AIRegroupgment.RedAI(this, this._map);
                    this._color = AIRegroupgment.RedAI.Color;
                    break;
                default:
                    this._AI = null;
                    break;
            }

            this._type = type;

            OnStart();
        }

        public override void OnStart()
        {
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
        public override void OnUpdate()
        {
            if (_body != null && _AI != null)
            {
                switch (this.OnWichCaseIsEntity())
                {
                    case GameMap.MapMeaning.TELEPORT:

                        if (base.CheckIfOnGrid())
                        {
                            base.TeleportEntity();
                            return;
                        }
                        break;

                    default:
                        break;
                }

                _AI.OnUpdate();
                //this.UpdateMap();
            }
        }


        #endregion Ghost update

        #region MapUpdate
        public new void OnUpdateMap()
        {
            base.OnUpdateMap();
            DrawFood();
        }

        private void DrawFood()
        {
            int x = 0;
            int y = 0;
            
            // getting the point
            switch (_currentDirection)
            {
                case Direction.North:
                    y = GameForm.SIZEOFSQUARE - this.Y % GameForm.SIZEOFSQUARE;
                    break;
                case Direction.East:
                    x = -this.X % GameForm.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        x -= GameForm.SIZEOFSQUARE;
                    }
                    break;
                case Direction.South:
                    y = -this.Y % GameForm.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        y -= GameForm.SIZEOFSQUARE;
                    }
                    break;
                case Direction.West:
                    x = GameForm.SIZEOFSQUARE - this.X % GameForm.SIZEOFSQUARE;
                    break;
                default:
                    break;
            }

            switch (GetGameMapMeaning(x, y))
            {
                case GameMap.MapMeaning.FOOD:
                    Food.DrawFood(_windowGameGraphics, Food.FoodMeaning.FOOD, (this.X + x), (this.Y + y));
                    break;
                case GameMap.MapMeaning.BIGFOOD:
                    Food.DrawFood(_windowGameGraphics, Food.FoodMeaning.BIGFOOD, (this.X + x), (this.Y + y));
                    break;
                default:
                    break;
            }
        }
        #endregion MapUpdate

        #region GhostMisc

        public void SetGhostLocation(int x, int y)
        {
            this._body.Location = new Point(x, y);
        }
        #endregion GhostMisc

        #region memory managment
        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            if (!_disposed)
            {
                this.Dispose(true);
            }

            GC.SuppressFinalize(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _body.Dispose();
                _deplacementGhost.Dispose();
                _AI.Dispose();
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
    }

    #region Ghost AI
    /// <summary>
    /// Is this bad ? Idk, i never did abstract and override
    /// </summary>
    public abstract class AIRegroupgment
    {
        /*
         * YELLOW : He will chase after Pac-Man in Blinky's manner, but will wander off to his home corner when he gets too close.
         * BLUE : Random at each intersection
         * PINK : Scared, go to opposit of youself, but if you are in a radius of you, chase you for 5 sec max
         * RED : Chase you, try to go to the close way possible of you
         */
        public class YellowAI : GhostAI
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

            public override void OnStart()
            {
                base.OnStart();
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        }

        public class BlueAI : GhostAI
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
                OnStart();
            }

            public override void OnStart()
            {
                base.OnStart();
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                if (base._ghost.CheckIfOnGrid())
                {
                    NeedVectorUpdate();
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
                    if (directions[0] == base.CurrentDirection)
                    {
                        return;
                    }

                    base.CurrentDirection = directions[0];
                    base._ghost.EntityVector2 = DirectionsValues[directions[0]];
                }
                else
                {
                    base.CurrentDirection = directions[_rnd.Next(0, directions.Count)];
                    base._ghost.EntityVector2 = DirectionsValues[base.CurrentDirection]; 
                    // i made a big mistake, i got the pointer to the object in the dictionary
                    // sometime i want to manage myself the pointers
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

            public override void OnStart()
            {
                base.OnStart();
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                base.OnUpdate();
            }
        }

        public class RedAI : GhostAI
        {
            /// <summary>
            /// Attribut
            /// </summary>
            private const int RAYON = 0;
            private GameMap _map;

            /// <summary>
            /// Propriety
            /// </summary>
            public static Color Color { get => Color.Red; }

            /// <summary>
            /// Custom constructor
            /// </summary>
            /// <param name="ghost">ghost</param>
            public RedAI(Ghost ghost, GameMap map) : base(ghost)
            {
                base._rayon = RAYON;
                this._map = map;
                OnStart();
            }

            public override void OnStart()
            {
                base.OnStart();
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate()
            {
                if (_ghost.CheckIfOnGrid())
                {
                    if (base.AvailableDirections().Count == 1)
                    {
                        base.CurrentDirection = AvailableDirections()[0];
                        base._ghost.EntityVector2 = DirectionsValues[CurrentDirection];
                    }
                    else
                    {
                        ChasePacMan();
                    }

                    /*
                    if (base.NeedVectorUpdate())
                    {
                        ChasePacMan();
                    }
                    else if (base.AvailableDirections().Count > 1)
                    {
                        ChasePacMan();
                    }*/
                }

                base.OnUpdate();
            }

            private void ChasePacMan()
            {
                base.CurrentDirection = BestDirectionToChoose();
                base._ghost.EntityVector2 = DirectionsValues[CurrentDirection];
            }

            private Direction BestDirectionToChoose()
            {
                Direction direction;
                int deltaX = ObjectContainer.PacMans[0].X - _ghost.X;
                int deltaY = ObjectContainer.PacMans[0].Y - _ghost.Y;

                System.Diagnostics.Debug.WriteLine($"deltaX : {deltaX} / deltaY : {deltaY}");
                System.Diagnostics.Debug.WriteLine(ObjectContainer.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE + 1] != GameMap.MapMeaning.WALL);
                System.Diagnostics.Debug.WriteLine(ObjectContainer.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE - 1] != GameMap.MapMeaning.WALL);

                // this is machin learning level thing :sunglass:
                if (deltaX >= deltaY && base._ghost.GetGameMapMeaning(0, GameForm.SIZEOFSQUARE) != GameMap.MapMeaning.WALL 
                    && base._ghost.GetGameMapMeaning(GameForm.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL || deltaY == 0)
                {
                    if (deltaX >= 0)
                    {
                        direction = Direction.East;
                    }
                    else
                    {
                        direction = Direction.West;
                    }
                }
                else
                {
                    if (deltaY >= 0)
                    {
                        direction = Direction.South;
                    }
                    else
                    {
                        direction = Direction.North;
                    }
                }

                /*
                if (deltaX > 0 && ((int)CurrentDirection % 2 == 0))
                {
                    // move right
                    direction = Direction.East;
                }
                else if (((int)CurrentDirection % 2 == 0))
                {
                    // move left
                    direction = Direction.West;
                }
                else if (deltaY > 0)
                {
                    // move down
                    direction = Direction.South;
                }
                else
                {
                    // move up
                    direction = Direction.North;
                }*/ 

                return direction;
            }
            
            private void FindDirection()
            {
                List<Direction> directions = base.AvailableDirections();
                base.CurrentDirection = directions[0];
                base._ghost.EntityVector2 = DirectionsValues[directions[0]];
            }
        }
    }

    public abstract class GhostAI : EntityDirection
    {
        /// <summary>
        /// Attribut
        /// </summary>
        protected int _rayon;
        protected Ghost _ghost;
        protected ObjectContainer _objectContainer;

        /// <summary>
        /// Propriety
        /// </summary>
        public Direction CurrentDirection { get => _ghost.CurrentDirection; set => _ghost.CurrentDirection = value; }

        public ObjectContainer ObjectContainer { get => _ghost.ObjectContainer; set => _objectContainer = value; }
        /// <summary>
        /// constructor
        /// </summary>
        public GhostAI(Ghost ghost)
        {
            this._ghost = ghost;
            OnStart();
        }

        public virtual void OnStart()
        {
            this._ghost.EntityVector2.X = Ghost.SPEED;
            this._ghost.EntityVector2.Y = 0;
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
            _ghost.Location = new Point(_ghost.X + _ghost.EntityVector2.X, _ghost.Y + _ghost.EntityVector2.Y); // wierd bug, when a ghost move, it erase the last graphic.fillRectangle, in 
        }

        /// <summary>
        /// Check if the player is to near
        /// </summary>
        /// <returns>if player is to near</returns>
        protected bool IsPlayerToNear()
        {
            // this is basicly pythagor with DeltaX and DeltaY
            // thanks poland
            return Math.Sqrt( Math.Pow(Math.Abs(ObjectContainer.PacMans[0].X - _ghost.X), 2) + Math.Pow(Math.Abs(ObjectContainer.PacMans[0].Y - _ghost.Y), 2)) <= _rayon;
        }

        /// <summary>
        /// the future location of ghost
        /// </summary>
        /// <returns>if ghost can move</returns>
        protected bool NeedVectorUpdate()
        {
            if (_ghost.GetGameMapMeaning(_ghost.EntityVector2.X / Ghost.SPEED, _ghost.EntityVector2.Y / Ghost.SPEED) == GameMap.MapMeaning.WALL)
            //if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE + _ghost.EntityVector2.Y / Ghost.SPEED, _ghost.X / GameForm.SIZEOFSQUARE + _ghost.EntityVector2.X / Ghost.SPEED] == GameMap.MapMeaning.WALL)
            {
                this._ghost.EntityVector2.X = 0;
                this._ghost.EntityVector2.Y = 0;

                return true;
            }

            // no need to update vector
            return false;
        }

        /// <summary>
        /// Find you all the possible directions !
        /// 
        /// !!!!!!!!! EXCLUDE OPPOSIT OF CURRENT DIRECTION !!!!!!!!!
        /// </summary>
        /// <returns>the directions</returns>
        protected List<Direction> AvailableDirections()
        {
            List<Direction> vs = new List<Direction>();

            /*
            if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE - 1, _ghost.X / GameForm.SIZEOFSQUARE] != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.North);
            }*/

            if (_ghost.GetGameMapMeaning(0, -GameForm.SIZEOFSQUARE) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.North);
            }

            /*
            if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE + 1] != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.East);
            }*/

            if (_ghost.GetGameMapMeaning(GameForm.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.East);
            }

            /*
            if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE + 1, _ghost.X / GameForm.SIZEOFSQUARE] != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.South);
            }*/

            if (_ghost.GetGameMapMeaning(0, GameForm.SIZEOFSQUARE) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.South);
            }
            
            /*
            if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE - 1] != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.West);
            }*/

            if (_ghost.GetGameMapMeaning(-GameForm.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.West);
            }

            // check the PacMan.CheckIfPackManCanMoveWhenRotaded function to see why i do this
            vs.Remove(CurrentDirection - 2);
            vs.Remove(CurrentDirection + 2);

            return vs;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
    #endregion Ghost AI
}
