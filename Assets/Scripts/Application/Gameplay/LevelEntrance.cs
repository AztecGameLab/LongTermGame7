using TriInspector;

namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// A place where the player may enter the level from.
    /// </summary>
    public class LevelEntrance : MonoBehaviour
    {
        [SerializeField]
        private string entranceID;

        [SerializeField]
        private bool defaultEntrance;

        /// <summary>
        /// Gets or sets the ID of this entrance. This is used to link the exit and the entrance together.
        /// </summary>
        [ShowInInspector]
        public string EntranceID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets whether this entrance should be the default one.
        /// That means it is used as a fallback if the requested one cannot be found.
        /// </summary>
        [ShowInInspector]
        public bool DefaultEntrance { get; set; }

        private void Awake()
        {
            // EntranceID = entranceID;
            // DefaultEntrance = defaultEntrance;
        }
    }
}
