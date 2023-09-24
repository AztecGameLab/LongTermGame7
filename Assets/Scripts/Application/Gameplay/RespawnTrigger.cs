using Application.Core;
using Application.Gameplay.Combat;
using UnityEngine;

// Use this script if you want the Respawn point and the trigger for it to be in seperate places
// Remember to add a BoxCollider and check the isTrigger box if you use it
public class RespawnTrigger : MonoBehaviour
{
    [SerializeField]
    private RespawnPoint respawn; // variable to get the id of the respawn point
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Services.RespawnTracker.SetRespawnPoint(respawn.Id);    // Set the respawn point
        }
    }
}