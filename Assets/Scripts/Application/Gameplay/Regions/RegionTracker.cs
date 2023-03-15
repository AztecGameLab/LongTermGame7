namespace Application.Gameplay.Regions
{
    using UnityEngine;

    /// <summary>
    /// Data to be made available.
    /// </summary>
    public class RegionTracker
    {
        // the current region
        private Region _currentRegion = Region.Undefined;

        /// <summary>
        /// The different potential areas in the game.
        /// </summary>
        public enum Region
        {
            /// <summary>
            /// Undefined is the default region and should be changed for every scene
            /// </summary>
            Undefined,
            Forest,
            Plains,
            Mountains,
        }

        /// <summary>
        /// Gets or sets the current region.
        /// </summary>
        /// <value>The current region.</value>
        public Region CurrentRegion
        {
            get
            {
                return _currentRegion;
            }

            set
            {
                if (_currentRegion != value)
                {
                    Debug.Log("Updating region to: " + value);
                    _currentRegion = value;
                }
            }
        }
    }
}
