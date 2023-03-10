using Application.Gameplay.Combat.UI;

namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using ImGuiNET;
    using UnityEngine;
    using UnityEngine.AI;
    using Object = UnityEngine.Object;

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
        private ParticleSystem lightning;

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
            _rangeIndicator = Services.IndicatorFactory.Borrow<RangeIndicator>();
            _rangeIndicator.Instance.Range = maxRange;
            _validityIndicator = Services.IndicatorFactory.Borrow<ValidityIndicator>();
            _validityIndicator.Instance.IsValid = false;
            _aimSystem.Initialize(Camera.main);
            _actionPointTracker = User.GetComponent<ActionPointTracker>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            Vector3 userPos = User.transform.position;
            _targetPosition = _aimSystem.Update().point;
            _rangeIndicator.Instance.transform.position = userPos;

            // Find the enemy closest to the aim location
            float closestDistance = float.MaxValue;
            GameObject closestEnemy = User; // Set to user temporarily
            foreach (GameObject enemy in Controller.EnemyTeam)
            {
                float thisDistance = Vector3.Distance(enemy.transform.position, _targetPosition);
                if (thisDistance < closestDistance)
                {
                    closestDistance = thisDistance;
                    closestEnemy = enemy;
                }
            }

            // Move the validity indicator to the closest enemy
            if (closestDistance <= float.MaxValue)
            {
                _validityIndicator.Instance.gameObject.SetActive(true);
                _validityIndicator.Instance.transform.position = closestEnemy.transform.position;
            }
            else
            {
                // Hide if no enemies
                _validityIndicator.Instance.gameObject.SetActive(false);
            }

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
            lightning.transform.position = _targetPosition + new Vector3(0, 3, 0);
            lightning.Play();

            // Do the damage
            _targetEnemy.TryGetComponent(out LivingEntity health);
            health.Damage(damage);

            _rangeIndicator.Dispose();
            _validityIndicator.Dispose();

            yield return null;
        }
    }
}
