namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Newtonsoft.Json;
    using UI.Indicators;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// An action that spawns a projectile.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class ProjectileAction : BattleAction
    {
        [SerializeField]
        [JsonProperty]
        private string actionName = "Projectile";

        [SerializeField]
        [JsonProperty]
        private string actionDescription = "Launches a projectile";

        [SerializeField]
        [JsonProperty]
        private AssetReference projectileAsset;

        [SerializeField]
        [JsonProperty]
        private float projectileTime = 1;

        private AimSystem _aimSystem = new AimSystem();
        private IPooledObject<ValidityIndicator> _indicator;
        private Vector3 _targetPosition;

        /// <inheritdoc/>
        public override string Name => actionName;

        /// <inheritdoc/>
        public override string Description => actionDescription;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize(groundSnap: false);
            _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
            DisposeOnExit(_indicator);
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            var aimInfo = _aimSystem.Update();
            _indicator.Instance.transform.position = aimInfo.point;
            _targetPosition = aimInfo.point;

            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            Vector3 origin = User.transform.position + Vector3.up;
            var launchVelocity = ProjectileMotion.GetLaunchVelocity(origin, _targetPosition, projectileTime);
            var projectileInstance = projectileAsset.InstantiateAsync(origin, Quaternion.LookRotation(launchVelocity)).WaitForCompletion();

            if (projectileInstance.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.velocity = launchVelocity;
                yield return new WaitForSeconds(projectileTime);
            }
        }
    }
}
