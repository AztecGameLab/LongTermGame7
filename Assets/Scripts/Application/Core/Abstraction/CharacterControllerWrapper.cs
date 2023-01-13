using UnityEngine;

namespace Application.Core.Abstraction
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerWrapper : PhysicsComponent
    {
        private CharacterController _character;
        private float _airTime;
        private bool _isGrounded;

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _character.Move(Velocity * Time.deltaTime);
            _isGrounded = _character.isGrounded;

            if (Mathf.Round(_character.velocity.sqrMagnitude) < Mathf.Round(Velocity.sqrMagnitude))
                Velocity = _character.velocity;

            if (!_isGrounded)
            {
                _airTime += Time.deltaTime;
            }

            else _airTime = 0;
        }
    
        public override Vector3 Velocity { get; set; }
        public override bool IsGrounded => _isGrounded;
        public override float AirTime => _airTime;
    }
}