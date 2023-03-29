namespace Application.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Moves a group of transforms over time to follow another transform.
    /// </summary>
    public class GroupFollowTarget
    {
        private const int MaxFollowGroup = 10;

        private readonly float _followSpacing;
        private readonly Vector3[] _targetPositions = new Vector3[MaxFollowGroup];
        private readonly List<Transform> _groupMembers = new List<Transform>();

        private float _elapsedDistance;
        private Vector3 _previousPosition;
        private int _headIndex;
        private Transform _target;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupFollowTarget"/> class.
        /// </summary>
        /// <param name="spacing">The distance between each group member.</param>
        public GroupFollowTarget(float spacing = 3)
        {
            _followSpacing = spacing;
        }

        /// <summary>
        /// Gets a list of the group members following the target.
        /// </summary>
        public IList<Transform> GroupMembers => _groupMembers;

        /// <summary>
        /// Gets or sets the transform that each group member is following.
        /// </summary>
        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                _previousPosition = _target.position;

                for (int i = 0; i < _targetPositions.Length; i++)
                {
                    _targetPositions[i] = _previousPosition;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the group transforms should be updated.
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Updates each member of the group to move behind the target.
        /// </summary>
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
                    moveDelta * 0.9f);
            }

            if (_elapsedDistance >= _followSpacing)
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
