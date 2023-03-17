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
        /// Gets or sets the entrance that the player is aiming for.
        /// </summary>
        public string TargetID { get; set; }
    }
}
