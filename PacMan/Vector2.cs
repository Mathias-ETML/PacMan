using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Vector2 : IDisposable
    {
        private bool _disposed = false;

        private int _x;
        private int _y;

        // default constructor
        public Vector2() { }

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }

        public Vector2(int x, int y)
        {
            this._x = x;
            this._y = x;
        }

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

            }

            GC.SuppressFinalize(this);

            _disposed = true;
        }
    }
}
