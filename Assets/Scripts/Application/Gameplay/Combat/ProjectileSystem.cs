namespace Application.Gameplay.Combat
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ProjectileSystem
    {
        private Vector3 _userPosition;
        private Vector3 _targetPosition;
        private Transform _projectileTransform;

        public ProjectileSystem(Vector3 userPos, Vector3 targetPos, Transform projectileTransform)
        {
            _userPosition = userPos;
            _targetPosition = targetPos;
            _projectileTransform = projectileTransform;
        }


    }
}
