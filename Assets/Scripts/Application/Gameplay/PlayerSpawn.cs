namespace Application.Gameplay
{
    using Core;
    using UnityEngine;

    public class PlayerSpawn : MonoBehaviour
    {
        private void Awake()
        {
            var instance = Services.PlayerTeamData.Player.CreateWorldView();
            instance.transform.position = transform.position;
        }
    }
}
