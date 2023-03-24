using Application.Core;
using TriInspector;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    public class RespawnPoint : MonoBehaviour
    {
        [SerializeField]
        private string id;

        public string Id => id;

        [Button]
        private void SetActive()
        {
            Services.RespawnTracker.SetRespawnPoint(id);
        }
    }
}