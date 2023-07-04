using UnityEngine;
namespace Application.Gameplay.Combat.Brains
{
    using Application.Core.Utility;
    using System.Collections;
    public class GhostALight : MonsterBrain
    {
        public float invisibilityRange;
        public Collider targetCollider;
        [SerializeField] private GameObject projectilePrefab;

        protected override IEnumerator MakeDecision(BattleController controller)
        {
            var closest = controller.PlayerTeam.GetClosest(transform.position, out float distance);
            //if the closest person is too close, go invisibility
            if (distance <= invisibilityRange)
            {
                targetCollider.enabled = false;

                if (TryGetComponent(out Rigidbody rb))
                    rb.isKinematic = true;
            }
            else {          //else, fire a projectile to the closest
                targetCollider.enabled = true;
                if (TryGetComponent(out Rigidbody rb))
                    rb.isKinematic = false;
                var launchVelocity = ProjectileMotion.GetLaunchVelocity(transform.position, closest.transform.position);

                Vector3 origin = transform.position + Vector3.up;
                var prefab = Instantiate(projectilePrefab, origin, Quaternion.identity);

                if (prefab.TryGetComponent(out Rigidbody rigidbody))
                {
                rigidbody.velocity = launchVelocity;
                yield return new WaitForSeconds(1);
                }
            }
            yield return null;
        }

    }
}
