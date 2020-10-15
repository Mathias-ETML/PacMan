using System;

namespace PacMan
{
    /// <summary>
    /// 2D Vector class
    /// </summary>
    public class Vector2 : IDisposable
    {
        #region variables
        /// <summary>
        /// Attributs
        /// </summary>
        private bool _disposed = false;
        private int _x;
        private int _y;
        #endregion variables

        #region propriety
        /// <summary>
        /// Proprieties
        /// </summary>
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        #endregion propriety

        #region vector
        /// <summary>
        /// Default constructor
        /// </summary>
        public Vector2() { }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="x">int x</param>
        /// <param name="y">int y</param>
        public Vector2(int x, int y)
        {
            this._x = x;
            this._y = x;
        }
        #endregion vector

        #region memory managment
        /// <summary>
        /// Remove the vector from the memory
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
            }
        }

        /// <summary>
        /// Remove the vector from the memory
        /// </summary>
        /// <param name="disposing">if you want to remove the objects that are in the vector class</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            GC.SuppressFinalize(this);

            _disposed = true;
        }
        #endregion memory managment
    }
}
