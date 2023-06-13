namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Core.Abstraction;
    using UI.Indicators;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// An action that pushes monsters in a certain direction.
    /// </summary>
    [Serializable]
    public class WindPush : BattleAction
    {
        private readonly AimSystem _aimSystem = new AimSystem();

        [SerializeField]
        private int cost;

        [SerializeField]
        private float duration = 1;

        [SerializeField]
        private float strength = 5;

        private Vector3 _direction;

        private IPooledObject<ValidityIndicator> _indicator;

        /// <inheritdoc/>
        public override string Name => "Wind Push";

        /// <inheritdoc/>
        public override string Description => "Push with wind.";

        /// <inheritdoc/>
        public override int Cost => cost;

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
            _direction = _aimSystem.Update().point;
            _indicator.Instance.transform.position = _direction;
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
            PhysicsComponent[] physicsComponents = Object.FindObjectsOfType<PhysicsComponent>();
            float elapsed = 0;
            Vector3 pushDirection = (_direction - User.transform.position).normalized;

            while (elapsed < duration)
            {
                foreach (PhysicsComponent component in physicsComponents)
                {
                    component.Velocity += pushDirection * strength;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}
