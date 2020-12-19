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
        void OnStart(UpdatableObjectOnUpdateFunctionPointer.OnUpdateFunctionPointer updatableObjectOnUpdateFunctionPointer);

        void OnUpdate();
    }

    /// <summary>
    /// UpdatableObjectOnUpdateFunctionPointer abstract class
    /// </summary>
    public abstract class UpdatableObjectOnUpdateFunctionPointer
    {
        public delegate void OnUpdateFunctionPointer();
    }
}
