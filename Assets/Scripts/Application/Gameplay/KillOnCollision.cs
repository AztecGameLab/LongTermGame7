using System;

namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Kills an entity when it collides with something.
    /// </summary>
    [RequireComponent(typeof(LivingEntity))]
    public class KillOnCollision : MonoBehaviour
    {
        private void OnCollisionEnter()
        {
            GetComponent<LivingEntity>().Kill();
        }
    }
}
