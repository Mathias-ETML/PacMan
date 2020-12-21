using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacManGame.Interfaces.IControllerNS;
using PacManGame.Entities;
using PacManGame.Interfaces.IEntityNS;
using static PacManGame.Interfaces.IEntityNS.Entity;
using static PacManGame.Interfaces.IEntityNS.EntityBase;

namespace PacManGame.Controllers.GameControllerNS
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
            base.OnStart(new OnUpdateFunctionPointer(this.OnUpdate), new EntityOverlapedEventHandler(this.OnEntityOverlapEvent));
        }

        public override void OnUpdate()
        {
            OnUpdateGhost();
            OnUpdatePacMan();
            OnUpdateMap();
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
            foreach (PacMan pc in ObjectContainer.PacMans)
            {
                pc.OnUpdate();
            }
        }

        public override void OnUpdateMap()
        {
            foreach (Ghost gh in ObjectContainer.Ghosts)
            {
                gh.OnUpdateMap();
            }

            foreach (PacMan pc in ObjectContainer.PacMans)
            {
                pc.OnUpdateMap();
            }
        }

        public override void OnEntityOverlapEvent(Entity sender, Entity overlaped)
        {
            throw new NotImplementedException();
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
