using Application.Gameplay.Combat.Effects;
using UnityEngine;

namespace Application.Gameplay.Combat.Actions
{
    public class ApplyBurnOnDamage : MonoBehaviour
    {
        public BurningSettings burningSettings;
        public DamageSource damageSource;

        private void Start()
        {
            if (damageSource != null || TryGetComponent(out damageSource))
            {
                damageSource.dealtDamage.AddListener(HandleDealtDamage);
            }
        }

        private void HandleDealtDamage(LivingEntity target)
        {
            FindObjectOfType<BattleController>()?.EffectApplier.ApplyBurning(target.gameObject, burningSettings);
        }
    }
}