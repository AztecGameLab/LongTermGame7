namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    using Core.Utility;
    using UnityEngine;

    /// <summary>
    /// A simple enemy type that chooses to chase and attack the player.
    /// </summary>
    public class ChaseAndMeleeBrain : MonsterBrain
    {
        [SerializeField]
        private float meleeRange = 1;

        [SerializeField]
        private float attackDamage = 1;

        [SerializeField]
        private float moveDistance = 5;

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

                if (distance > meleeRange)
                {
                    yield return transform.PathFindTo(targetPosition, stopDistance: meleeRange / 2, maxDistance: moveDistance);
                    distance = Vector3.Distance(transform.position, targetPosition);
                }

                if (distance <= meleeRange && closest.TryGetComponent(out LivingEntity entity))
                {
                    // melee attack, presumable play attack animation here.
                    yield return new WaitForSeconds(1);
                    entity.Damage(attackDamage);
                    yield return new WaitForSeconds(1);
                }
            }
        }
    }
}
