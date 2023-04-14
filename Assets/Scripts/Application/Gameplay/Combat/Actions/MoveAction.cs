namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using Core.Utility;
    using ImGuiNET;
    using Newtonsoft.Json;
    using UI.Indicators;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// The action where an entity moves around, in a grounded manner, on the battlefield.
    /// </summary>
    [Serializable]
    [JsonObject(MemberSerialization.OptIn)]
    public class MoveAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        [JsonProperty]
        private float actionPointsPerUnit = 0.25f;

        [SerializeField]
        [JsonProperty]
        private float moveSpeed = 7;

        private Vector3 _targetPosition;
        private float _distance;
        private IPooledObject<PathIndicator> _pathIndicator;
        private NavMeshPath _path;
        private AimSystem _aimSystem = new AimSystem();
        private ActionPointTracker _actionPointTracker;

        /// <inheritdoc/>
        public override string Name => "Move";

        /// <inheritdoc/>
        public override string Description => "Move the monster to a different location.";

        private int PointCost => (int)Mathf.Ceil(_distance * actionPointsPerUnit);

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _pathIndicator = Services.IndicatorFactory.Borrow<PathIndicator>();
            _path ??= new NavMeshPath();
            _aimSystem.Initialize();
            _actionPointTracker = User.GetComponent<ActionPointTracker>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            _targetPosition = _aimSystem.Update().point;

            if (NavMesh.CalculatePath(User.transform.position, _targetPosition, NavMesh.AllAreas, _path))
            {
                _distance = NavMeshPathUtil.CalculateDistance(_path);
                _pathIndicator.Instance.RenderPath(_path.corners);

                bool canAfford = _actionPointTracker.CanAfford(PointCost);

                IsPrepFinished |= canAfford && Input.GetKeyDown(KeyCode.Mouse0) && canAfford;
                _pathIndicator.Instance.IsValid = canAfford;
            }
            else
            {
                _pathIndicator.Instance.RenderPath(new[] { User.transform.position, _targetPosition });
                _pathIndicator.Instance.IsValid = false;
            }
        }

        /// <inheritdoc/>
        public override void PrepExit()
        {
            base.PrepExit();

            if (!IsPrepFinished)
            {
                _pathIndicator.Dispose();
            }
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Text($"Position: {_targetPosition}");
            ImGui.Text($"Distance: {_distance}");
            ImGui.Text($"Action Point Cost: {_distance:0} * {actionPointsPerUnit} = {(int)Mathf.Ceil(_distance * actionPointsPerUnit)}");
        }

        /// <inheritdoc/>
        protected override IEnumerator Execute()
        {
            ActionTracker.Spend(PointCost);
            yield return User.transform.PathFindTo(_targetPosition, moveSpeed, 0, _distance, _pathIndicator);
            _pathIndicator.Instance.ClearPath();
            _pathIndicator.Dispose();
        }
    }
}
