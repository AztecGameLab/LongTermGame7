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
    public class Dash : BattleAction
    {
        [SerializeField]
        [JsonProperty]
        private string name = "Dash";

        [SerializeField]
        [JsonProperty]
        private string description = "You dash forward, not stepping on anything in the stage.";

        [SerializeField]
        [JsonProperty]
        private float distance = 3;

        [SerializeField]
        private int apCost = 6;

        [SerializeField]
        [JsonProperty]
        private AssetReference dashEffect;

        [SerializeField]
        [JsonProperty]
        private float moveDuration = 0.25f;

        [SerializeField]
        [JsonProperty]
        private EaseType moveEasing = EaseType.Linear;

        private IPooledObject<ArrowIndicator> _arrowIndicator;
        private AimSystem _aimSystem = new AimSystem();
        private Vector3 _targetPosition;

        /// <inheritdoc/>
        public override string Name => name;

        /// <inheritdoc/>
        public override string Description => description;

        /// <inheritdoc/>
        public override int Cost => apCost;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize();

            // Replace this with whatever custom indicator you need.
            _arrowIndicator = Services.IndicatorFactory.Borrow<ArrowIndicator>();
            DisposeOnExit(_arrowIndicator);
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
            yield return User.transform.TweenPosition(_targetPosition, moveDuration).SetEase(moveEasing).Await();
            yield return new WaitForSeconds(1f);
        }
    }
}
