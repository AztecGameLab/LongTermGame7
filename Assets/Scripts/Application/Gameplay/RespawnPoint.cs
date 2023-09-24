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
        
        /* IF YOU WANT THE RESPAWN POINT AND THE TRIGGER TO ACTIVATE IT TO BE IN THE SAME SPOT, USE THIS SCRIPT
        * ALONG WITH THE OnTriggerEnter FUNCTION BELOW.
        * IF YOU WANT THE RESPWAN POINT AND THE TRIGGER TO BE SEPERATE, USE THIS SCRIPT (just how it is) AS WELL AS
        * THE RESPAWN TRIGGER SCRIPT
        */
        
        // If you use this OnTriggerEnter method, remember to add a BoxCollider and check the isTrigger box
        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.tag == "Player")
        //     {
        //        SetActive();
        //     }
        // }

        [Button]
        private void SetActive()
        {
            Services.RespawnTracker.SetRespawnPoint(id);
        }
    }
}
