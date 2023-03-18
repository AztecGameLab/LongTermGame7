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
            _currentDirection = GetMovementDirection();
            player.material = currentMovementSet.Get(_currentDirection);

            // Keep track of our position so we can check our velocity next frame.
            _previousPosition = transform.position;
        }

        private MovementDirection GetMovementDirection()
        {
            Vector3 movementDirection = transform.position - _previousPosition;

            Data down = new Data { Value = -movementDirection.z, Direction = MovementDirection.Down };
            Data up = new Data { Value = movementDirection.z, Direction = MovementDirection.Up };
            Data left = new Data { Value = -movementDirection.x, Direction = MovementDirection.Left };
            Data right = new Data { Value = movementDirection.x, Direction = MovementDirection.Right };

            Data[] data = { down, up, left, right };
            Data max = down;

            foreach (Data d in data)
            {
                // todo: this is still so janky, find better solution
                if (d.Value - max.Value > MovementThreshold)
                {
                    max = d;
                }
            }

            return max.Value < MovementThreshold * Time.deltaTime ? _currentDirection : max.Direction;
        }

        private struct Data
        {
            public float Value;
            public MovementDirection Direction;
        }
    }
}
