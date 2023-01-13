namespace Application.Core
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Signals that a level has been successfully loaded.
    /// </summary>
    [PublicAPI]
    public readonly struct LoadLevelEvent : IEquatable<LoadLevelEvent>
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

        /// <inheritdoc/>
        public bool Equals(LoadLevelEvent other) => LevelName == other.LevelName;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LoadLevelEvent other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => LevelName != null ? LevelName.GetHashCode() : 0;
    }
}
