using System;
using System.Collections.Generic;
using Vector.Vector2;
using PacManGame.GameView;
using PacManGame.Map;
using System.Windows.Forms;
using System.Drawing;
using PacManGame.Entities;

namespace PacManGame.Interfaces
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
    public abstract class EntityTeleportation : EntityDirection
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
