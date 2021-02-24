using PacManGame.GameView;
using PacManGame.Interfaces;
using PacManGame.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vector.Vector2;

namespace PacManGame.Entities
{
    /// <summary>
    /// Entity class
    /// Mother class
    /// </summary>
    public abstract class Entity : EntityTeleportation, IEntity
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
        public abstract IMapContainer MapContainer { get; set; }

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
            this.Body.Location = new Point(-GameMap.SIZEOFSQUARE, -GameMap.SIZEOFSQUARE);
            MapContainer.GameForm.panPanGame.Controls.Remove(this.Body);
            Map.GameMap.DrawMapRectangle(MapContainer.GameFormPanelGraphics, GameMap.MapMeaning.ROAD, x, y);
        }

        /// <summary>
        /// On update
        /// </summary>
        public abstract void OnUpdate(IEntityContainer entityContainer);// { }

        /// <summary>
        /// Tell you on wich type of case the entity is
        /// </summary>
        /// <returns>the type of case</returns>
        public GameMap.MapMeaning OnWichCaseIsEntity()
        {
            return MapContainer.Map.GameMapMeaning[this.Body.Location.Y / GameMap.SIZEOFSQUARE, this.Body.Location.X / GameMap.SIZEOFSQUARE];
        }

        /// <summary>
        /// Teleport the entity in a relation with the teleportation pad
        /// </summary>
        protected void TeleportEntity()
        {
            Point lastPosition = this.Body.Location;
            Point buffer = EntityTeleportation.EntityTeleportationDictionaryRelation[this.Body.Location];
            this.Body.Location = new Point(buffer.X, buffer.Y);
            GameMap.DrawMapRectangle(MapContainer.GameFormPanelGraphics, GameMap.MapMeaning.TELEPORT, lastPosition.X, lastPosition.Y);

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
            return MapContainer.Map.GameMapMeaning[(this.Body.Location.Y + yOffset) / GameMap.SIZEOFSQUARE, (this.Body.Location.X + xOffset) / GameMap.SIZEOFSQUARE];
        }

        /// <summary>
        /// Check if on grid
        /// </summary>
        /// <returns>if on grid</returns>
        public bool CheckIfOnGrid()
        {
            return this.Body.Location.Y % GameMap.SIZEOFSQUARE == 0 && this.Body.Location.X % GameMap.SIZEOFSQUARE == 0;
        }

        /// <summary>
        /// Change direction function
        /// </summary>
        /// <param name="direction">direction</param>
        public void ChangeDirection(Direction direction)
        {
            this.CurrentDirection = direction;
            this.Vector2Ghost = DirectionsValues[direction];
        }

        /// <summary>
        /// Check if we overland any entity
        /// </summary>
        /// <returns>if overlap entity</returns>
        public bool CheckIfEntityOverlap(IEntityContainer entityContainer)
        {
            foreach (PacMan other in entityContainer.PacMans)
            {
                if (other == this)
                {
                    continue;
                }

                if (CheckOverlap(other))
                {
                    RaiseEntityOverlaped(other);
                    return true;
                }
            }

            foreach (Ghost other in entityContainer.Ghosts)
            {
                if (other == this)
                {
                    continue;
                }

                if (CheckOverlap(other))
                {
                    RaiseEntityOverlaped(other);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if entity overlap with another entity
        /// </summary>
        /// <param name="entity">other entity</param>
        /// <returns>overlap</returns>
        private bool CheckOverlap(Entity entity)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(entity.X - this.X), 2) + Math.Pow(Math.Abs(entity.Y - this.Y), 2)) <= GameMap.SIZEOFSQUARE;

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
                    y = GameMap.SIZEOFSQUARE - this.Body.Location.Y % GameMap.SIZEOFSQUARE;
                    break;
                case Direction.East:
                    x = -this.Body.Location.X % GameMap.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        x -= GameMap.SIZEOFSQUARE;
                    }
                    break;
                case Direction.South:
                    y = -this.Body.Location.Y % GameMap.SIZEOFSQUARE;
                    if (CheckIfOnGrid())
                    {
                        y -= GameMap.SIZEOFSQUARE;
                    }
                    break;
                case Direction.West:
                    x = GameMap.SIZEOFSQUARE - this.Body.Location.X % GameMap.SIZEOFSQUARE;
                    break;
                default:
                    break;
            }

            // draw the map
            // looks like c#++
            // c# but with pointers ? microsoft ?
            GameMap.DrawMapRectangle(MapContainer.GameFormPanelGraphics,
                                     GetGameMapMeaning(x, y),
                                     this.Body.Location.X + x, this.Body.Location.Y + y);
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
}
