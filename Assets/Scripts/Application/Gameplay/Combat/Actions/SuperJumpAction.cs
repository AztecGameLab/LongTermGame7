namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Core.Abstraction;
    using UI.Indicators;
    using UnityEngine;

    /// <summary>
    /// An action that launches the user to a desired position in the world.
    /// </summary>
    [Serializable]
    public class SuperJumpAction : BattleAction
    {
        private readonly AimSystem _aimSystem = new AimSystem();

        [SerializeField]
        private int apCost = 4;

        private Vector3 _landingSpot;
        private IPooledObject<ValidityIndicator> _indicator;

        /// <inheritdoc/>
        public override string Name => "Super Jump Anywhere";

        /// <inheritdoc/>
        public override string Description => "Jumps across the stage with ease";

        /// <inheritdoc/>
        public override int Cost => apCost;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepTick();
            _aimSystem.Initialize();
            _indicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            _landingSpot = _aimSystem.Update().point;
            _indicator.Instance.transform.position = _landingSpot;
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
            ActionTracker.Spend(apCost);
            var launchVelocity = ProjectileMotion.GetLaunchVelocity(User.transform.position, _landingSpot);
            PhysicsComponent physics = User.GetComponent<PhysicsComponent>();
            physics.Velocity = launchVelocity;
            yield return new WaitForSeconds(1);
        }
    }
}
