namespace Application.Gameplay
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Associates regions with scenes, for the purpose of sub-arena loading.
    /// </summary>
    [CreateAssetMenu]
    public class SubArenaLookup : ScriptableObject
    {
        [SerializeField]
        private List<RegionMappingData> data;

        [SerializeField]
        private bool showDebug;

        /// <summary>
        /// Gets the name of a random sub-arena scene, associated with a certain region.
        /// </summary>
        /// <param name="region">The region to look for when choosing a sub-arena scene.</param>
        /// <returns>The name of the scene that contains the sub-arena.</returns>
        public string GetSceneName(RegionTracker.Region region)
        {
            Log("GetSceneName ENTERED");
            Log("ABOUT TO ENTER LOOP");

            foreach (var mappingData in data)
            {
                int listLength = mappingData.SubArenaSceneNames.Length;
                int randomIndex = Random.Range(0, listLength);

                // Entered loop check.
                Log("Loop entered/repeating");

                // Checking if randomIndex is different from last time.
                Log("RANDOM INDEX CHOSEN: " + randomIndex);

                if (region == mappingData.Region)
                {
                    Log("correct region found!");
                    Log($"Choosing a random sub arena: {mappingData.SubArenaSceneNames[randomIndex]}");
                    return mappingData.SubArenaSceneNames[randomIndex];
                }

                Log("Scene not found in region this region.");
            }

            Log("GetSceneName ENDED");
            return string.Empty;
        }

        private void Log(string message)
        {
            if (showDebug)
            {
                Debug.Log(message);
            }
        }

        /// <summary>
        /// Serialized data for associated a region with a list of sub-arenas.
        /// </summary>
        [Serializable]
        public class RegionMappingData
        {
            [SerializeField]
            private RegionTracker.Region region;

            [SerializeField]
            private string[] subArenaSceneNames;

            /// <summary>
            /// Gets the target region.
            /// </summary>
            /// <value>
            /// The target region.
            /// </value>
            public RegionTracker.Region Region => region;

            /// <summary>
            /// Gets the names of scenes that might be loaded from this region.
            /// </summary>
            /// <value>
            /// The names of scenes that might be loaded from this region.
            /// </value>
            public string[] SubArenaSceneNames => subArenaSceneNames;
        }
    }
}
