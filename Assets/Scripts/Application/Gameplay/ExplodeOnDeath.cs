namespace Application.Gameplay
{
    using Combat;
    using Core.Abstraction;
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(LivingEntity))]
    public class ExplodeOnDeath : MonoBehaviour
    {
        private readonly Collider[] _colliderBuffer = new Collider[25];

        [SerializeField]
        private LayerMask layersToDamage;

        [SerializeField]
        private float explosionRadius = 5;

        [SerializeField]
        private float explosionDamage = 1;

        [SerializeField]
        private float explosionKnockback = 1;

        [SerializeField]
        private float explosionUpBoost = 1;

        private void Awake()
        {
            var entity = GetComponent<LivingEntity>();
            entity.OnDeath.Subscribe(_ => Explode()).AddTo(this);
        }

        private void Explode()
        {
            Vector3 source = transform.position;
            int hits = Physics.OverlapSphereNonAlloc(source, explosionRadius, _colliderBuffer, layersToDamage.value);

            for (int i = 0; i < hits; i++)
            {
                Collider targetCollider = _colliderBuffer[i];
                Vector3 target = targetCollider.transform.position;
                var falloff = 1 - Mathf.Clamp01(Vector3.Distance(source, target) / explosionRadius);

                if (targetCollider.TryGetComponent(out LivingEntity entity))
                {
                    entity.Damage(explosionDamage * falloff);
                }

                if (targetCollider.TryGetComponent(out IPhysicsComponent physics))
                {
                    var explosionVelocity = ProjectileMotion.GetExplosionVelocity(source, target, explosionKnockback, explosionUpBoost);
                    physics.Velocity = explosionVelocity * falloff;
                }
            }
        }
    }
}
