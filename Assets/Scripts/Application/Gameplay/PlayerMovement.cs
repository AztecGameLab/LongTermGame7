namespace Application.Gameplay
{
    using Core.Abstraction;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Applies movement to the player.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float gravity = -9.81f;

        [SerializeField]
        private float maxSpeed = 5;

        [SerializeField]
        private float accelerationSmoothTime = 0.1f; // Acceleration & Deceleration speed

        [SerializeField]
        private float reverseMultiplier = 1; // Used to cut _accelerationSmoothTime to allow more snappy movement when changing direction

        [SerializeField]
        private GroundCheck groundCheck;

        private IPhysicsComponent _controller;
        private Vector3 _targetVelocity;
        private Vector3 _velocity;
        private bool _didReverse;
        private Vector3 _currentVelocity;

        /// <summary>
        /// Gets the direction that the player is currently facing.
        /// </summary>
        public Vector3 FacingDirection { get; private set; }

        /// <summary>
        /// Automatically called from PlayerInput component.
        /// </summary>
        /// <param name="value">The input value.</param>
        public void OnMove(InputValue value)
        {
            if (value != null)
            {
                var playerInput = value.Get<Vector2>();
                _targetVelocity = new Vector3(playerInput.x, 0, playerInput.y) * maxSpeed;

                if (playerInput != Vector2.zero)
                {
                    FacingDirection = new Vector3(playerInput.x, 0, playerInput.y);
                }
            }
        }

        private void Start()
        {
            _controller = GetComponent<IPhysicsComponent>();
        }

        private void Update()
        {
            _velocity = _controller.Velocity;
            ApplyGravity();

            if (groundCheck.IsGrounded)
                ApplyAcceleration();

            CheckIfReverse();
            MovePlayer();
        }

        private void ApplyGravity()
        {
            if (groundCheck.IsGrounded && _velocity.y < 0f)
            {
                _velocity.y = 0f;
            }

            _velocity.y += gravity * Time.deltaTime;
        }

        private void ApplyAcceleration()
        {
            var oldY = _velocity.y;
            _velocity = !_didReverse
                ? Vector3.SmoothDamp(_velocity, _targetVelocity, ref _currentVelocity, accelerationSmoothTime)
                : Vector3.SmoothDamp(_velocity, _targetVelocity, ref _currentVelocity, accelerationSmoothTime / reverseMultiplier);
            _velocity.y = oldY;
        }

        private void CheckIfReverse()
        {
            if (_velocity.x > 0f && _targetVelocity.x < 0f)
            {
                _didReverse = true;
            }
            else if (_velocity.x < 0f && _targetVelocity.x > 0f)
            {
                _didReverse = true;
            }
            else if (_velocity.y > 0f && _targetVelocity.y < 0f)
            {
                _didReverse = true;
            }
            else if (_velocity.y < 0f && _targetVelocity.y > 0f)
            {
                _didReverse = true;
            }
            else
            {
                _didReverse = false;
            }
        }

        private void MovePlayer()
        {
            Physics.SyncTransforms();
            _controller.Velocity = _velocity;
        }
    }
}
