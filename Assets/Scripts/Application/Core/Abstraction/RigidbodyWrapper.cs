using System;
using UnityEngine;

namespace Application.Core.Abstraction
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyWrapper : PhysicsComponent
    {
        private Rigidbody _rigidbody;
        private float _airTime;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!IsGrounded)
                _airTime += Time.deltaTime;

            else _airTime = 0;
        }

        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        // todo: implement IsGrounded.
        public override bool IsGrounded => throw new NotImplementedException();
        public override float AirTime => _airTime;
    }
}