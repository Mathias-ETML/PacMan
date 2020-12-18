using System;
using System.Collections.Generic;
using Vector.Vector2;
using PacMan.Interfaces.IUpdatableObjectNS;
using PacMan.Interfaces.IControllerNS;
using System.Windows.Forms;

namespace PacMan.Interfaces.IEntityNS
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
    public abstract class Entity : EntityDirection, IEntity
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

        #region IDisposable Support
        /// <summary>
        /// disposed value
        /// </summary>
        protected bool _disposedValue = false;

        /// <summary>
        /// IDisposable implementaiton
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose function
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
}
