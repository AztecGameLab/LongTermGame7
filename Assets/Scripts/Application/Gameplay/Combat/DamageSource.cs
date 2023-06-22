using Application.Core;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Application.Gameplay.Combat.Actions
{
    public class DamageSource : MonoBehaviour
    {
        public UnityEvent<LivingEntity> dealtDamage;

        public void DealDamageTo(LivingEntity entity, float amount)
        {
            entity.Damage(amount);
            dealtDamage.Invoke(entity);
        }
    }
}
