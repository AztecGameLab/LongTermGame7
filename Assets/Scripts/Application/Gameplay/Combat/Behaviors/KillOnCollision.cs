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
            // Ensure the object we collided with is a layer we defined
            if ((1 << col.gameObject.layer & layersToHit) != 0)
            {
                GetComponent<LivingEntity>().Kill();
            }
        }
    }
}
