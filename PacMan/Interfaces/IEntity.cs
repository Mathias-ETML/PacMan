using System;
using System.Collections.Generic;
using Vector.Vector2;
using PacManGame.Interfaces.IUpdatableObjectNS;
using PacManGame.Interfaces.IControllerNS;
using PacManGame.GameView;
using PacManGame.Map;
using System.Windows.Forms;
using System.Drawing;

namespace PacManGame.Interfaces.IEntityNS
{
    /// <summary>
    /// IEntity interface
    /// Mother interface
    /// </summary>
    public interface IEntity : IUpdatableObject, IDisposable
    {
        void Spawn(int x, int y);

        bool Die();
    }

    /// <summary>
    /// Entity class
    /// Mother class
    /// </summary>
    public abstract class Entity : EntityBase, IEntity
    {
        /// <summary>
        /// Pixel to move when on update function occure
        /// </summary>
        public const int SPEED = 10;

        /// <summary>
        /// Delegate for the event
        /// </summary>
        /// <param name="entitySender">sender</param>
        /// <param name="overlapedEntity">overlaped entity</param>
        public delegate void EntityOverlapedEventHandler(Entity entitySender, Entity overlapedEntity);

        /// <summary>
        /// Event when entity overlap
        /// </summary>
        public event EntityOverlapedEventHandler EntityOverlaped;

        /// <summary>
        /// function to call when entities overlap
        /// </summary>
        /// <param name="overlapedEntity">the overlaped entity</param>
        protected virtual void RaiseEntityOverlaped(Entity overlapedEntity)
        {
            EntityOverlaped?.Invoke(this, overlapedEntity);
        }

        /// <summary>
        /// Object container field
        /// </summary>
        public abstract ObjectContainer ObjectContainer { get; set; }

        /// <summary>
        /// Body pannel
        /// </summary>
        public abstract Panel Body { get; set; }

        /// <summary>
        /// X field
        /// </summary>
        public virtual int X { get => Body.Location.X; }

        /// <summary>
        /// Y field
        /// </summary>
        public virtual int Y { get => Body.Location.Y; }

        /// <summary>
        /// entity vector
        /// </summary>
        public abstract Vector2 Vector2Ghost { get; set; }

        /// <summary>
        /// Current direction
        /// </summary>
        public abstract Direction CurrentDirection { get; set; }

        /// <summary>
        /// Spawn entity
        /// </summary>
        public abstract void Spawn(int x, int y);

        /// <summary>
        /// Get if alive
        /// </summary>
        /// <returns>if alive</returns>
        public abstract bool IsAlive();

        /// <summary>
        /// Die function
        /// </summary>
        public abstract bool Die();

        /// <summary>
        /// dispawn entity
        /// </summary>
        public virtual void Dispawn()
        {
            int x = X;
            int y = Y;
            this.Body.Location = new Point(-GameForm.SIZEOFSQUARE, -GameForm.SIZEOFSQUARE);
            ObjectContainer.GameForm.panPanGame.Controls.Remove(this.Body);
            Map.GameMap.DrawMapRectangle(ObjectContainer.GameFormPanelGraphics, GameMap.MapMeaning.ROAD, x, y);
        }

        /// <summary>
        /// On update
        /// </summary>
        public abstract void OnUpdate();// { }

        /// <summary>
        /// Tell you on wich type of case the entity is
        /// </summary>
        /// <returns>the type of case</returns>
        public Map.GameMap.MapMeaning OnWichCaseIsEntity()
        {
            return ObjectContainer.Map.GameMapMeaning[this.Body.Location.Y / GameForm.SIZEOFSQUARE, this.Body.Location.X / GameForm.SIZEOFSQUARE];
        }

        /// <summary>
        /// Teleport the entity in a relation with the teleportation pad
        /// </summary>
        protected void TeleportEntity()
        {
            Point lastPosition = this.Body.Location;
            Point buffer = EntityTeleportation.EntityTeleportationDictionaryRelation[this.Body.Location];
            this.Body.Location = new Point(buffer.X, buffer.Y);
            GameMap.DrawMapRectangle(ObjectContainer.GameFormPanelGraphics, GameMap.MapMeaning.TELEPORT, lastPosition.X, lastPosition.Y);

            // there is a bug with creating a new point for a panel :
            // it erase the last drawn rectangle
            // why ? idk. maybe the stack ?
        }

        /// <summary>
        /// Get you the type of map chunk you are on
        /// </summary>
        /// <param name="xOffset">x offset</param>
        /// <param name="yOffset">y offset</param>
        /// <returns>type of map chunk you are on</returns>
        public GameMap.MapMeaning GetGameMapMeaning(int xOffset = 0, int yOffset = 0)
        {
            return ObjectContainer.Map.GameMapMeaning[(this.Body.Location.Y  + yOffset) / GameForm.SIZEOFSQUARE, (this.Body.Location.X + xOffset) / GameForm.SIZEOFSQUARE];
        }

        /// <summary>
        /// Check if on grid
        /// </summary>
        /// <returns>if on grid</returns>
        public bool CheckIfOnGrid()
        {
            return this.Body.Location.Y % GameForm.SIZEOFSQUARE == 0 && this.Body.Location.X % GameForm.SIZEOFSQUARE == 0;
        }


        public void ChangeDirection(Direction direction)
        {
            this.CurrentDirection = direction;
            this.Vector2Ghost = DirectionsValues[direction];
        }

        /// <summary>
        /// Check if we overland any entity
        /// </summary>
        /// <returns>if overlap entity</returns>
        public bool CheckIfEntityOverlap()
        {
            foreach (Entity item in ObjectContainer)
            {
                if (item == this)
                {
                    continue;
                }

                if (CheckOverlap(item))
                {
                    RaiseEntityOverlaped(item);
                    return true;
                }
            }

            return false;
        }

        private bool CheckOverlap(Entity entity)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(entity.X - this.X), 2) + Math.Pow(Math.Abs(entity.Y - this.Y), 2)) <= GameForm.SIZEOFSQUARE;

            /*
            return entity.Y + GameForm.SIZEOFSQUARE == this.Y && entity.X == this.X || // north

                   entity.X - GameForm.SIZEOFSQUARE == this.X && entity.Y == this.Y || // east

                   entity.Y - GameForm.SIZEOFSQUARE == this.Y && entity.X == this.X || // south

                   entity.X + GameForm.SIZEOFSQUARE == this.X && entity.Y == this.Y; // west
                   */
        }   

        /// <summary>
        /// Update the map for the entity
        /// </summary>
        public void OnUpdateMap()
        {
            /*
            GameMap.DrawMapRectangle(ObjectContainer.GameFormPanelGraphics, ObjectContainer.Map.GameMapMeaning[Body.Location.Y / GameForm.SIZEOFSQUARE, Body.Location.X / GameForm.SIZEOFSQUARE],
                Body.Location.X - EntityVector2.X, Body.Location.Y - EntityVector2.Y);*/

            int x = 0;
            int y = 0;

            // getting direction
            // i will explain why and why not, but now i am tired and i have other things to do
            switch (CurrentDirection)
            {
                case Direction.North:
                    y = GameForm.SIZEOFSQUARE - this.Body.Location.Y % GameForm.SIZEOFSQUARE;
                    break;
                case Direction.East:
                    x = -this.Body.Location.X % GameForm.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        x -= GameForm.SIZEOFSQUARE;
                    }
                    break;
                case Direction.South:
                    y = -this.Body.Location.Y % GameForm.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        y -= GameForm.SIZEOFSQUARE;
                    }
                    break;
                case Direction.West:
                    x = GameForm.SIZEOFSQUARE - this.Body.Location.X % GameForm.SIZEOFSQUARE;
                    break;
                default:
                    break;
            }

            // draw the map
            GameMap.DrawMapRectangle(ObjectContainer.GameFormPanelGraphics,
                                     GetGameMapMeaning(x, y), 
                                    (this.Body.Location.X + x), (this.Body.Location.Y + y)  );
        }

        #region IDisposable Support
        /// <summary>
        /// disposed value
        /// </summary>
        private bool _disposedValue = false;

        /// <summary>
        /// IDisposable implementaiton
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {

                }
            }

            _disposedValue = true;
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        #endregion IDisposable Support
    }

    /// <summary>
    /// Entity base class
    /// </summary>
    public abstract class EntityBase : EntityTeleportation
    {

    }

    /// <summary>
    /// Entity direction class
    /// </summary>
    public abstract class EntityDirection 
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
        /// Get the opposit of a direction
        /// </summary>
        /// <param name="direction">direction</param>
        /// <returns>opposit of direction</returns>
        public static Direction GetOpposit(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.East:
                    return Direction.West;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                default:
                    throw new Exception("You are a magician, this sould never happen");
            }
        }

        /// <summary>
        /// Interface to get the entity direction
        /// </summary>
        public static readonly Dictionary<Direction, Vector2> DirectionsValues = new Dictionary<Direction, Vector2>(4)
        {
            {Direction.North, new Vector2(0, -Entity.SPEED)},
            {Direction.East, new Vector2(Entity.SPEED, 0)},
            {Direction.South, new Vector2(0, Entity.SPEED)},
            {Direction.West, new Vector2(-Entity.SPEED, 0)}
        };
    }

    /// <summary>
    /// Class for the teleportation relation
    /// </summary>
    public class EntityTeleportation : EntityDirection
    {
        // work with XY
        // point to where to teleport the entity
        private static Point FirstLocation = new Point(40, 360);
        private static Point FirstLocationEnd = new Point(640, 360);
        private static Point SecondLocation = new Point(680, 360);
        private static Point SecondLocationEnd = new Point(80, 360);

        // get you on wich location to teleport entity
        public static readonly Dictionary<Point, Point> EntityTeleportationDictionaryRelation = new Dictionary<Point, Point>(2)
        {
            {FirstLocation, FirstLocationEnd },
            {SecondLocation, SecondLocationEnd}
        };
    }
}
