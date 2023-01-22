namespace Application.Gameplay
{
    using Core.Abstraction;
    using UnityEngine;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Applies movement to the player.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
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

        private CharacterController _controller;
        private Vector2 _playerInput;
        private Vector2 _currentDirection;
        private Vector2 _currentVelocity;
        private Vector3 _movementDirection;
        private bool _didReverse;

        /// <summary>
        /// Automatically called from PlayerInput component.
        /// </summary>
        /// <param name="value">The input value.</param>
        public void OnMove(InputValue value)
        {
            if (value != null)
            {
                _playerInput = value.Get<Vector2>();
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            ApplyGravity();
            ApplyAcceleration();
            CheckIfReverse();
            MovePlayer();
        }

        private void ApplyGravity()
        {
            if (groundCheck.IsGrounded && _movementDirection.y < 0f)
            {
                _movementDirection.y = 0f;
            }

            _movementDirection.y += gravity * Time.deltaTime;
        }

        private void ApplyAcceleration()
        {
            _currentDirection = !_didReverse
                ? Vector2.SmoothDamp(_currentDirection, _playerInput, ref _currentVelocity, accelerationSmoothTime)
                : Vector2.SmoothDamp(_currentDirection, _playerInput, ref _currentVelocity, accelerationSmoothTime / reverseMultiplier);

            _movementDirection = new Vector3(_currentDirection.x, _movementDirection.y, _currentDirection.y);
        }

        private void CheckIfReverse()
        {
            if (_currentDirection.x > 0f && _playerInput.x < 0f)
            {
                _didReverse = true;
            }
            else if (_currentDirection.x < 0f && _playerInput.x > 0f)
            {
                _didReverse = true;
            }
            else if (_currentDirection.y > 0f && _playerInput.y < 0f)
            {
                _didReverse = true;
            }
            else if (_currentDirection.y < 0f && _playerInput.y > 0f)
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
            _controller.Move(_movementDirection * (maxSpeed * Time.deltaTime));
        }
    }
}
