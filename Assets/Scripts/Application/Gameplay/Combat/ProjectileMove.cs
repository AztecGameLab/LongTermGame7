namespace Application.Gameplay.Combat
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectileMove : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private Vector3 _spawnPos;
        private float _projectileSpeed;
        private bool _isInitialized = false;

        public void Init(Vector3 targetPosition, Vector3 spawnPos, float speed)
        {
            _targetPosition = targetPosition;
            _spawnPos = spawnPos;
            _projectileSpeed = speed;
            _isInitialized = true;
        }

        private void Update()
        {
            if (_isInitialized)
            {
                float step = _projectileSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(_spawnPos, _targetPosition, step);
            }
        }
    }

}
