using PacManGame.Entities;
using PacManGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManGame.Controllers
{
    public class LogicController : IController, IGameController
    {
        private GameContainer _gameContainer;

        public GameContainer GameContainer { get => _gameContainer; set => _gameContainer = value; }

        public LogicController(GameContainer container)
        {
            this._gameContainer = container;
        }

        public void OnStart()
        {
            throw new NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public void OnUpdatePacMan()
        {
            throw new NotImplementedException();
        }

        public void OnUpdateGhost()
        {
            throw new NotImplementedException();
        }

        public void OnEntityOverlapedEvent(Entity entitySender, Entity overlapedEntity)
        {
            throw new NotImplementedException();
        }

        public void OnGhostDeathEvent(Ghost ghost)
        {
            throw new NotImplementedException();
        }

        public void OnPacManDeathEvent(PacMan pacman)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // Pour détecter les appels redondants

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: supprimer l'état managé (objets managés).
                }

                // TODO: libérer les ressources non managées (objets non managés) et remplacer un finaliseur ci-dessous.
                // TODO: définir les champs de grande taille avec la valeur Null.

                disposedValue = true;
            }
        }

        // Ce code est ajouté pour implémenter correctement le modèle supprimable.
        public void Dispose()
        {
            // Ne modifiez pas ce code. Placez le code de nettoyage dans Dispose(bool disposing) ci-dessus.
            Dispose(true);
            // TODO: supprimer les marques de commentaire pour la ligne suivante si le finaliseur est remplacé ci-dessus.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
