using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan.Variables;

namespace PacMan
{
    public partial class Ghost : AIRegroupgment, IDisposable
    {
        private Panel _body;
        private bool _disposed = false;
        private GhostType.Type _type;
        private GhostAI _AI;

        public Panel Body { get => _body; }

        public Point GhostLocation
        {
            get => _body.Location;
            set => _body.Location = new Point(value.X, value.Y);
        }

        private Vector2 DeplacementGhost;

        public Vector2 GetDeplacementGhost { get => DeplacementGhost; }

        public Ghost(int x, int y, GhostType.Type Type)
        {
            _body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE)
            };

            DeplacementGhost = new Vector2(0, 0);

            switch (Type)
            {
                case GhostType.Type.YELLOW:
                    _AI = new YellowAI();
                    break;
                case GhostType.Type.BLUE:
                    _AI = new BlueAI();
                    break;
                case GhostType.Type.PINK:
                    _AI = new PinkAI();
                    break;
                case GhostType.Type.RED:
                    _AI = new RedAI();
                    break;
                default:
                    _AI = null;
                    break;
            }

            this._type = Type;
        }

        public void SetPacManDeplacement(int x, int y)
        {
            DeplacementGhost.X = x;
            DeplacementGhost.Y = y;
        }

        public void Move()
        {
            if (_body != null && _AI != null)
            {
                _AI.OnUpdate();
            }
        }

        //public int intWhereGhostNeedToMoveX { get; set; } = 0;
        //public int intWhereGhostNeedToMoveY { get; set; } = 0;

        public void ChangeGhostDirection()
        {
            if (_body != null)
            {

            }
        }

        /// <summary>
        /// disposing
        /// </summary>
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
                _body.Dispose();
            }

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }

    public class GhostType
    {
        /*
        YELLOW : 
        BLUE : Random at each intersection
        PINK : Scared, go to opposit of youself, but if you are in a radius of you, chase you for 5 sec max
        RED : Chase you, try to go to the close way possible of you
        */

        public enum Type : byte
        {
            YELLOW,
            BLUE,
            PINK,
            RED
        }
    }

    /// <summary>
    /// Is this bad ? Idk, i never did abstract and override
    /// </summary>
    public class AIRegroupgment
    {
        public class YellowAI : GhostAI
        {
            public override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }

        public class BlueAI : GhostAI
        {
            public override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }

        public class PinkAI : GhostAI
        {
            public override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }

        public class RedAI : GhostAI
        {
            public override void OnUpdate()
            {
                throw new NotImplementedException();
            }
        }
    }

    public abstract class GhostAI
    {
        public abstract void OnUpdate();
    }
}
