namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using ImGuiNET;
    using UI.Indicators;
    using UnityEngine;

    [Serializable]
    public class RockPlatformStairsAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private int actionPointCost = 4;
        private float maxRange = 5f; // max range the stairs can be placed away from the player
        private float stairsHeight = 1.5f;
        private float stairsWidth = 3f;
        private Vector3 _targetPosition; // pos of stairs and new pos of player w/o new hight

        private IPooledObject<RangeIndicator> _rangeIndicator;
        private IPooledObject<ValidityIndicator> _validityIndicator;
        private AimSystem _aimSystem = new AimSystem();
        private ActionPointTracker _actionPointTracker;

        public GameObject stairsPrefab; // object for stairs
        public GameObject stairs;

        public override string Name => "Rock Stairs";
        public override string Description => "Summon stairs and move up them";

        public override void PrepEnter()
        {
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

        public override void PrepTick()
        {
            // Find mouse possition
            _targetPosition = _aimSystem.Update().point;

            // Check if the stair placement will be in range/the space is not occupied
            float distance = Vector3.Distance(_targetPosition, User.transform.position);

            if (distance <= maxRange && !Physics.CheckSphere(_targetPosition, stairsWidth)
                && _actionPointTracker.CanAfford(actionPointCost))
            {
                _validityIndicator.Instance.IsValid = true;
                IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
            }
            else
            {
                _validityIndicator.Instance.IsValid = false;
            }
        }

        public void RenderImGui()
        {
            ImGui.Text($"Choose Place of the Stairs");
            ImGui.Text($"Position: {_targetPosition}");
            ImGui.Text($"Action Point Cost: {actionPointCost}");
        }

        protected override IEnumerator Execute()
        {
            // Remove action points
            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(actionPointCost);
            }

            // Summon stairs
            stairs = UnityEngine.Object.Instantiate(stairsPrefab);
            stairs.transform.position = _targetPosition;

            // Move up stairs
            _targetPosition = _targetPosition + new Vector3(0, stairsHeight, 0);
            User.transform.position = _targetPosition;

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }
    }
}
