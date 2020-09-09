using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    public class Vector1
    {
        private bool _disposed = false;

        private int _length;

        public int Length { get => _length; set => _length = value; }

        public Vector1(int length)
        {
            this.Length = length;
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
