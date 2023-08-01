using System;
using UniRx;
using UniRx.Diagnostics;
using UnityEngine;

namespace Levels.__TESTING_LEVELS__.Real_Demo
{
    public class Projectile : AttackEmission
    {
        [SerializeField] private float intialSpeed = 5;
        
        private Subject<GameObject> _objectHit = new Subject<GameObject>();

        public IObservable<GameObject> ObserveObjectHit() => _objectHit;

        private void Start()
        {
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = transform.forward * intialSpeed;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.attachedRigidbody == null || IgnoreList.Contains(col.attachedRigidbody.gameObject))
            {
                return;
            }

            _objectHit.OnNext(col.attachedRigidbody.gameObject);
            _objectHit.OnCompleted();
            Destroy(gameObject);
        }

        /// <inheritdoc/>
        public override IObservable<Unit> ObserveFinished() => _objectHit.Select(_ => Unit.Default).Debug();
    }
}