namespace Application.Core
{
    /// <summary>
    /// An object that can be loaded by the StateMachine class.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called when this state is set as the current state.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called during each Update().
        /// </summary>
        void OnTick();

        /// <summary>
        /// Called before this state is replaced as the current state.
        /// </summary>
        void OnExit();
    }
}
