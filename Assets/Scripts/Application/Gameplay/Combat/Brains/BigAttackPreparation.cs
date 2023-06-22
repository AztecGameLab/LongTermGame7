using System;
using UnityEngine;

namespace Application.Gameplay.Combat.Brains
{
    [Serializable]
    public class BigAttackPreparation
    {
        [SerializeField] private float scaleAmount = 2;
        [SerializeField] private float scaleSpeed = 15;
        [SerializeField] private float shakeAmount = 1;
        [SerializeField] private float shakeSpeed = 1;

        private Transform _animatedTransform;
        private Transform _targetTransform;
        private Vector3 _baseScale;

        public bool IsPlaying { get; set; } = false;
        public Vector3 TargetScale => IsPlaying ? _baseScale * scaleAmount : _baseScale;

        public Vector3 TargetOffset => IsPlaying
            ? new Vector3(GetNoise(1), GetNoise(2))
            : Vector3.zero;

        private float GetNoise(float offset)
        {
            return (Mathf.PerlinNoise((Time.time + offset) * shakeSpeed, 0) - 0.5f) * 2 * shakeAmount;
        }

        public void Initialize(Transform target)
        {
            _targetTransform = target;
            _baseScale = _targetTransform.localScale;

            // Create a new parent for our target which we will animate with effects.
            _animatedTransform = new GameObject($"[Auto-Generated Shake Transform, by {nameof(SkysluggerBrain)}]").transform;
            _animatedTransform.SetParent(target.parent);
            target.SetParent(_animatedTransform);
        }

        public void Update()
        {
            _targetTransform.localScale = Vector3.Lerp(_targetTransform.localScale, TargetScale, scaleSpeed * Time.deltaTime);
            _animatedTransform.position = Vector3.Lerp(_animatedTransform.position, TargetOffset, scaleSpeed * Time.deltaTime);
        }
    }
}