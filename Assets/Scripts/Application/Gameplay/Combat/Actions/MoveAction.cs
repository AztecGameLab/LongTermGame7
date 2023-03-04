using Application.Core;
using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Combat
{
    [Serializable]
    public class MoveAction : BattleAction, IDebugImGui
    {
        public override string Name => "Move";
        public override string Description => "Move the monster to a different location.";

        [SerializeField] 
        private float actionPointsPerUnit = 0.25f;

        [SerializeField] 
        private GameObject targetPrefab;

        private Camera _camera;
        private Vector3 _targetPosition;
        private float _distance;
        private GameObject _targetInstance;

        private int PointCost => (int)Mathf.Ceil(_distance * actionPointsPerUnit);
        
        protected override IEnumerator Execute()
        {
            User.transform.position = _targetPosition;

            if (User.TryGetComponent(out ActionPointTracker tracker))
                tracker.remainingActionPoints -= PointCost;
            
            yield return null;
        }

        public override void PrepEnter()
        {
            _camera = Camera.main;
            _targetInstance = Object.Instantiate(targetPrefab);
        }

        public override bool PrepTick()
        {
            var clickRay = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(clickRay, out var hitInfo))
            {
                _targetPosition = hitInfo.point;
                _distance = Vector3.Distance(User.transform.position, _targetPosition);

                if (User.TryGetComponent(out ActionPointTracker tracker) && PointCost > tracker.remainingActionPoints)
                {
                    _distance = Mathf.Min(tracker.remainingActionPoints * (1 / actionPointsPerUnit), _distance);
                    _targetPosition = User.transform.position + (_targetPosition - User.transform.position).normalized * _distance;
                }
                
                _targetInstance.transform.position = _targetPosition;
            }

            return Input.GetKeyDown(KeyCode.Mouse0);
        }

        public override void PrepExit()
        {
            Object.Destroy(_targetInstance);
        }

        public void RenderImGui()
        {
            ImGui.Text($"Position: {_targetPosition}");
            ImGui.Text($"Distance: {_distance}");
            ImGui.Text($"Action Point Cost: {_distance:0} * {actionPointsPerUnit} = {(int) Mathf.Ceil(_distance * actionPointsPerUnit)}");
        }
    }
}