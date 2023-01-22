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

        [SerializeField]
        private MeshRenderer player;

        [SerializeField]
        private MovementMaterialSet idleMaterials;

        [SerializeField]
        private MovementMaterialSet runMaterials;

        private Vector3 _previousPosition;
        private MovementDirection _currentDirection = MovementDirection.Down;

        private static bool InputHeld(params KeyCode[] keys)
        {
            bool result = false;

            foreach (KeyCode keyCode in keys)
            {
                result |= Input.GetKey(keyCode);
            }

            return result;
        }

        private void OnValidate()
        {
            Awake();
        }

        private void Awake()
        {
            idleMaterials.Build();
            runMaterials.Build();
        }

        private void Update()
        {
            // Square Magnitude is faster than computing normal magnitude.
            bool isRunning = (transform.position - _previousPosition).sqrMagnitude > MinSpeed * MinSpeed * Time.deltaTime;

            MovementMaterialSet currentMovementSet = isRunning ? runMaterials : idleMaterials;
            _currentDirection = GetMovementDirection();
            player.material = currentMovementSet.Get(_currentDirection);

            // Keep track of our position so we can check our velocity next frame.
            _previousPosition = transform.position;
        }

        // todo: we want to avoid checking Input.GetKey eventually, so we can steal input from the player for menus
        private MovementDirection GetMovementDirection()
        {
            if (InputHeld(KeyCode.DownArrow, KeyCode.S))
            {
                return MovementDirection.Down;
            }
            else if (InputHeld(KeyCode.LeftArrow, KeyCode.A))
            {
                return MovementDirection.Left;
            }
            else if (InputHeld(KeyCode.RightArrow, KeyCode.D))
            {
                return MovementDirection.Right;
            }
            else if (InputHeld(KeyCode.UpArrow, KeyCode.W))
            {
                return MovementDirection.Up;
            }
            else
            {
                return _currentDirection;
            }
        }
    }
}
