namespace Application.Gameplay.Combat
{
    using UnityEngine;

    /// <summary>
    /// Provides helper utilities for computing projectile motion data.
    /// </summary>
    public static class ProjectileMotion
    {
        public static Vector3 GetExplosionVelocity(Vector3 source, Vector3 target, float strength, float yBoost = 0)
        {
            var sourceToTarget = target - source;
            var upwardBoost = Vector3.up * yBoost;
            return (sourceToTarget.normalized * strength) + upwardBoost;
        }

            /// <summary>
        /// Calculates the velocity needed to launch a projectile between two points, over a set time.
        /// </summary>
        /// <param name="from">The starting point.</param>
        /// <param name="to">The ending point.</param>
        /// <param name="duration">How long the projectile should take in-flight.</param>
        /// <returns>The velocity to assign the projectile.</returns>
        public static Vector3 GetLaunchVelocity(Vector3 from, Vector3 to, float duration = 1)
        {
            float distance = GetFlatDistance(from, to);

            // Local-space launch velocity, derived from projectile motion equations
            float initialY = (to.y - from.y - (0.5f * Physics.gravity.y * duration * duration)) / duration;
            float initialZ = distance / duration;
            Vector3 localVelocity = new Vector3(0, initialY, initialZ);

            // Rotate the local-space velocity so its aimed the correct direction.
            Vector3 direction = (new Vector3(to.x, 0, to.z) - new Vector3(from.x, 0, from.z)).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            return rotation * localVelocity;
        }

        private static float GetFlatDistance(Vector3 from, Vector3 to)
        {
            return (new Vector2(to.x, to.z) - new Vector2(from.x, from.z)).magnitude;
        }
    }
}
