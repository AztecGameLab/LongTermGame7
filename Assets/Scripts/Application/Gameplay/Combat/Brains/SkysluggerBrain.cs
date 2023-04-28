using Application.Core.Abstraction;
using Application.Core.Utility;
using Application.Gameplay.Combat.Actions;
using ElRaccoone.Tweens;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Application.Gameplay.Combat.Brains
{
    public class SkysluggerBrain : MonsterBrain
    {
        public enum State { Charging, Attacking }

        public State currentState = State.Charging;
        public float stopDistance = 1;
        public float attackDamage = 1;
        public float attackRadius = 1;
        public float knockbackStrength = 1;
        public float knockbackUpward = 1;
        public DamageSource damageSource;
        public LayerMask attackMask;
        public BigAttackPreparation prepFx;
        public BigAttackEffect attackFx;
        public Rigidbody rigidbody;

        private Collider[] _hitBuffer = new Collider[20];

        private void Start()
        {
            prepFx.Initialize(transform);
        }

        private void Update()
        {
            rigidbody.velocity = Vector3.MoveTowards(rigidbody.velocity, Vector3.zero, 15 * Time.deltaTime);
            prepFx.Update();
        }

        protected override IEnumerator MakeDecision(BattleController controller)
        {
            var target = controller.PlayerTeam.GetClosest(transform.position, out float distance);

            switch (currentState)
            {
                case State.Charging:
                    // Move to the target
                    yield return transform.PathFindTo(target.transform.position, stopDistance: stopDistance);
                    // Start the charge up attack fx
                    prepFx.IsPlaying = true;
                    currentState = State.Attacking;
                    break;
                case State.Attacking:
                    {
                        // Damage all stuff in the attack zone
                        // Play damage fx
                        using var _ = controller.TemporaryFollow(transform);
                        yield return new WaitForSeconds(1);
                        Vector3 attackPosition = transform.position + Vector3.ClampMagnitude(target.transform.position - transform.position, attackRadius);
                        attackFx.Play(attackPosition);
                        prepFx.IsPlaying = false;
                        currentState = State.Charging;
                        int hits = Scanner.GetAllInSphere(attackPosition, attackRadius, attackMask, _hitBuffer);

                        for (int i = 0; i < hits; i++)
                        {
                            if (_hitBuffer[i].gameObject != gameObject)
                            {
                                if (_hitBuffer[i].TryGetComponent(out LivingEntity entity))
                                {
                                    damageSource.DealDamageTo(entity, attackDamage);
                                }

                                if (_hitBuffer[i].TryGetComponent(out IPhysicsComponent physics))
                                {
                                    physics.Velocity = ProjectileMotion.GetExplosionVelocity(transform.position,
                                        target.transform.position, knockbackStrength, knockbackUpward);
                                }
                            }
                        }
                        yield return new WaitForSeconds(1);
                        break;
                    }
            }
        }
    }
}
