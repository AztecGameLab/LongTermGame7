using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat.Brains
{
    public class ProtectorEnemy : MonoBehaviour
    {
        public GameObject shieldPrefab;
        public LivingEntity entity;
        public LivingEntity[] targets;

        private Dictionary<LivingEntity, GameObject> _entitiesToShields
            = new Dictionary<LivingEntity, GameObject>();

        private void Start()
        {
            entity.OnDeath.Subscribe(_ => BreakShields()).AddTo(this);
            AddShields();
        }

        private void AddShield(LivingEntity target)
        {
            target.IsInvincible = true;
            var instance = Instantiate(shieldPrefab, target.transform);
            _entitiesToShields.Add(target, instance);
        }

        private void BreakShield(LivingEntity target)
        {
            target.IsInvincible = false;
            var instance = _entitiesToShields[target];
            Destroy(instance);
        }

        private void AddShields()
        {
            foreach (LivingEntity target in targets)
                AddShield(target);
        }

        private void BreakShields()
        {
            foreach (LivingEntity target in targets)
                BreakShield(target);
        }
    }
}
