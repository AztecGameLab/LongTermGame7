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
        private GameObject _targetInstance;
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
            DisposeOnExit(Services.IndicatorFactory.GetCube(out _targetInstance));
            _path ??= new NavMeshPath();
            _aimSystem.Initialize(Camera.main);
            _actionPointTracker = User.GetComponent<ActionPointTracker>();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            _targetPosition = _aimSystem.Update().point;
            _targetInstance.transform.position = _targetPosition;

            if (NavMesh.CalculatePath(User.transform.position, _targetPosition, NavMesh.AllAreas, _path))
            {
                _distance = NavMeshPathUtil.CalculateDistance(_path);

                if (_actionPointTracker.CanAfford(PointCost))
                {
                    IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
                }
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
            User.transform.position = _targetPosition;

            if (User.TryGetComponent(out ActionPointTracker tracker))
            {
                tracker.TrySpend(PointCost);
            }

            float elapsedDistance = 0;

            while (elapsedDistance < _distance)
            {
                elapsedDistance += Time.deltaTime * moveSpeed;
                User.transform.MoveTo(_path, elapsedDistance);
                yield return null;
            }
        }
    }
}
