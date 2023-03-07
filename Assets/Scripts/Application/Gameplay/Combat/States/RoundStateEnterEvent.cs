namespace Application.Gameplay.Combat.States
{
    /// <summary>
    /// Called when a round state is entered.
    /// </summary>
    /// <typeparam name="T">The round state being entered.</typeparam>
    public class RoundStateEnterEvent<T>
        where T : RoundState
    {
        public T State;
    }
}
