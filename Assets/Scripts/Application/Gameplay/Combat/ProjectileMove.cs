namespace Application.Gameplay.Combat
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectileMove : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private float _projectileSpeed;

        public void Init(Vector3 targetPosition, float speed)
        {
            _targetPosition = targetPosition;
            _projectileSpeed = speed;
        }
        private void Update()
        {
            if (_targetPosition != null && _projectileSpeed != null)
            {
                float step = _projectileSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            }
        }
    }

}
