namespace Application.Vfx.Animation
{
    using Core;
    using UnityEngine;

    /// <summary>
    ///  Applies a material to this object based on the current input.
    /// </summary>
    public class AnimationController : MonoBehaviour
    {
        private const float MinSpeed = 0.1f;
        private const float MovementThreshold = 0.01f;

        [SerializeField]
        private MeshRenderer player;

        [SerializeField]
        private MovementMaterialSet idleMaterials;

        [SerializeField]
        private MovementMaterialSet runMaterials;

        private Vector3 _previousPosition;
        private MovementDirection _currentDirection = MovementDirection.Down;

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            idleMaterials.Build();
            runMaterials.Build();
        }

        private void FixedUpdate()
        {
            // Square Magnitude is faster than computing normal magnitude.
            bool isRunning = (transform.position - _previousPosition).sqrMagnitude > MinSpeed * MinSpeed * Time.deltaTime;

            MovementMaterialSet currentMovementSet = isRunning ? runMaterials : idleMaterials;
            _currentDirection = BetterGetDirection();
            player.material = currentMovementSet.Get(_currentDirection);

            // Keep track of our position so we can check our velocity next frame.
            _previousPosition = transform.position;
        }

        private MovementDirection BetterGetDirection()
        {
            Vector3 movementDirection = transform.position - _previousPosition;

            if (Mathf.Abs(movementDirection.x) - Mathf.Abs(movementDirection.z) > MovementThreshold)
            {
                if (movementDirection.x > 0)
                {
                    return MovementDirection.Right;
                }

                if (movementDirection.x < 0)
                {
                    return MovementDirection.Left;
                }
            }

            if (Mathf.Abs(movementDirection.z) - Mathf.Abs(movementDirection.x) > MovementThreshold)
            {
                if (movementDirection.z > 0)
                {
                    return MovementDirection.Up;
                }

                if (movementDirection.z < 0)
                {
                    return MovementDirection.Down;
                }
            }

            return _currentDirection;
        }
    }
}
