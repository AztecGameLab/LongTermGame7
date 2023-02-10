namespace Application.Core.Events
{
    /// <summary>
    /// Signals that a level has been successfully loaded.
    /// </summary>
    public readonly struct LoadLevelEvent
    {
        /// <summary>
        /// The name of the level that has been loaded.
        /// </summary>
        public readonly string LevelName;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadLevelEvent"/> struct.
        /// </summary>
        /// <param name="levelName">The name of the level that has been loaded.</param>
        public LoadLevelEvent(string levelName)
        {
            LevelName = levelName;
        }
    }
}
