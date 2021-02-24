using PacManGame.Entities;
using PacManGame.GameView;
using PacManGame.Interfaces;
using PacManGame.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PacManGame.Controllers
{
    /// <summary>
    /// Controller class
    /// </summary>
    public class UpdateController : IController
    {
        /// <summary>
        /// Const
        /// </summary>
        public const int TIME_TO_UPDATE = 250; // in ms
        public const int NUMBER_OF_MAXIMUM_PACMAN = 2;
        public const int NUMBER_OF_MAXIMUM_PACMAN_WHILE_IAM_TOO_LAZY_TODO_TCP_IP_IMPLEMENTATION = 1;
        public const int NUMBER_OF_MAXIMUM_GHOST = 4;

        /// <summary>
        /// Attributs
        /// </summary>
        private bool disposedValue = false;
        protected int _numberOfPacMan = 0;
        protected int _numberOfGhost = 0;
        protected Timer _onUpdateTimer;
        private GameContainer _gameContainer;
        private LogicController _logicController;

        /// <summary>
        /// Property
        /// </summary>
        public GameContainer GameContainer { get => _gameContainer; set => _gameContainer = value; }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="gameFrom">form</param>
        public UpdateController()
        {
            _onUpdateTimer = new Timer()
            {
                Interval = TIME_TO_UPDATE
            };

            _gameContainer = new GameContainer();
            this._logicController = new LogicController(_gameContainer);
        }

        /// <summary>
        /// On start function
        /// </summary>
        public virtual void OnStart()
        {
            _onUpdateTimer.Start();

            _gameContainer.GameForm.OnStart();

            for (int i = 0; i < NUMBER_OF_MAXIMUM_PACMAN_WHILE_IAM_TOO_LAZY_TODO_TCP_IP_IMPLEMENTATION; i++)
            {
                GameContainer.PacMans.Add(new PacMan(PacMan.XSPAWN, PacMan.YSPAWN, GameContainer));
            }

            _gameContainer.Ghosts.Add(new Ghost(6 * GameMap.SIZEOFSQUARE, 7 * GameMap.SIZEOFSQUARE, Ghost.Type.BLUE, GameContainer));
            _gameContainer.Ghosts.Add(new Ghost((6 + 6) * GameMap.SIZEOFSQUARE, (7 + 4) * GameMap.SIZEOFSQUARE, Ghost.Type.RED, GameContainer));

            foreach (Entity item in GameContainer)
            {
                //ObjectContainer.GameForm.panPanGame.Controls.Add(item.Body);
                item.EntityOverlaped += _logicController.OnEntityOverlapedEvent;

                if (item is PacMan)
                {
                    ((PacMan)item).PacManDeathEvent += _logicController.OnPacManDeathEvent;
                }
                else
                {
                    ((Ghost)item).GhostDeathEvent += _logicController.OnGhostDeathEvent;
                }
            }

            Application.Run(GameContainer.GameForm);
        }

        public virtual void OnUpdate()
        {

        }

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true); // bug here
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
