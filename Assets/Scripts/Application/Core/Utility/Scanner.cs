namespace Application.Core.Utility
{
    using UnityEngine;

    /// <summary>
    /// Utility class for quickly finding objects in the world.
    /// </summary>
    public static class Scanner
    {
        /// <summary>
        /// Returns all of the colliders within a slice of a sphere.
        /// </summary>
        /// <param name="origin">The origin of the slice.</param>
        /// <param name="direction">The direction of the slice.</param>
        /// <param name="spread">The spread of the slice in degrees.</param>
        /// <param name="radius">The radius of the slice.</param>
        /// <param name="mask">What objects should be included when searching.</param>
        /// <param name="results">The buffer where results are stored.</param>
        /// <returns>How many objects were added to the result buffer.</returns>
        public static int GetAllInSlice(Vector3 origin, Vector3 direction, float spread, float radius, LayerMask mask, Collider[] results)
        {
            int hits = Physics.OverlapSphereNonAlloc(origin, radius, results, mask);
            int result = 0;

            for (int i = 0; i < hits; i++)
            {
                var collider = results[i];
                var v = collider.transform.position - origin;
                v.y = 0;

                if (Vector3.Angle(direction, v) <= spread)
                {
                    results[result++] = collider;
                }
            }

            return result;
        }
    }
}
