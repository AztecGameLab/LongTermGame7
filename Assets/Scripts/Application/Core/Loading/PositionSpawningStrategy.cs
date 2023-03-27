namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Spawns the player at a certain position in the scene.
    /// </summary>
    public class PositionSpawningStrategy : ISpawningStrategy
    {
        private readonly Vector3 _position;

        /// <summary>
        /// Initializes a new instance of the <see cref="PositionSpawningStrategy"/> class.
        /// </summary>
        /// <param name="position">The position to spawn at.</param>
        public PositionSpawningStrategy(Vector3 position)
        {
            _position = position;
        }

        /// <inheritdoc/>
        public Vector3 CalculateSpawnPosition()
        {
            return _position;
        }
    }
}
