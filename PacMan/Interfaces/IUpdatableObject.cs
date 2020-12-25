using PacManGame.Interfaces.IEntityNS;
using System;

namespace PacManGame.Interfaces.IUpdatableObjectNS
{
    /// <summary>
    /// IUpdatableObject interface
    /// </summary>
    public interface IUpdatableObject
    {
        void OnUpdate();
    }

    /// <summary>
    /// IUpdatableObjectGameController interface
    /// </summary>
    public interface IUpdatableObjectGameController
    {
        void OnStart(UpdatableObjectFunctionPointer.OnUpdateFunctionPointer updatableObjectOnUpdateFunctionPointer,
                     Entity.EntityOverlapedEventHandler onEntityOverlapFunctionPointer,
                     Entities.PacMan.OnPacManDeathEventHandler onPacManDeathEventHandler,
                     Entities.Ghost.OnGhostDeathEventHandler onGhostDeathEventHandler);

        void OnUpdate();
    }

    /// <summary>
    /// UpdatableObjectOnUpdateFunctionPointer abstract class
    /// </summary>
    public abstract class UpdatableObjectFunctionPointer
    {
        public delegate void OnUpdateFunctionPointer();
    }
}
