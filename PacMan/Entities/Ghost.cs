﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vector.Vector2;
using PacManGame.Interfaces;
using PacManGame.Map;
using PacManGame.GameView;
using static PacManGame.Misc.Variables;
using PacManGame.Controllers;

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
        public delegate void OnGhostDeathEventHandler(Ghost ghost);

        public event OnGhostDeathEventHandler GhostDeathEvent;

        protected virtual void RaiseDeathEvent()
        {
            GhostDeathEvent?.Invoke(this);
        }

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
        private const int _GHOSTLIFE = 1;
        private IMapContainer _objectContainer;
        private GameMap _map;
        private Panel _body;
        private Graphics _ghostGraphics;
        private readonly Graphics _windowGameGraphics;
        private bool _disposed = false;
        private readonly Type _type;
        private GhostAI _AI;
        private Vector2 _vector2Ghost;
        private readonly Color _color;
        private Direction _currentDirection = Direction.East;
        private int _ghostLife;
        #endregion variables

        #region proprieties
        public override Panel Body { get => _body; set => _body = value; }
        public override Vector2 Vector2Ghost
        {
            get => _vector2Ghost;
            set
            {
                _vector2Ghost.X = value.X;
                _vector2Ghost.Y = value.Y; // not getting the pointer, but the value, so no need to clone
            }
        }
        public GameMap Map { get => _map; }

        public override IMapContainer MapContainer { get => _objectContainer; set => _objectContainer = value; }

        public override int X { get => base.X; }
        public override int Y { get => base.Y; }

        public Point Location { get => _body.Location; set => _body.Location = new Point(value.X, value.Y); }

        public override Direction CurrentDirection { get => _currentDirection; set => _currentDirection = value; }
        #endregion proprieties

        #region Ghost construtor
        public Ghost(int x, int y, Type type, GameContainer objectContainer)
        {
            this.MapContainer = objectContainer;

            this._windowGameGraphics = MapContainer.GameFormPanelGraphics;

            this._map = MapContainer.Map;

            this._type = type;

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

            this._ghostLife = _GHOSTLIFE;

            Spawn(x, y);
        }

        public override void Spawn(int x, int y)
        {
            this._vector2Ghost = new Vector2(0, 0);

            this._body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(GameMap.SIZEOFSQUARE, GameMap.SIZEOFSQUARE),
                BackColor = Color.Black
            };

            this._ghostGraphics = _body.CreateGraphics();

            this._body.Paint += CreateGhost;

            MapContainer.GameForm.panPanGame.Controls.Add(this.Body);
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
        public override void OnUpdate(IEntityContainer entityContainer)
        {
            if (_body != null && _AI != null)
            {
                CheckIfEntityOverlap(entityContainer);

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

                _AI.OnUpdate(entityContainer);
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
                    y = GameMap.SIZEOFSQUARE - this.Y % GameMap.SIZEOFSQUARE;
                    break;
                case Direction.East:
                    x = -this.X % GameMap.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        x -= GameMap.SIZEOFSQUARE;
                    }
                    break;
                case Direction.South:
                    y = -this.Y % GameMap.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        y -= GameMap.SIZEOFSQUARE;
                    }
                    break;
                case Direction.West:
                    x = GameMap.SIZEOFSQUARE - this.X % GameMap.SIZEOFSQUARE;
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

        public override bool IsAlive()
        {
            return this._ghostLife >= 0;
        }

        public override bool Die()
        {
            this._ghostLife--;
            return IsAlive();
        }

        public override void Dispawn()
        {
            base.Dispawn();
        }

        public void RaiseDeath()
        {
            this.RaiseDeathEvent();
        }
        #endregion GhostMisc

        #region memory managment

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _body.Dispose();
                    _vector2Ghost.Dispose();
                    _AI.Dispose();
                    base.Dispose(true);
                }

                this.Dispawn();
                this.OnUpdateMap();
            }

            _disposed = true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate(IEntityContainer entityContainer)
            {
                base.OnUpdate(entityContainer);
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
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate(IEntityContainer entityContainer)
            {
                if (base._ghost.CheckIfOnGrid())
                {
                    NeedVectorUpdate();
                    FindRandomDirection();
                }

                base.OnUpdate(entityContainer);
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
                    base._ghost.Vector2Ghost = DirectionsValues[directions[0]];
                }
                else
                {
                    base.CurrentDirection = directions[_rnd.Next(0, directions.Count)];
                    base._ghost.Vector2Ghost = DirectionsValues[base.CurrentDirection]; 
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

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate(IEntityContainer entityContainer)
            {
                base.OnUpdate(entityContainer);
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
            }

            /// <summary>
            /// On update function
            /// </summary>
            public override void OnUpdate(IEntityContainer entityContainer)
            {
                if (_ghost.CheckIfOnGrid())
                {
                    // is ghost stuck ?
                    if (base.AvailableDirections().Count == 1)
                    {
                        base.CurrentDirection = AvailableDirections()[0];
                        base._ghost.Vector2Ghost = DirectionsValues[CurrentDirection];
                    }
                    else
                    {
                        ChasePacMan(entityContainer);
                    }
                }

                base.OnUpdate(entityContainer);
            }

            private void ChasePacMan(IEntityContainer entityContainer)
            {
                base.CurrentDirection = BestDirectionToChoose(entityContainer);
                base._ghost.Vector2Ghost = DirectionsValues[CurrentDirection];
            }

            private PacMan ClosestPacMan(IEntityContainer entityContainer)
            {
                return null;
            }

            private Direction BestDirectionToChoose(IEntityContainer entityContainer)
            {
                Direction direction;
                PacMan closest = ClosestPacMan(entityContainer);
                int deltaX = closest.X - _ghost.X;
                int deltaY = closest.Y - _ghost.Y;

                /*
                System.Diagnostics.Debug.WriteLine($"deltaX : {deltaX} / deltaY : {deltaY}");
                System.Diagnostics.Debug.WriteLine(ObjectContainer.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE + 1] != GameMap.MapMeaning.WALL);
                System.Diagnostics.Debug.WriteLine(ObjectContainer.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE, _ghost.X / GameForm.SIZEOFSQUARE - 1] != GameMap.MapMeaning.WALL);*/

                // this is machin learning level thing :sunglass:
                if (deltaX >= deltaY && base._ghost.GetGameMapMeaning(0, GameMap.SIZEOFSQUARE) != GameMap.MapMeaning.WALL 
                    && base._ghost.GetGameMapMeaning(GameMap.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL || deltaY == 0)
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

                return direction;
            }
            
            private void FindDirection()
            {
                List<Direction> directions = base.AvailableDirections();
                base.CurrentDirection = directions[0];
                base._ghost.Vector2Ghost = DirectionsValues[directions[0]];
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
        protected GameContainer _objectContainer;

        /// <summary>
        /// Propriety
        /// </summary>
        public Direction CurrentDirection { get => _ghost.CurrentDirection; set => _ghost.CurrentDirection = value; }

        /// <summary>
        /// constructor
        /// </summary>
        public GhostAI(Ghost ghost)
        {
            this._ghost = ghost;
        }

        /// <summary>
        /// On update function
        /// </summary>
        public virtual void OnUpdate(IEntityContainer entityContainer)
        {
            MoveGhost();
        }

        /// <summary>
        /// Move the ghost
        /// </summary>
        protected void MoveGhost()
        {
            _ghost.Location = new Point(_ghost.X + _ghost.Vector2Ghost.X, _ghost.Y + _ghost.Vector2Ghost.Y); // wierd bug, when a ghost move, it erase the last graphic.fillRectangle, in 
        }

        /// <summary>
        /// Check if the player is to near
        /// </summary>
        /// <returns>if player is to near</returns>
        protected bool IsPlayerToNear(IEntityContainer entityContainer)
        {
            foreach (PacMan item in entityContainer.PacMans)
            {
                // this is basicly pythagor with DeltaX and DeltaY
                // thanks poland
                if (Math.Sqrt(Math.Pow(Math.Abs(item.X - _ghost.X), 2) + Math.Pow(Math.Abs(item.Y - _ghost.Y), 2)) <= _rayon)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// the future location of ghost
        /// </summary>
        /// <returns>if ghost can move</returns>
        protected bool NeedVectorUpdate()
        {
            if (_ghost.GetGameMapMeaning(_ghost.Vector2Ghost.X / Ghost.SPEED, _ghost.Vector2Ghost.Y / Ghost.SPEED) == GameMap.MapMeaning.WALL)
            //if (_ghost.Map.GameMapMeaning[_ghost.Y / GameForm.SIZEOFSQUARE + _ghost.EntityVector2.Y / Ghost.SPEED, _ghost.X / GameForm.SIZEOFSQUARE + _ghost.EntityVector2.X / Ghost.SPEED] == GameMap.MapMeaning.WALL)
            {
                this._ghost.Vector2Ghost.X = 0;
                this._ghost.Vector2Ghost.Y = 0;

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

            if (_ghost.GetGameMapMeaning(0, -GameMap.SIZEOFSQUARE) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.North);
            }

            if (_ghost.GetGameMapMeaning(GameMap.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.East);
            }

            if (_ghost.GetGameMapMeaning(0, GameMap.SIZEOFSQUARE) != GameMap.MapMeaning.WALL)
            {
                vs.Add(Direction.South);
            }

            if (_ghost.GetGameMapMeaning(-GameMap.SIZEOFSQUARE, 0) != GameMap.MapMeaning.WALL)
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
