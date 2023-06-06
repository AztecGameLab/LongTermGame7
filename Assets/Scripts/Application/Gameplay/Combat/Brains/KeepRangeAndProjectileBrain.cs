using Application.Core.Abstraction;
using Application.Core.Utility;
using System;
using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat.Brains
{
    public class KeepRangeAndProjectileBrain : MonsterBrain
    {
        [SerializeField] private float lungeRange = 5;
        [SerializeField] private float lungeSpeed = 15;
        [SerializeField] private float lungeDamage = 1;
        [SerializeField] private float moveAmount = 2;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 1;
        [SerializeField] private float waitTime = 2;
        [SerializeField] private PhysicsComponent physics;
        [SerializeField] private LayerMask layersToDamage;

        private bool _isLunging;

        protected override IEnumerator MakeDecision(BattleController controller)
        {
            GameObject target = controller.PlayerTeam.GetClosest(transform.position, out float distance);

            if (distance < lungeRange)
            {
                // lunge
                _isLunging = true;
                Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
                physics.Velocity = directionToTarget * lungeSpeed;
                yield return new WaitForSeconds(waitTime);
                _isLunging = false;
            }
            else
            {
                // fire projectile and move
            }

            yield return null;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (_isLunging && (1 << other.gameObject.layer & layersToDamage.value) != 0 && other.gameObject.TryGetComponent(out LivingEntity entity))
            {
                // we hit a target while lunging
                entity.Damage(lungeDamage);
            }
        }
    }
}
