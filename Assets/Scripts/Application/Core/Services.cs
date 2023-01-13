namespace Application.Core
{
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            // Resets static data, so fast play mode works without carried-over data..
            EventBus = null;
        }
    }
}
