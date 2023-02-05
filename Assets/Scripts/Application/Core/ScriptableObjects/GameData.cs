namespace Application.Core.ScriptableObjects
{
    using UnityEngine;

    /// <summary>
    /// Data to be made available
    /// </summary>
    public class GameData : ScriptableObject
    {
        // the current region
        private static Region _currentRegion = Region.Undefined;

        public enum Region
        {
            /// <summary>
            /// Undefined is the default region and should be changed for every scene
            /// </summary>
            Undefined,
            Forest,
            Plains,
            Mountains
        }

        /// <summary>
        /// Gets or sets the current region
        /// </summary>
        /// <value>The current region.</value>
        public static Region CurrentRegion
        {
            get
            {
                return _currentRegion;
            }

            set
            {
                Debug.Log("Updating region to: " + value);
                _currentRegion = value;
            }
        }
    }
}
