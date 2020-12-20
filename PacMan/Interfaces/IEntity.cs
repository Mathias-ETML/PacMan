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
        void Spawn();

        void Die();
    }

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
        /// Object container field
        /// </summary>
        public abstract ObjectContainer ObjectContainer { get; set; }

        /// <summary>
        /// Body pannel
        /// </summary>
        public abstract Panel Body { get; set; }

        /// <summary>
        /// entity vector
        /// </summary>
        public abstract Vector2 EntityVector2 { get; set; }

        /// <summary>
        /// Current direction
        /// </summary>
        public abstract Direction CurrentDirection { get; set; }

        /// <summary>
        /// Spawn entity
        /// </summary>
        public abstract void Spawn();

        /// <summary>
        /// Die function
        /// </summary>
        public abstract void Die();

        /// <summary>
        /// On start
        /// </summary>
        public abstract void OnStart();// { }

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
            GameMap.DrawMapRectangle(ObjectContainer.GameFormPanelGraphics, GetGameMapMeaning(), lastPosition.X, lastPosition.Y);
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

            // getting the point
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

            GameMap.DrawMapRectangle(
                
                ObjectContainer.GameFormPanelGraphics, 
                
                ObjectContainer.Map.GameMapMeaning[(this.Body.Location.Y + y) / GameForm.SIZEOFSQUARE, (this.Body.Location.X + x) / GameForm.SIZEOFSQUARE], 

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
