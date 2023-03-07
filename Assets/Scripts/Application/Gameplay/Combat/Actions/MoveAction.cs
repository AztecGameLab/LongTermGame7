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
    public class MoveAction : BattleAction, IDebugImGui
    {
        [SerializeField]
        private float actionPointsPerUnit = 0.25f;

        [SerializeField]
        private GameObject targetPrefab;

        [SerializeField]
        private float moveSpeed = 1;

        private Camera _camera;
        private Vector3 _targetPosition;
        private float _distance;
        private GameObject _targetInstance;
        private NavMeshPath _path;

        /// <inheritdoc/>
        public override string Name => "Move";

        /// <inheritdoc/>
        public override string Description => "Move the monster to a different location.";

        private int PointCost => (int)Mathf.Ceil(_distance * actionPointsPerUnit);

        /// <inheritdoc/>
        public override void PrepEnter()
        {
            base.PrepEnter();
            _camera = Camera.main;
            _targetInstance = Object.Instantiate(targetPrefab);
            _path ??= new NavMeshPath();
        }

        /// <inheritdoc/>
        public override void PrepTick()
        {
            base.PrepTick();
            Ray clickRay = _camera.ScreenPointToRay(Input.mousePosition);
            Vector3 position = User.transform.position;

            if (Physics.Raycast(clickRay, out var hitInfo) && NavMesh.CalculatePath(position, hitInfo.point, NavMesh.AllAreas, _path))
            {
                _targetPosition = hitInfo.point;
                _distance = NavMeshPathUtil.CalculateDistance(_path);

                if (User.TryGetComponent(out ActionPointTracker tracker) &&
                    PointCost > tracker.remainingActionPoints)
                {
                    _distance = Mathf.Min(tracker.remainingActionPoints * (1 / actionPointsPerUnit), _distance);
                    _targetPosition = NavMeshPathUtil.GetPositionAtDistance(_path, _distance);
                }

                _targetInstance.transform.position = _targetPosition;
            }

            IsPrepFinished |= Input.GetKeyDown(KeyCode.Mouse0);
        }

        /// <inheritdoc/>
        public override void PrepExit()
        {
            base.PrepExit();
            Object.Destroy(_targetInstance);
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
                tracker.remainingActionPoints -= PointCost;
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
