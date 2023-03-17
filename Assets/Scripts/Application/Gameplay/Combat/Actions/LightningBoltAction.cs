using UnityEngine.AddressableAssets;

namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Core.Utility;
    using ImGuiNET;
    using UI.Indicators;
    using UnityEngine;

    /// <summary>
    /// The action where an entity moves around, in a grounded manner, on the battlefield.
    /// </summary>
    [Serializable]
    public class LightningBoltAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private int actionPointCost = 4;

        [SerializeField]
        private float maxRange = 5f;

        [SerializeField]
        private float damage = 5f;

        [SerializeField]
        private string lightningAssetPath;

        private Vector3 _targetPosition;
        private GameObject _targetEnemy;
        private IPooledObject<RangeIndicator> _rangeIndicator;
        private IPooledObject<ValidityIndicator> _validityIndicator;
        private AimSystem _aimSystem = new AimSystem();
        private ActionPointTracker _actionPointTracker;

        /// <inheritdoc/>
        public override string Name => "Lightning Bolt";

        /// <inheritdoc/>
        public override string Description => "Summon a Lightning Bolt dealing damage to an enemy.";

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();

            // Setting up range indicator
            _rangeIndicator = Services.IndicatorFactory.Borrow<RangeIndicator>();
            _rangeIndicator.Instance.Range = maxRange;
            DisposeOnExit(_rangeIndicator);

             // Setting up validity indicator
            _validityIndicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
            _validityIndicator.Instance.IsValid = false;
            DisposeOnExit(_validityIndicator);

            _aimSystem.Initialize(Camera.main);
            _actionPointTracker = User.GetComponent<ActionPointTracker>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            _targetPosition = _aimSystem.Update().point;
            Vector3 userPos = User.transform.position;
            GameObject closestEnemy = Controller.EnemyTeam.GetClosest(_targetPosition, out float _);

            _rangeIndicator.Instance.transform.position = userPos;
            _validityIndicator.Instance.transform.position = closestEnemy.transform.position;

            // Check if the target is in range and if we can afford the action
            float distance = Vector3.Distance(closestEnemy.transform.position, userPos);

            if (distance <= maxRange && _actionPointTracker.CanAfford(actionPointCost))
            {
                _validityIndicator.Instance.IsValid = true;
                _targetEnemy = closestEnemy;
                IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
            }
            else
            {
                _validityIndicator.Instance.IsValid = false;
            }
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Text($"Position: {_targetPosition}");
            ImGui.Text($"Action Point Cost: {actionPointCost}");
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(actionPointCost);
            }

            // Play the lightning particle
            var instance = Addressables.InstantiateAsync(lightningAssetPath).WaitForCompletion().GetComponent<ParticleSystem>();
            instance.transform.position = _targetPosition + new Vector3(0, 3, 0);
            instance.Play();

            // Do the damage
            _targetEnemy.TryGetComponent(out LivingEntity health);
            health.Damage(damage);

            yield return null;
        }
    }
}
