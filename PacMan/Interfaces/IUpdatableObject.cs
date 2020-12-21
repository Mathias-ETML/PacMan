using PacManGame.Interfaces.IEntityNS;
using System;
using static PacManGame.Interfaces.IEntityNS.Entity;

namespace PacManGame.Interfaces.IUpdatableObjectNS
{
    /// <summary>
    /// IUpdatableObject interface
    /// </summary>
    public interface IUpdatableObject
    {
        void OnStart();

        void OnUpdate();
    }

    /// <summary>
    /// IUpdatableObjectGameController interface
    /// </summary>
    public interface IUpdatableObjectGameController
    {
        void OnStart(UpdatableObjectFunctionPointer.OnUpdateFunctionPointer updatableObjectOnUpdateFunctionPointer,
             EntityBase.EntityOverlapedEventHandler onEntityOverlapFunctionPointer);

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
