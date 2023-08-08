namespace Application.Gameplay.Combat
{
    using Core;
    using TriInspector;
    using UnityEngine;

    /// <summary>
    /// A position that the player can respawn at.
    /// </summary>
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string id;

        /// <summary>
        /// Gets the ID for this respawn point.
        /// </summary>
        public string Id => id;

        [Button]
        public void SetActive()
        {
            Services.RespawnTracker.SetRespawnPoint(id);
        }
    }
}
