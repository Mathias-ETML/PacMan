using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using PacManGame.Interfaces.IUpdatableObjectNS;
using PacManGame.Entities;
using PacManGame.Map;
using PacManGame.GameView;
using PacManGame.Interfaces.IEntityNS;
using System.Collections;

namespace PacManGame.Interfaces.IControllerNS
{
    /// <summary>
    /// IObjectContainer Interface
    /// </summary>
    public interface IObjectContainer : IDisposable
    {
        List<Ghost> Ghosts { get; set; }

        List<PacMan> PacMans { get; set; }

        GameMap Map { get; set; }

        GameForm GameForm { get; set; }

        Graphics GameFormPanelGraphics { get; set; }
    }

    /// <summary>
    /// IObjectContainer class
    /// </summary>
    public class ObjectContainer : IObjectContainer, IEnumerable<Entity>
    {
        /// <summary>
        /// Attribut
        /// </summary>
        private bool disposedValue = false; // Pour détecter les appels redondants

        /// <summary>
        /// Properties
        /// </summary>
        public List<Ghost> Ghosts { get; set; }
        public List<PacMan> PacMans { get; set; }
        public GameMap Map { get; set; }
        public GameForm GameForm { get; set; }
        public Graphics GameFormPanelGraphics { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ObjectContainer()
        {
            this.Ghosts = new List<Ghost>();
            this.PacMans = new List<Entities.PacMan>();
            this.Map = new GameMap();
            this.GameForm = new GameForm(this);
            this.GameFormPanelGraphics = GameForm.panPanGame.CreateGraphics();
        }

        #region IDisposable Support

        /// <summary>
        /// Dispose implementation
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Map.Dispose();
                    GameForm.Dispose();

                    foreach (PacMan pc in PacMans)
                    {
                        pc.Dispose();
                    }

                    foreach (Ghost gh in Ghosts)
                    {
                        gh.Dispose();
                    }

                    GameFormPanelGraphics.Dispose();
                }

                GameFormPanelGraphics = null;

                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get you the entity
        /// </summary>
        /// <returns>entity</returns>
        public IEnumerator<Entity> GetEnumerator()
        {
            foreach (PacMan item in PacMans)
            {
                yield return item as Entity;
            }

            foreach (Ghost item in Ghosts)
            {
                yield return item as Entity;
            }
        }

        /// <summary>
        /// Get you the entity
        /// </summary>
        /// <returns>entity</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (PacMan item in PacMans)
            {
                yield return item as Entity;
            }

            foreach (Ghost item in Ghosts)
            {
                yield return item as Entity;
            }
        }
        #endregion
    }

    /// <summary>
    /// IController interface
    /// </summary>
    public interface IController : IUpdatableObjectGameController, IDisposable
    {
        void OnUpdatePacMan();

        void OnUpdateGhost();
    }

    /// <summary>
    /// Controller class
    /// </summary>
    public abstract class Controller : ControllerBase, IController
    {
        /// <summary>
        /// Const
        /// </summary>
        public const int TIMETOUPDATE = 250; // in ms
        public const int NUMBEROFMAXIMUMPACMAN = 2;
        public const int NUMBEROFMAXIMUMPACMANWHILEIAMTOOLAZYTODOTCPIPPLEMENTATION = 1;
        public const int NUMBEROFMAXIMUMGHOST = 4;

        /// <summary>
        /// Attributs
        /// </summary>
        private OnUpdateFunctionPointer _onUpdateFunctionPointer;
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected int _numberOfPacMan = 0;
        protected int _numberOfGhost = 0;
        
        protected new ObjectContainer ObjectContainer { get => base.ObjectContainer; set => base.ObjectContainer = value; }

        /// <summary>
        /// Custom constructor
        /// </summary>
        /// <param name="map">map</param>
        /// <param name="gameFrom">form</param>
        public Controller()
        {
            base.OnUpdateTimer = new Timer()
            {
                Interval = TIMETOUPDATE
            };

            base.ObjectContainer = new ObjectContainer();
        }

        /// <summary>
        /// On start function
        /// </summary>
        /// <param name="onUpdateFunctionPointer">pointer to the on update function of the child class</param>
        public virtual void OnStart(OnUpdateFunctionPointer onUpdateFunctionPointer)
        {
            this._onUpdateFunctionPointer = onUpdateFunctionPointer;

            OnUpdateTimer.Tick += OnUpdateHandler;

            OnUpdateTimer.Start();

            ObjectContainer.GameForm.OnStart();

            for (int i = 0; i < NUMBEROFMAXIMUMPACMANWHILEIAMTOOLAZYTODOTCPIPPLEMENTATION; i++)
            {
                ObjectContainer.PacMans.Add(new PacMan(40, 40, ObjectContainer));
            }

            ObjectContainer.Ghosts.Add(new Ghost(0 * GameForm.SIZEOFSQUARE + GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE, Ghost.Type.BLUE, ObjectContainer));
            ObjectContainer.Ghosts.Add(new Ghost(1 * GameForm.SIZEOFSQUARE + GameForm.SIZEOFSQUARE, GameForm.SIZEOFSQUARE, Ghost.Type.RED, ObjectContainer));

            /*
            for (int i = 0; i < NUMBEROFMAXIMUMGHOST; i++)
            {
                ObjectContainer.Ghosts.Add(new Ghost(i * GameView.GameForm.SIZEOFSQUARE + GameView.GameForm.SIZEOFSQUARE, GameView.GameForm.SIZEOFSQUARE, (Ghost.Type)i, ObjectContainer));
            }*/

            foreach (Entity item in ObjectContainer)
            {
                ObjectContainer.GameForm.panPanGame.Controls.Add(item.Body);
            }

            Application.Run(ObjectContainer.GameForm);
        }

        /// <summary>
        /// On update handler
        /// 
        /// Handle the pointer to the child class
        /// </summary>
        /// <param name="sender">timer</param>
        /// <param name="e">event arg</param>
        private void OnUpdateHandler(object sender, EventArgs e)
        {
            _onUpdateFunctionPointer();
        }

        public abstract void OnUpdate();

        public abstract void OnUpdateGhost();

        public abstract void OnUpdatePacMan();

        public abstract void OnUpdateMap();

        #region IDisposable Support
        protected new virtual void Dispose(bool disposing)
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    public abstract class ControllerBase : UpdatableObjectOnUpdateFunctionPointer, IDisposable
    {
        protected Timer OnUpdateTimer { get; set; }

        protected ObjectContainer ObjectContainer { get; set; }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    OnUpdateTimer.Stop();
                    OnUpdateTimer.Dispose();

                    ObjectContainer.Dispose();
                }

                disposedValue = true;
            }
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
