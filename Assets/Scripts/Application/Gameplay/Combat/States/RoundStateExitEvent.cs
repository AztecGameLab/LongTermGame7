namespace Application.Gameplay.Combat.States
{
    /// <summary>
    /// Called when a round state is exited.
    /// </summary>
    /// <typeparam name="T">The round state being exited.</typeparam>
    public class RoundStateExitEvent<T>
        where T : RoundState
    {
        public T State;
    }
}
