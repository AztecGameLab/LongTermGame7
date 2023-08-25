namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    using Core.Utility;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// A simple enemy type that chooses to chase and attack the player.
    /// </summary>
    public class ChaseAndProjectileBrain : MonsterBrain
    {
        [SerializeField]
        private float moveDistance = 5;

        [SerializeField]
        private float stopDistance = 2;

        [SerializeField]
        private AssetReference projectile;

        [SerializeField]
        private float projectileTime = 1;

        /// <inheritdoc/>
        protected override IEnumerator MakeDecision(BattleController controller)
        {
            if (controller == null)
            {
                yield break;
            }

            var closest = controller.PlayerTeam.GetClosest(transform.position, out float distance);

            if (closest != null)
            {
                var targetPosition = closest.transform.position;
                yield return transform.PathFindTo(targetPosition, stopDistance: stopDistance, maxDistance: moveDistance);

                yield return new WaitForSeconds(1);

                Vector3 origin = transform.position + Vector3.up;
                var launchVelocity = ProjectileMotion.GetLaunchVelocity(origin, targetPosition, projectileTime);
                var projectileInstance = projectile.InstantiateAsync(origin, Quaternion.LookRotation(launchVelocity)).WaitForCompletion();

                if (projectileInstance.TryGetComponent(out Rigidbody body))
                {
                    body.velocity = launchVelocity;
                    yield return new WaitForSeconds(projectileTime);
                }

                yield return new WaitForSeconds(1);
            }
        }
    }
}
