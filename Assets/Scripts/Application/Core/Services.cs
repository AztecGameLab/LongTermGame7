using Application.Gameplay.Regions;

namespace Application.Core
{
    using Gameplay;
    using UnityEngine;

    /// <summary>
    /// A global access point for cross-cutting concerns.
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// Gets or sets the global EventBus.
        /// </summary>
        /// <value>
        /// The global EventBus.
        /// </value>
        public static EventBus EventBus { get; set; }

        /// <summary>
        /// Gets or sets the global RegionTracker.
        /// </summary>
        /// <value>
        /// The global RegionTracker.
        /// </value>
        public static RegionTracker RegionTracker { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            // Resets static data, so fast play mode works without carried-over data..
            EventBus = null;
            RegionTracker = null;
        }
    }
}
