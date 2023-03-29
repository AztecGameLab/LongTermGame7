namespace Application.Core
{
    using System.Collections.Generic;
    using Cinemachine;
    using UnityEngine;

    /// <summary>
    /// Extension methods for making life easier with Cinemachine Target Groups.
    /// </summary>
    public static class CinemachineTargetGroupExtensions
    {
        /// <summary>
        /// Removes all members from a cinemachine target group.
        /// </summary>
        /// <param name="targetGroup">The group to remove all members from.</param>
        public static void RemoveAllMembers(this CinemachineTargetGroup targetGroup)
        {
            if (targetGroup == null)
            {
                return;
            }

            // Clear out the old members.
            for (int i = targetGroup.m_Targets.Length - 1; i >= 0; i--)
            {
                var target = targetGroup.m_Targets[i].target;
                targetGroup.RemoveMember(target);
            }
        }

        /// <summary>
        /// Adds all members from a source list into the target group.
        /// </summary>
        /// <param name="targetGroup">The group to add new members to.</param>
        /// <param name="source">The list to draw new members from.</param>
        /// <param name="weight">The weight assigned to each member.</param>
        /// <param name="radius">The radius assigned to each member.</param>
        public static void AddMemberRange(this CinemachineTargetGroup targetGroup, IEnumerable<Transform> source, float weight, float radius)
        {
            if (targetGroup == null || source == null)
            {
                return;
            }

            foreach (Transform transform in source)
            {
                targetGroup.AddMember(transform, weight, radius);
            }
        }

        /// <inheritdoc cref="AddMemberRange(Cinemachine.CinemachineTargetGroup,System.Collections.Generic.IEnumerable{UnityEngine.Transform},float,float)"/>
        public static void AddMemberRange(this CinemachineTargetGroup targetGroup, IEnumerable<GameObject> source, float weight, float radius)
        {
            if (targetGroup == null || source == null)
            {
                return;
            }

            foreach (GameObject gameObject in source)
            {
                targetGroup.AddMember(gameObject.transform, weight, radius);
            }
        }
    }
}
