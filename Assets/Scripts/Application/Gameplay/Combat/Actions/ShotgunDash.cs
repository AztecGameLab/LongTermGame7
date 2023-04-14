namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Core.Utility;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using Newtonsoft.Json;
    using UI.Indicators;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// Copy and paste this file to quickly get started with a new BattleAction.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ShotgunDash : BattleAction
    {
        private readonly Collider[] _resultBuffer = new Collider[25];

        [SerializeField]
        [JsonProperty]
        private string name = "Burst Dash";

        [SerializeField]
        [JsonProperty]
        private string description = "Launch forward and deal damage to everything behind you.";

        [SerializeField]
        [JsonProperty]
        private float distance = 3;

        [SerializeField]
        private int apCost = 6;

        [SerializeField]
        [JsonProperty]
        private float damage = 3;

        [SerializeField]
        [JsonProperty]
        private float range = 2;

        [SerializeField]
        [JsonProperty]
        private float spread = 180;

        [SerializeField]
        [JsonProperty]
        private AssetReference dashEffect;

        [SerializeField]
        [JsonProperty]
        private LayerMask damageMask;

        [SerializeField]
        [JsonProperty]
        private float moveDuration = 0.25f;

        [SerializeField]
        [JsonProperty]
        private EaseType moveEasing = EaseType.Linear;

        private IPooledObject<ArrowIndicator> _arrowIndicator;
        private IPooledObject<SliceIndicator> _sliceIndicator;
        private AimSystem _aimSystem = new AimSystem();
        private Vector3 _targetPosition;

        /// <inheritdoc/>
        public override string Name => name;

        /// <inheritdoc/>
        public override string Description => description;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize();

            // Replace this with whatever custom indicator you need.
            _arrowIndicator = Services.IndicatorFactory.Borrow<ArrowIndicator>();
            _sliceIndicator = Services.IndicatorFactory.Borrow<SliceIndicator>();
            DisposeOnExit(_arrowIndicator);
            DisposeOnExit(_sliceIndicator);
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            var aimData = _aimSystem.Update();
            var origin = User.transform.position;
            var direction = (aimData.point - origin).normalized;
            _targetPosition = Vector3.ClampMagnitude(aimData.point - origin, distance) + origin;

            _arrowIndicator.Instance.UpdateView(origin, _targetPosition);
            _sliceIndicator.Instance.UpdateView(origin, -direction, range, spread);

            // By default, lock in by left-clicking. You may want a different method.
            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0) && ActionTracker.CanAfford(apCost);
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            ActionTracker.Spend(apCost);
            yield return new WaitForSeconds(0.1f);
            var origin = User.transform.position;
            var direction = _targetPosition - origin;
            direction.y = 0;

            // Visual effects for dashing.
            dashEffect.InstantiateAsync(origin, Quaternion.identity);

            int hits = Scanner.GetAllInSlice(origin, -direction, spread, range, damageMask, _resultBuffer);

            for (int i = 0; i < hits; i++)
            {
                if (_resultBuffer[i].TryGetComponentParents(out LivingEntity entity))
                {
                    entity.Damage(damage);
                }
            }

            yield return User.transform.TweenPosition(_targetPosition, moveDuration).SetEase(moveEasing).Await();
            yield return new WaitForSeconds(1f);
        }
    }
}
