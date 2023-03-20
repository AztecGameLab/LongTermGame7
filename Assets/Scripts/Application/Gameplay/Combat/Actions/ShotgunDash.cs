namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Core;
    using Core.Utility;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using Newtonsoft.Json;
    using UI.Indicators;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    // todo: arrow indicator and spread indicator

    /// <summary>
    /// Copy and paste this file to quickly get started with a new BattleAction.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ShotgunDash : BattleAction
    {
        [SerializeField]
        [JsonProperty]
        private string name = "Burst Dash";

        [SerializeField]
        [JsonProperty]
        private string description = "Launch forward and deal damage to everything behind you.";

        [SerializeField]
        private float distance = 3;

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
        private AssetReferenceGameObject dashEffect;

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
            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            yield return new WaitForSeconds(0.1f);
            dashEffect.InstantiateAsync(User.transform.position, Quaternion.identity).WaitForCompletion();
            var direction = _targetPosition - User.transform.position;
            direction.y = 0;

            foreach (Collider collider in GetAllInSlice(User.transform.position, -direction, spread, range, damageMask))
            {
                if (collider.TryGetComponentParents(out LivingEntity entity))
                {
                    entity.Damage(damage);
                }
            }

            yield return User.transform.TweenPosition(_targetPosition, moveDuration).SetEase(moveEasing).Await();
            yield return new WaitForSeconds(1f);
        }

        private List<Collider> GetAllInSlice(Vector3 origin, Vector3 direction, float s, float radius, LayerMask mask)
        {
            Collider[] colliders = Physics.OverlapSphere(origin, radius, mask.value);
            List<Collider> results = new List<Collider>();

            foreach (Collider collider in colliders)
            {
                var v = collider.transform.position - origin;
                v.y = 0;

                if (Vector3.Angle(direction, v) <= s)
                {
                    results.Add(collider);
                }
            }

            return results;
        }
    }
}
