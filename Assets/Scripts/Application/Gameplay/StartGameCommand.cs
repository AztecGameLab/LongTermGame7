namespace Application.Gameplay
{
    /// <summary>
    /// A request to enter gameplay.
    /// </summary>
    public class StartGameCommand
    {
        /// <summary>
        /// The initial scene that the game should be started in.
        /// </summary>
        public string InitialScene = string.Empty;
    }
}
