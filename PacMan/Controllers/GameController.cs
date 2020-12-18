using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacMan.Interfaces.IControllerNS;
using PacMan.Entities;


namespace PacMan.Controllers.GameControllerNS
{
    /// <summary>
    /// Game controller class
    /// </summary>
    public sealed class GameController : Controller
    {
        private bool disposedValue = false;

        /// <summary>
        /// default constructor
        /// </summary>
        public GameController() : base()
        {
            // unfortunatly, i cannot pass the OnUpdateFunctionPointer in the constructor, because it need to be static
            OnStart();
        }

        public void OnStart()
        {
            base.OnStart(new OnUpdateFunctionPointer(this.OnUpdate));
        }

        public override void OnUpdate()
        {
            OnUpdateGhost();
            OnUpdatePacMan();
        }

        public override void OnUpdateGhost()
        {
            foreach (Ghost gh in ObjectContainer.Ghosts)
            {
                gh.OnUpdate();
            }
        }

        public override void OnUpdatePacMan()
        {
            foreach (Entities.PacMan pc in ObjectContainer.PacMans)
            {
                pc.OnUpdate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                base.Dispose();

                disposedValue = true;
            }
        }

        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
