using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PacMan.Variables;

/*
 * TODO : EXCEPTIONS WHEN SOMETHING GOES WRONG
 * TODO : Comments
 * TODO : IA for ghosts
 * TODO : REGIONS
 */
namespace PacMan
{
    public partial class Ghost : AIRegroupgment, IDisposable
    {
        /*
        YELLOW : 
        BLUE : Random at each intersection
        PINK : Scared, go to opposit of youself, but if you are in a radius of you, chase you for 5 sec max
        RED : Chase you, try to go to the close way possible of you
        */
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

        private Panel _body;
        private bool _disposed = false;
        private Type _type;
        private GhostAI _AI;
        private Vector2 DeplacementGhost;
        #endregion variables

        #region proprieties
        public Panel Body { get => _body; }
        public Point GhostLocation
        {
            get => _body.Location;
            set => _body.Location = new Point(value.X, value.Y);
        }
        public Vector2 GetDeplacementGhost { get => DeplacementGhost; }
        #endregion proprieties

        #region Ghost construtor
        public Ghost(int x, int y, Type type)
        {
            _body = new Panel()
            {
                Location = new Point(x, y),
                Size = new Size(G_BYTESIZEOFSQUARE, G_BYTESIZEOFSQUARE)
            };

            DeplacementGhost = new Vector2(0, 0);

            switch (type)
            {
                case Type.YELLOW:
                    _AI = new YellowAI();
                    break;
                case Type.BLUE:
                    _AI = new BlueAI();
                    break;
                case Type.PINK:
                    _AI = new PinkAI();
                    break;
                case Type.RED:
                    _AI = new RedAI();
                    break;
                default:
                    _AI = null;
                    break;
            }

            this._type = type;
        }
        #endregion Ghost constructor

        #region Ghost update
        public void Move()
        {
            if (_body != null && _AI != null)
            {
                _AI.OnUpdate();
            }
        }
        #endregion Ghost update

        #region Ghost deplacment
        public void ChangeGhostDirection()
        {
            if (_body != null)
            {

            }
        }

        public void SetGhostDeplacement(int x, int y)
        {
            DeplacementGhost.X = x;
            DeplacementGhost.Y = y;
        }
        #endregion Ghost deplacment

        #region memory managment
        /// <summary>
        /// Dispose
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
        #endregion memory managment
    }

    #region Ghost AI
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
    #endregion Ghost AI
}
