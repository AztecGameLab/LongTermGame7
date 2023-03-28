namespace Application.Gameplay
{
    /// <summary>
    /// An event signaling the player's movement between scenes.
    /// </summary>
    public class LevelChangeEvent
    {
        /// <summary>
        /// Gets or sets the scene that the player is moving to.
        /// </summary>
        public string NextScene { get; set; }

        /// <summary>
        /// Gets or sets the strategy that should be used to spawn the player.
        /// </summary>
        public ISpawningStrategy SpawningStrategy { get; set; } = new OriginSpawningStrategy();
    }
}
