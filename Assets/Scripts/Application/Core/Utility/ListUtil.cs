﻿namespace Application.Core.Utility
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Static utility methods for working with lists.
    /// </summary>
    public static class ListUtil
    {
        /// <summary>
        /// Iterates through a list of GameObjects and returns the closest one to a position.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="position">The position to compare to for distance.</param>
        /// <param name="distance">The final distance of the closest object.</param>
        /// <returns>The closest object from the list, null if the list in null.</returns>
        public static GameObject GetClosest(this IList<GameObject> list, Vector3 position, out float distance)
        {
            if (list == null)
            {
                distance = -1;
                return null;
            }

            if (list.Count == 1)
            {
                distance = Vector3.Distance(position, list[0].transform.position);
                return list[0];
            }
            else if (list.Count > 1)
            {
                GameObject closest = list[0];
                float closestDistanceSqr = (list[0].transform.position - position).sqrMagnitude;

                for (int i = 1; i < list.Count; i++)
                {
                    var distSqr = (list[i].transform.position - position).sqrMagnitude;

                    if (distSqr < closestDistanceSqr)
                    {
                        closest = list[i];
                        closestDistanceSqr = distSqr;
                    }
                }

                distance = Mathf.Sqrt(closestDistanceSqr);
                return closest;
            }
            else
            {
                distance = -1;
                return null;
            }
        }

        /// <summary>
        /// Iterates through a list of GameObjects and returns the furthest one to a position.
        /// </summary>
        /// <param name="list">The list to search.</param>
        /// <param name="position">The position to compare to for distance.</param>
        /// <param name="distance">The final distance of the furthest object.</param>
        /// <returns>The closest object from the list, null if the list in null.</returns>
        public static GameObject GetFurthest(this IList<GameObject> list, Vector3 position, out float distance)
        {
            if (list == null)
            {
                distance = -1;
                return null;
            }

            if (list.Count == 1)
            {
                distance = Vector3.Distance(position, list[0].transform.position);
                return list[0];
            }
            else if (list.Count > 1)
            {
                GameObject furthest = list[0];
                float furthestDistanceSqr = (list[0].transform.position - position).sqrMagnitude;

                for (int i = 1; i < list.Count; i++)
                {
                    var distSqr = (list[i].transform.position - position).sqrMagnitude;

                    if (distSqr > furthestDistanceSqr)
                    {
                        furthest = list[i];
                        furthestDistanceSqr = distSqr;
                    }
                }

                distance = Mathf.Sqrt(furthestDistanceSqr);
                return furthest;
            }
            else
            {
                distance = -1;
                return null;
            }
        }
    }
}
