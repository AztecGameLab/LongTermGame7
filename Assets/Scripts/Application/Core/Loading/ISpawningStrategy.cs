namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// A method used for determining where the player should spawn in a scene.
    /// </summary>
    public interface ISpawningStrategy
    {
        /// <summary>
        /// Calculates a spawn position for the player.
        /// </summary>
        /// <returns>The position to spawn the player.</returns>
        Vector3 CalculateSpawnPosition();
    }
}
