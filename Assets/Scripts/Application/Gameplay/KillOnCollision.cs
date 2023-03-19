namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Kills an entity when it collides with something.
    /// </summary>
    [RequireComponent(typeof(LivingEntity))]
    public class KillOnCollision : MonoBehaviour
    {
        [SerializeField]
        private LayerMask layersToHit;

        private void OnTriggerEnter(Collider col)
        {
            if ((1 << col.gameObject.layer & layersToHit) != 0)
            {
                GetComponent<LivingEntity>().Kill();
            }
        }
    }
}
