namespace Application.Gameplay.Combat.Actions
{
    using System;
    using System.Collections;
    using Core;
    using ImGuiNET;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// The action where an entity moves around, in a grounded manner, on the battlefield.
    /// </summary>
    [Serializable]
    public class MoveAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private float actionPointsPerUnit = 0.25f;

        [SerializeField]
        private float moveSpeed = 1;

        private Vector3 _targetPosition;
        private float _distance;
        private PathIndicator _pathIndicator;
        private NavMeshPath _path;
        private AimSystem _aimSystem = new AimSystem();
        private ActionPointTracker _actionPointTracker;
        private IDisposable _pathDisposable;

        /// <inheritdoc/>
        public override string Name => "Move";

        /// <inheritdoc/>
        public override string Description => "Move the monster to a different location.";

        private int PointCost => (int)Mathf.Ceil(_distance * actionPointsPerUnit);

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _pathDisposable = Services.IndicatorFactory.GetPathIndicator(out _pathIndicator);
            _path ??= new NavMeshPath();
            _aimSystem.Initialize(Camera.main);
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
                _pathIndicator.RenderPath(_path.corners);

                bool canAfford = _actionPointTracker.CanAfford(PointCost);

                IsPrepFinished |= canAfford && Input.GetKeyDown(KeyCode.Mouse0);
                _pathIndicator.IsValid = canAfford;
            }
            else
            {
                _pathIndicator.RenderPath(new[] { User.transform.position, _targetPosition });
                _pathIndicator.IsValid = false;
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
            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(PointCost);
            }

            float elapsedDistance = 0;
            NavMeshPath inProgressPath = new NavMeshPath();

            while (elapsedDistance < _distance)
            {
                elapsedDistance += Time.deltaTime * moveSpeed;
                User.transform.MoveTo(_path, elapsedDistance);
                NavMesh.CalculatePath(User.transform.position, _targetPosition, NavMesh.AllAreas, inProgressPath);
                _pathIndicator.RenderPath(inProgressPath.corners);
                yield return null;
            }

            _pathIndicator.ClearPath();
            _pathDisposable.Dispose();
        }
    }
}
