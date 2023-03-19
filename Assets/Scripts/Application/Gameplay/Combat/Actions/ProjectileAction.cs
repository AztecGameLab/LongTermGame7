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
        private AssetReferenceGameObject projectileAsset;

        [SerializeField]
        [JsonProperty]
        private float projectileTime = 1;

        private AimSystem _aimSystem = new AimSystem();
        private IPooledObject<ValidityIndicator> _indicator;
        private Vector3 _targetPosition;

        /// <inheritdoc/>
        public override string Name => "Projectile";

        /// <inheritdoc/>
        public override string Description => "Launches a projectile";

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize(groundSnap: false);
            _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
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
        public override void PrepExit()
        {
            base.PrepExit();
            _indicator.Dispose();
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            Vector3 origin = User.transform.position + Vector3.up;
            var projectileInstance = projectileAsset.InstantiateAsync(origin, Quaternion.identity).WaitForCompletion();

            if (projectileInstance.TryGetComponent(out Rigidbody rigidbody))
            {
                var launchVelocity = ProjectileMotion.GetLaunchVelocity(origin, _targetPosition, projectileTime);
                rigidbody.velocity = launchVelocity;
                yield return new WaitForSeconds(projectileTime);
            }
        }
    }
}
