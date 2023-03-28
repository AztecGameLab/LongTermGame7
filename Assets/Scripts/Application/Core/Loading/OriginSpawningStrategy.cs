namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Spawns the player at the player spawn, if it exists.
    /// </summary>
    public class OriginSpawningStrategy : ISpawningStrategy
    {
        /// <inheritdoc/>
        public Vector3 CalculateSpawnPosition()
        {
            var spawn = Object.FindObjectOfType<PlayerSpawn>();
            return spawn.transform.position;
        }
    }
}
