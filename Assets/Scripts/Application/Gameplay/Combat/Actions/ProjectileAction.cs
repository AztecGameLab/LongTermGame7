namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Application.Core;
    using ImGuiNET;
    using UnityEngine;

    /// <summary>
    /// A template for an action which fires a projectile at a target.
    /// </summary>
    [Serializable]
    public class ProjectileAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private string description;

        [SerializeField]
        private int actionPointCost;

        [SerializeField]
        private int damage;

        [SerializeField]
        private float _projectileVelocity;

        [SerializeField]
        private GameObject projectilePrefab;

        private Vector3 _targetPosition;
        private AimSystem _aimSystem = new AimSystem();
        private ActionPointTracker _actionPointTracker;
        private GameObject _projectileTarget;

        /// <inheritdoc/>
        public override string Name => name;

        /// <inheritdoc/>
        public override string Description => description;

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _aimSystem.Initialize();
            _actionPointTracker = User.GetComponent<ActionPointTracker>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            _targetPosition = _aimSystem.Update().point;
            OnClickTarget();
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            IsPrepFinished |= ImGui.Button("Lock in demo action");
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            Debug.Log("Executing projectile action...");

            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(actionPointCost);
            }

            // Use AimSystem to find and set target
            
            // Instantiate a projectile prefab
            
            // Move prefab to target

            // Delete prefab

            // Process damage to target

            // Should wait until target has been hit!

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }

        private void OnClickTarget()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Collider collider = _aimSystem.Update().collider;
                Debug.Log("Collider: " + collider.name + " hit!");
            }
        }
    }
}
