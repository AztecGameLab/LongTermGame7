namespace Application.Core.Utility
{
    using System.Collections;
    using Gameplay.Combat.UI.Indicators;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Static utility methods for working with NavMeshPaths.
    /// </summary>
    public static class NavMeshPathUtil
    {
        /// <summary>
        /// Determines the total distance of a path.
        /// </summary>
        /// <param name="path">The path to check the distance of.</param>
        /// <returns>The length of the path.</returns>
        public static float CalculateDistance(NavMeshPath path)
        {
            if (path != null && path.corners.Length > 1)
            {
                float total = 0;

                for (int i = 1; i < path.corners.Length; i++)
                {
                    total += Vector3.Distance(path.corners[i - 1], path.corners[i]);
                }

                return total;
            }
            else
            {
                return 0;
            }
        }

        // todo: for some reason this doesnt work?

        /// <summary>
        /// Visualizes this path by drawing lines between each corner.
        /// </summary>
        /// <param name="path">The path to visualize.</param>
        /// <param name="color">The color to use when drawing this line.</param>
        /// <param name="duration">How long the lines should stay visible for.</param>
        public static void DebugDraw(NavMeshPath path, Color color, float duration = 0)
        {
            if (path != null && path.corners.Length > 1)
            {
                for (int i = 1; i < path.corners.Length; i++)
                {
                    Debug.DrawLine(path.corners[i - 1], path.corners[i], color, duration, true);
                }
            }
        }

        /// <summary>
        /// Gets the position at some distance along the path.
        /// </summary>
        /// <param name="path">The path to traverse.</param>
        /// <param name="distance">The distance along the path to find the position of.</param>
        /// <returns>The position on a path.</returns>
        public static Vector3 GetPositionAtDistance(NavMeshPath path, float distance)
        {
            if (path != null && path.corners.Length > 1)
            {
                float elapsed = 0;

                for (int i = 1; i < path.corners.Length; i++)
                {
                    float d = Vector3.Distance(path.corners[i - 1], path.corners[i]);

                    if (elapsed + d >= distance)
                    {
                        // we found the final segment, calc and return pos
                        float remaining = distance - elapsed;
                        Vector3 direction = (path.corners[i] - path.corners[i - 1]).normalized;
                        return path.corners[i - 1] + (direction * remaining);
                    }
                    else
                    {
                        // we are still too short
                        elapsed += d;
                    }
                }

                // If we traverse the entire path and never hit the distance, just snap to the last point.
                return path.corners[path.corners.Length - 1];
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Moves this transform to a position along the path.
        /// </summary>
        /// <param name="transform">The transform to move.</param>
        /// <param name="path">The path to move along.</param>
        /// <param name="targetDistance">How far to move along the path.</param>
        public static void MoveTo(this Transform transform, NavMeshPath path, float targetDistance)
        {
            if (path != null && path.corners.Length > 1 && transform != null)
            {
                var newPosition = GetPositionAtDistance(path, targetDistance);
                transform.position = newPosition;
            }
        }

        public static IEnumerator PathFindTo(
            this Transform user,
            Vector3 targetPosition,
            float moveSpeed = 5,
            float stopDistance = 1,
            float maxDistance = -1,
            IPooledObject<PathIndicator> indicator = null)
        {
            if (user == null)
            {
                yield break;
            }

            bool hasIndicator = true;

            if (indicator == null)
            {
                indicator = Services.IndicatorFactory.Borrow<PathIndicator>();
                indicator.Instance.IsValid = true;
                hasIndicator = false;
            }

            float elapsedDistance = 0;
            NavMeshPath path = new NavMeshPath();
            NavMeshPath inProgressPath = new NavMeshPath();
            NavMesh.CalculatePath(user.transform.position, targetPosition, NavMesh.AllAreas, path);

            float totalDistance = CalculateDistance(path);
            float distance = Mathf.Min(totalDistance, maxDistance) - stopDistance;

            while (elapsedDistance < distance)
            {
                NavMesh.CalculatePath(user.transform.position, targetPosition, NavMesh.AllAreas, inProgressPath);
                elapsedDistance += Time.deltaTime * moveSpeed;
                user.transform.MoveTo(path, elapsedDistance);
                indicator.Instance.RenderPath(inProgressPath.corners);
                yield return null;
            }

            if (!hasIndicator)
            {
                indicator.Instance.ClearPath();
                indicator.Dispose();
            }
        }
    }
}
