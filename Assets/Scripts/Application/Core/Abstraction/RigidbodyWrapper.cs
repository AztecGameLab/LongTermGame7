namespace Application.Core.Abstraction
{
    using UnityEngine;

    /// <summary>
    /// Wraps the Unity Rigidbody as a physics object.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyWrapper : PhysicsComponent
    {
        private Rigidbody _rigidbody;
        private bool _isGrounded;

        /// <inheritdoc/>
        public override Vector3 Velocity
        {
            get => _rigidbody.velocity;
            set => _rigidbody.velocity = value;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
}
