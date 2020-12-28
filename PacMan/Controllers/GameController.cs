using System;
using PacManGame.Interfaces.IControllerNS;
using PacManGame.Entities;
using PacManGame.Interfaces.IEntityNS;
using static PacManGame.Interfaces.IEntityNS.Entity;
using static PacManGame.Entities.PacMan;
using static PacManGame.Entities.Ghost;
using static PacManGame.Map.GameMap;
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
            base.OnStart(new OnUpdateFunctionPointer(this.OnUpdate), 
                         new EntityOverlapedEventHandler(this.OnEntityOverlapEvent),
                         new OnPacManDeathEventHandler(this.OnPacManDeathEvent), 
                         new OnGhostDeathEventHandler(this.OnGhostDeathEvent),
                         new OnAllFoodWasEatenEventHandler(this.OnAllFoodWasEatenEvent));
        }

        public override void OnUpdate()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();

            OnUpdateGhost();
            OnUpdatePacMan();
            OnUpdateMap();

            //stopwatch.Stop();

            // tick to complete : 15-25'000
            // my processor speed : 4.05 ghz ( ryzen 7 3700x )
            // time for 1 loop : 20'000 / 4'050'000'000 = 2 / 405'000
            // give the result of ~4.938271 microseconds

            // this is bad

            //long p = stopwatch.ElapsedTicks;
        }

        public override void OnUpdateGhost()
        {
            for (int i = 0; i < ObjectContainer.Ghosts.Count; i++)
            {
                ObjectContainer.Ghosts[i].OnUpdate();
            }
        }

        public override void OnUpdatePacMan()
        {
            for (int i = 0; i < ObjectContainer.PacMans.Count; i++)
            {
                ObjectContainer.PacMans[i].OnUpdate();
            }
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
        }

        public override void OnEntityOverlapEvent(Entity sender, Entity overlaped)
        {
            if (sender is Ghost && overlaped is Ghost)
            {
                sender.ChangeDirection(EntityDirection.GetOpposit(sender.CurrentDirection));
                overlaped.ChangeDirection(EntityDirection.GetOpposit(overlaped.CurrentDirection));

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
                if (pacman.SpawnProtection)
                {
                    return;
                }

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

        public override void OnAllFoodWasEatenEvent()
        {
            System.Windows.Forms.MessageBox.Show("Wow, you did it. You played this game ? That is unexcepted");

            GameControllerManager.TerminateGame(this);
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
