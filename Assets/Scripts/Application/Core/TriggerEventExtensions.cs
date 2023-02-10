namespace Application.Core
{
    using System;
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Extensions for quickly hooking into trigger events.
    /// </summary>
    public static class TriggerEventExtensions
    {
        /// <summary>
        /// Gets or adds TriggerEvents onto a collider.
        /// </summary>
        /// <param name="collider">The collider to listen to.</param>
        /// <returns>The TriggerEvents attached to the collider.</returns>
        public static TriggerEvents GetTriggerEvents([NotNull] this Collider collider)
        {
            if (collider == null)
            {
                throw new ArgumentNullException(nameof(collider));
            }

            if (!collider.gameObject.TryGetComponent(out TriggerEvents triggerEvents))
            {
                triggerEvents = collider.gameObject.AddComponent<TriggerEvents>();
            }

            return triggerEvents;
        }
    }
}
