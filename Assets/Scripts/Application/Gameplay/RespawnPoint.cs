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
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
               SetActive();
            }
        }

        [Button]
        private void SetActive()
        {
           // if () { } check to see if there is already a respawn point. If so, make it null
           
           Services.RespawnTracker.SetRespawnPoint(id);
        }
    }
}
