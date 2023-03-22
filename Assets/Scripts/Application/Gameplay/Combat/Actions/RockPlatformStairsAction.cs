namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using ImGuiNET;
    // using UI.Indicators;
    using UnityEngine;

    [Serializable]
    public class RockPlatformStairsAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private int actionPointCost = 4;
        private float maxRange = 5f; // max range the stairs can be placed away from the player
        private float stairsHeight = 1.5f;
        private float stairsWidth = 3f;
        private AimSystem _aimSystem = new AimSystem();
        private Vector3 _targetPosition; // pos of stairs and new pos of player w/o new hight

        private GameObject stairsPrefab; // object for stairs
        private GameObject stairs; // public??????

        public override string Name => "Rock Stairs";
        public override string Description => "Summon stairs and move up them";

        public override void PrepEnter()
        {
            _aimSystem.Initialize(Camera.main);
        }

        public override void PrepTick()
        {
            // Find mouse possition
            _targetPosition = _aimSystem.Update().point;

            // Check if the stair placement will be in range/the space is not occupied
            float distance = Vector3.Distance(_targetPosition, User.transform.position);

            if (distance <= maxRange && !Physics.CheckSphere(_targetPosition, stairsWidth))
            {
                IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
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
            // Remove action points and exit
            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(actionPointCost);
            }

            // Summon stairs
            stairs = Instantiate(stairsPrefab, _targetPosition);

            // Move up stairs
            _targetPosition = _targetPosition + new Vector3(0, stairsHeight, 0);
            User.transform.position = _targetPosition;

            yield return new WaitForSeconds(1);
            Debug.Log("Done!");
        }
    }
}
