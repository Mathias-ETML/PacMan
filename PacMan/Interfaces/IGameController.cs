using System;

namespace PacManGame.Interfaces
{
    /// <summary>
    /// IController interface
    /// </summary>
    public interface IGameController : IController, IDisposable
    {
        void OnUpdatePacMan();

        void OnUpdateGhost();
    }
}
