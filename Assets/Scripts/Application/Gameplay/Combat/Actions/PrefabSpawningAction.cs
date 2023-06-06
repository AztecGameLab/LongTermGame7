using UnityEngine.AddressableAssets;

namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Newtonsoft.Json;
    using UI.Indicators;
    using UnityEngine;

    /// <summary>
    /// Copy and paste this file to quickly get started with a new BattleAction.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class PrefabSpawningAction : BattleAction
    {
        [SerializeField]
        [JsonProperty]
        private string name;

        [SerializeField]
        [JsonProperty]
        private string description;

        [SerializeField]
        [JsonProperty]
        private AssetReference prefabAsset;

        [SerializeField]
        [JsonProperty]
        private int apCost = 1;

        [SerializeField]
        [JsonProperty]
        private float maxRange = 5;

        [SerializeField]
        [JsonProperty]
        private float waitTime = 1;

        private IPooledObject<ValidityIndicator> _indicator;
        private AimSystem _aimSystem = new AimSystem();

        /// <inheritdoc/>
        public override string Name => name;

        /// <inheritdoc/>
        public override string Description => description;

        /// <inheritdoc/>
        public override int Cost => apCost;

        private Vector3 _targetPosition;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize();

            // Replace this with whatever custom indicator you need.
            _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
            DisposeOnExit(_indicator);
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            var aimData = _aimSystem.Update();
            _targetPosition = aimData.point;
            _indicator.Instance.transform.position = _targetPosition;
            bool isValid = (_targetPosition - User.transform.position).sqrMagnitude <= maxRange * maxRange;
            _indicator.Instance.IsValid = isValid;

            // By default, lock in by left-clicking. You may want a different method.
            IsPrepFinished |= isValid && ActionTracker.CanAfford(apCost) && Input.GetKeyDown(KeyCode.Mouse0);
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            // Put execution logic here.
            ActionTracker.Spend(apCost);
            var instance = prefabAsset.InstantiateAsync(_targetPosition, Quaternion.identity) .WaitForCompletion();
            using IDisposable disposable = Controller.TemporaryFollow(instance.transform);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
