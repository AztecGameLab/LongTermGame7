namespace Application.Gameplay
{
    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;

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

    public interface ISpawningStrategy
    {
        Vector3 GetSpawnPosition();
    }

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
        public Vector3 GetSpawnPosition()
        {
            return _position;
        }
    }

    public class OriginSpawningStrategy : ISpawningStrategy
    {
        /// <inheritdoc/>
        public Vector3 GetSpawnPosition()
        {
            var spawn = UnityEngine.Object.FindObjectOfType<PlayerSpawn>();
            return spawn.transform.position;
        }
    }

    public class EntranceSpawningStrategy : ISpawningStrategy
    {
        private readonly string _targetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntranceSpawningStrategy"/> class.
        /// </summary>
        /// <param name="targetId">The ID of the level entrance to enter through.</param>
        public EntranceSpawningStrategy(string targetId)
        {
            _targetId = targetId;
        }

        /// <inheritdoc/>
        public Vector3 GetSpawnPosition()
        {
            LevelEntrance entrance = UnityEngine.Object.FindObjectsOfType<LevelEntrance>()
                .FirstOrDefault(entrance => entrance.EntranceID == _targetId);

            // If we are traveling between entrances and exits, we want to use that position.
            if (entrance != default)
            {
                return entrance.transform.position;
            }

            throw new ArgumentException(
                $"Entrance {_targetId} was not found when loading {SceneManager.GetActiveScene().name}.");
        }
    }
}
