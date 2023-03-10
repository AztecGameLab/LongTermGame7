namespace Application.Gameplay
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Moves a group of transforms over time to follow another transform.
    /// </summary>
    public class GroupFollowTarget
    {
        [SerializeField]
        private float followSpacing = 2;

        private float _elapsedDistance;
        private Vector3 _previousPosition;
        private Vector3[] _targetPositions = new Vector3[10];
        private int _headIndex;
        private ReactiveCollection<Transform> _groupMembers = new ReactiveCollection<Transform>();

        public IList<Transform> GroupMembers => _groupMembers;

        public Transform Target { get; set; }

        public bool Enabled { get; set; } = true;

        public GroupFollowTarget(float spacing = 3)
        {
            followSpacing = spacing;
        }

        public void Tick()
        {
            if (Target == null || GroupMembers.Count <= 0 || !Enabled)
            {
                return;
            }

            Vector3 currentPosition = Target.position;
            float moveDelta = Vector3.Distance(currentPosition, _previousPosition);

            for (int i = 0; i < GroupMembers.Count; i++)
            {
                GroupMembers[i].position = Vector3.MoveTowards(
                    GroupMembers[i].position,
                    _targetPositions[(_headIndex + i) % GroupMembers.Count],
                    moveDelta);
            }

            if (_elapsedDistance >= followSpacing)
            {
                // we want to add a new position
                _elapsedDistance = 0;
                _headIndex = (_headIndex + 1) % GroupMembers.Count;
                _targetPositions[_headIndex] = currentPosition;
            }

            _elapsedDistance += moveDelta;
            _previousPosition = currentPosition;
        }
    }
}
