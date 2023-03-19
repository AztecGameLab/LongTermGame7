using System;
using UnityEngine;

namespace poetools.Abstraction
{
    public class Gravity : MonoBehaviour
    {
        public Vector3 downDirection = Vector3.down;
        public float amount = -Physics.gravity.y;
        [SerializeField]private float idleGravity;
        
        private Rigidbody _physicsComponent;

        private void Awake()
        {
            _physicsComponent = GetComponent<Rigidbody>();
            // debug = DebugWhiteboard.Instance.AddLabel(() =>
                // $"velocity: {_physicsComponent.Velocity}\ngrounded: {_physicsComponent.IsGrounded}");
        }

        // private IDisposable debug;

        // private void OnDestroy()
        // {
            // debug.Dispose();
        // }

        private void FixedUpdate()
        {
            _physicsComponent.velocity = _physicsComponent.velocity + downDirection * (amount * Time.deltaTime);
        }
    }
}