using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace Application.Gameplay.Combat.Effects
{
    public class EffectApplier
    {
        private readonly BattleController _controller;

        public EffectApplier(BattleController battleController)
        {
            _controller = battleController;
        }

        public void ApplyBurning(GameObject target, BurningSettings settings)
        {
            if (target.TryGetComponent(out BurningEffect effect))
            {
                effect.remainingRounds = Mathf.Max(settings.roundDuration, effect.remainingRounds);
                effect.damage = settings.damagePerRound;
            }
            else
            {
                var instance = target.AddComponent<BurningEffect>();
                instance.Initialize(_controller, settings);
            }
        }
    }

    public interface IBattleEffect
    {
        void Initialize(BattleController controller);
    }

    [Serializable]
    public class BurningSettings
    {
        public int roundDuration;
        public float damagePerRound;
        public AssetReference fxPrefab = new AssetReference("particles-burning-default");
    }

    public class BurningEffect : MonoBehaviour
    {
        public int remainingRounds;
        public float damage;

        public void Initialize(BattleController controller, BurningSettings settings)
        {
            var livingEntity = GetComponent<LivingEntity>();
            _fxInstance = settings.fxPrefab.InstantiateAsync(livingEntity.transform).WaitForCompletion().gameObject;
            remainingRounds = settings.roundDuration;
            damage = settings.damagePerRound;
            var disposable = controller.Round.RoundNumber
                .Skip(1)
                .Subscribe(_ => controller.Interrupts.Enqueue(() => ApplyBurnDamage(livingEntity, settings, controller)));

            // Three cases when an effect should stop applying:
            // 1) This effect is destroyed (some kind of heal, ect.)
            disposable.AddTo(this);
            // 2) When the battle ends (no context of rounds anymore)
            controller.OnBattleEnd.Subscribe(_ => disposable.Dispose());
            // 3) When this entity dies (effect loses meaning)
            livingEntity.OnDeath.Subscribe(_ => disposable.Dispose());
        }

        private GameObject _fxInstance;

        private IEnumerator ApplyBurnDamage(LivingEntity entity, BurningSettings settings, BattleController controller)
        {
            if (remainingRounds > 0)
            {
                // todo: status vfx / sfx, message
                using IDisposable followHandle = controller.TemporaryFollow(entity.transform);
                yield return new WaitForSeconds(1f);
                entity.Damage(damage);
                remainingRounds--;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                // todo: fire going out animation
                Destroy(_fxInstance);
                Destroy(this);
            }
        }
    }
}
