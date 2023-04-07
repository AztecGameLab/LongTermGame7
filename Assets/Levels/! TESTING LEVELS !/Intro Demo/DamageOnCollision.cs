using Application.Gameplay;
using UniRx;
using UnityEngine;

namespace Levels.__TESTING_LEVELS__.Real_Demo
{
    [RequireComponent(typeof(Projectile))]
    public class DamageOnCollision : MonoBehaviour
    {
        public float amount = 1;
        
        private void Awake()
        {
            GetComponent<Projectile>().ObserveObjectHit().Subscribe(obj =>
            {
                if (obj.TryGetComponent(out LivingEntity entity))
                {
                    entity.Damage(amount);
                }
            });
        }
    }
}