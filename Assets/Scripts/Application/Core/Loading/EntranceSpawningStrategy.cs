namespace Application.Gameplay
{
    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Spawns the player at a level entrance, if it exists.
    /// </summary>
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
        public Vector3 CalculateSpawnPosition()
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
