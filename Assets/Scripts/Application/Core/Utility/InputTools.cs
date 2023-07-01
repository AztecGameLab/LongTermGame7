using System;
using UniRx;

namespace Application.Core
{
    using UnityEngine;

    /// <summary>
    /// Provides utility methods for polling the Unity input API.
    /// </summary>
    public static class InputTools
    {
        /// <summary>
        /// Gets an observable that fires each time an object becomes hovered over.
        /// </summary>
        public static IObservable<RaycastHit> ObjectMouseHover { get; } = Observable.EveryFixedUpdate()
            .Select(_ => CameraMouseRaycast())
            .Where(tuple => tuple.isHit) // only fire if we hit something
            .Select(tuple => tuple.info);

        private static (bool isHit, RaycastHit info) CameraMouseRaycast()
        {
            float distance = float.PositiveInfinity;
            Camera camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            int mask = ~LayerMask.GetMask("Ignore Raycast");
            bool hit = Physics.Raycast(ray, out var hitInfo, distance, mask);
            return (hit, hitInfo);
        }

        public static IObservable<RaycastHit> ObjectSelect { get; } = Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Mouse0))
            .SelectMany(_ => ObjectMouseHover.First());

        /// <summary>
        /// Polls the alpha-numeric keys to see if a number has just been pressed.
        /// </summary>
        /// <param name="number">The number that's being pressed, if any. Within the range [1, 9].</param>
        /// <returns>Whether an alpha-numeric key is being pressed.</returns>
        public static bool TryGetNumberDown(out int number)
        {
            const int maxNumber = 9;
            const int offsetToKeyCode1 = 48;

            for (int i = 1; i <= maxNumber; i++)
            {
                if (Input.GetKeyDown((KeyCode)i + offsetToKeyCode1))
                {
                    number = i;
                    return true;
                }
            }

            number = -1;
            return false;
        }

        /// <summary>
        /// Polls to see if the player has selected an input direction.
        /// </summary>
        /// <param name="direction">The resulting direction, if any.</param>
        /// <returns>True if the player just pressed an input direction.</returns>
        public static bool TryGetInputDirectionDown(out Vector2 direction)
        {
            direction = Vector2.zero;

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction = Vector3.forward;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction = Vector3.back;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction = Vector3.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction = Vector3.right;
            }

            return direction != Vector2.zero;
        }
    }
}
