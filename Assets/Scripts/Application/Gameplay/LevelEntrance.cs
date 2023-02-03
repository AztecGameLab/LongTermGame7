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
        /// Gets the ID of this entrance. This is used to link the exit and the entrance together.
        /// </summary>
        public string EntranceID => entranceID;

        /// <summary>
        /// Gets a value indicating whether gets whether this entrance should be the default one.
        /// That means it is used as a fallback if the requested one cannot be found.
        /// </summary>
        public bool DefaultEntrance => defaultEntrance;
    }
}