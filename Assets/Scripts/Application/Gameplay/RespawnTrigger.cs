using Application.Core;
using Application.Gameplay.Combat;
using UnityEngine;

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