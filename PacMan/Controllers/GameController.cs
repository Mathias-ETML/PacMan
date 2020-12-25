using System;
using PacManGame.Interfaces.IControllerNS;
using PacManGame.Entities;
using PacManGame.Interfaces.IEntityNS;
using static PacManGame.Interfaces.IEntityNS.Entity;
using static PacManGame.Entities.PacMan;
using static PacManGame.Entities.Ghost;
using System.Diagnostics;

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
            base.OnStart(new OnUpdateFunctionPointer(this.OnUpdate), new EntityOverlapedEventHandler(this.OnEntityOverlapEvent), new OnPacManDeathEventHandler(this.OnPacManDeathEvent), new OnGhostDeathEventHandler(this.OnGhostDeathEvent));
        }

        public override void OnUpdate()
        {
            OnUpdateGhost();
            OnUpdatePacMan();
            OnUpdateMap();
        }

        public override void OnUpdateGhost()
        {
            for (int i = 0; i < ObjectContainer.Ghosts.Count; i++)
            {
                ObjectContainer.Ghosts[i].OnUpdate();
            }

            /*
            foreach (Ghost gh in ObjectContainer.Ghosts)
            {
                gh.OnUpdate();
            }*/
        }

        public override void OnUpdatePacMan()
        {
            for (int i = 0; i < ObjectContainer.PacMans.Count; i++)
            {
                ObjectContainer.PacMans[i].OnUpdate();
            }

            /*
            foreach (PacMan pc in ObjectContainer.PacMans)
            {
                pc.OnUpdate();
            }*/
        }

        public override void OnUpdateMap()
        {
            for (int i = 0; i < ObjectContainer.Ghosts.Count; i++)
            {
                ObjectContainer.Ghosts[i].OnUpdateMap();
            }

            for (int i = 0; i < ObjectContainer.PacMans.Count; i++)
            {
                ObjectContainer.PacMans[i].OnUpdateMap();
            }

            /*
            foreach (Ghost gh in ObjectContainer.Ghosts)
            {
                gh.OnUpdateMap();
            }

            foreach (PacMan pc in ObjectContainer.PacMans)
            {
                pc.OnUpdateMap();
            }*/
        }

        public override void OnEntityOverlapEvent(Entity sender, Entity overlaped)
        {
            if (sender is Ghost && overlaped is Ghost)
            {
                sender.CurrentDirection = EntityDirection.GetOpposit(sender.CurrentDirection);
                overlaped.CurrentDirection = EntityDirection.GetOpposit(overlaped.CurrentDirection);

                return;
            }

            PacMan pacman;
            Ghost ghost;

            switch (sender)
            {
                case Ghost _:
                    ghost = sender as Ghost;
                    pacman = overlaped as PacMan;
                    break;
                default:
                    pacman = sender as PacMan;
                    ghost = overlaped as Ghost;
                    break;
            }

            if (pacman.CanPacManEatGhost)
            {
                ghost.RaiseDeath();
            }
            else
            {
                pacman.RaiseDeath();
            }
        }

        public override void OnPacManDeathEvent(PacMan pacman)
        {
            if (pacman.Die())
            {
                pacman.OnUpdateMap();
                pacman.Dispawn();
                pacman.Spawn(PacMan.XSPAWN, PacMan.YSPAWN);
            }
            else
            {
                ObjectContainer.PacMans.Remove(pacman);
                pacman.Dispose();

                if (ObjectContainer.PacMans.Count == 0) // multiplayer next year
                {
                    GameControllerManager.TerminateGame(this);
                }
            }
        }
        
        public override void OnGhostDeathEvent(Ghost ghost)
        {
            if (ghost.Die())
            {
                // not implemented, but you can if you want
            }
            else
            {
                ghost.OnUpdateMap();
                ghost.Dispawn();
                ObjectContainer.Ghosts.Remove(ghost);
                ghost.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    base.Dispose(true);
                }

                disposedValue = true;
            }
        }

        public new void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public static class GameControllerManager
    {
        /// <summary>
        /// this is bad but at this point yolo
        /// </summary>
        /// <param name="gameController">game controller</param>
        public static void TerminateGame(GameController gameController)
        {
            gameController.Dispose();
        }
    }
}
