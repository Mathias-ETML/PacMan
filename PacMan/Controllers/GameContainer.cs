using PacManGame.Entities;
using PacManGame.GameView;
using PacManGame.Interfaces;
using PacManGame.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManGame.Controllers
{
    public class GameContainer : IGameContainer
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
        public GameContainer()
        {
            this.Ghosts = new List<Ghost>();
            this.PacMans = new List<PacMan>();
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

                    // BAD, VERY BAD
                    for (int i = 0; i < PacMans.Count; i++)
                    {
                        PacMan buffer = PacMans[i];
                        PacMans.Remove(buffer);
                        buffer.Dispose();
                        i--;
                    }

                    for (int i = 0; i < Ghosts.Count; i++)
                    {
                        Ghost buffer = Ghosts[i];
                        Ghosts.Remove(buffer);
                        buffer.Dispose();
                        i--;
                    }

                    GameFormPanelGraphics.Dispose();
                }

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
        #endregion


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
    }
}
