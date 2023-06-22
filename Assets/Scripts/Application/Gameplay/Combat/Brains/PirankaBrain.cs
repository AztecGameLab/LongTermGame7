namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    using Core.Utility;
    using UnityEngine;

    /// <summary>
    /// A simple enemy type that chooses to chase and attack the player.
    /// </summary>
    public class PirankaBrain : MonsterBrain
    {
        [SerializeField]
        private float meleeRange = 5;

        [SerializeField]
        private float projectileRange = 8;

        [SerializeField]
        private float physicalAttackDamage = 5;

        [SerializeField]
        private float rangedAttackDamage = 3;

        [SerializeField]
        private float moveDistance = 5;

        /// <inheritdoc/>
        /// A brain for an enemy that will shoot water at you from a range, or leap at you if you are too close
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
                Debug.Log("This is distance: " + distance);

                if (AttackType(distance) == 1 && closest.TryGetComponent(out LivingEntity entityRanged))
                {
                    // melee attack, presumable play attack animation here.
                    yield return new WaitForSeconds(1);
                    entityRanged.Damage(physicalAttackDamage);
                    yield return new WaitForSeconds(1);
                }
                else if (AttackType(distance) == 2 && closest.TryGetComponent(out LivingEntity entityPhysical))
                {
                    // ranged attack, presumable play attack animation here.
                    yield return new WaitForSeconds(1);
                    entityPhysical.Damage(rangedAttackDamage);
                    yield return new WaitForSeconds(1);
                }
                else
                {
                    yield return transform.PathFindTo(targetPosition, stopDistance: 2, maxDistance: moveDistance);
                    distance = Vector3.Distance(transform.position, targetPosition);
                }
            }
        }

        /// <summary>
        /// aa.
        /// </summary>
        private int AttackType(float distance)
        {
            // This value signals for the creature to do nothing.
            // The creature will only be able to .
            int value = 0;

            if (distance < meleeRange)
            {
                value = 1;
            }
            else if (distance > meleeRange && distance <= projectileRange)
            {
                value = 2;
            }

            // Else statement is not required as all conditions are covered by previous ones.
            return value;
        }
    }
}
