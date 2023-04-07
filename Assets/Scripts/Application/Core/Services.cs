namespace Application.Core
{
    using Audio;
    using Gameplay;
    using Gameplay.Combat;
    using Gameplay.Combat.UI.Indicators;
    using Gameplay.Regions;
    using Serialization;
    using UnityEngine;

    /// <summary>
    /// A global access point for cross-cutting concerns.
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// Gets or sets the global EventBus.
        /// </summary>
        public static EventBus EventBus { get; set; }

        /// <summary>
        /// Gets or sets the global serializer.
        /// </summary>
        public static Serializer Serializer { get; set; }

        /// <summary>
        /// Gets or sets the global RegionTracker.
        /// </summary>
        public static RegionTracker RegionTracker { get; set; }

        public static RespawnTracker RespawnTracker { get; set; }

        public static MusicPlayer MusicPlayer { get; set; }
        
        public static DialogueSystem DialogueSystem { get; set; }

        /// <summary>
        /// Gets or sets the global indicator factory.
        /// </summary>
        public static IndicatorFactory IndicatorFactory { get; set; }

        /// <summary>
        /// Gets or sets the global player team data.
        /// </summary>
        public static TeamData PlayerTeamData { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            // Resets static data, so fast play mode works without carried-over data..
            EventBus = null;
            Serializer = null;
            RegionTracker = null;
            IndicatorFactory = null;
            PlayerTeamData = null;
            RegionTracker = null;
            DialogueSystem = null;
            MusicPlayer = null;
        }
    }
}
