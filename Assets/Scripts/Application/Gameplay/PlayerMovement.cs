using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _accelerationSmoothTime = 0.1f; //Acceleration & Deceleration speed
    [SerializeField] private float _reverseMultiplier = 1; //Used to cut _accelerationSmoothTime to allow more snappy movement when changing direction

    private CharacterController _controller;
    private Vector2 _playerInput;
    private Vector2 _currentDirection;
    private Vector2 _currentVelocity;
    private Vector3 _movementDirection;
    private bool _didReverse;

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

    /* OnMove() is automatically called by PlayerInput component with value change */
    public void OnMove(InputValue value)
    {
        _playerInput = value.Get<Vector2>();  
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _movementDirection.y < 0f)
            _movementDirection.y = 0f;

        _movementDirection.y += _gravity * Time.deltaTime;
    }

    private void ApplyAcceleration()
    {
        if(!_didReverse)
            _currentDirection = Vector2.SmoothDamp(_currentDirection, _playerInput, ref _currentVelocity, _accelerationSmoothTime);
        else
            _currentDirection = Vector2.SmoothDamp(_currentDirection, _playerInput, ref _currentVelocity, _accelerationSmoothTime / _reverseMultiplier);

        _movementDirection = new Vector3(_currentDirection.x, _movementDirection.y, _currentDirection.y);
    }

    private void CheckIfReverse()
    {
        if (_currentDirection.x > 0f && _playerInput.x < 0f)
            _didReverse = true;
        else if(_currentDirection.x < 0f && _playerInput.x > 0f)
            _didReverse = true;
        else if(_currentDirection.y > 0f && _playerInput.y < 0f)
            _didReverse = true;
        else if(_currentDirection.y < 0f && _playerInput.y > 0f)
            _didReverse = true;
        else
            _didReverse = false;
    }

    private void MovePlayer()
    {
        _controller.Move(_movementDirection * (_maxSpeed * Time.deltaTime));
    }
}