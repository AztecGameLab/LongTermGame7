namespace Application.Gameplay
{
    using JetBrains.Annotations;
    using UnityEngine;

    /// <summary>
    /// Input data associated with this object.
    /// </summary>
    public class InputLocal : MonoBehaviour
    {
        /// <summary>
        /// Gets the current direction the Player is trying to move.
        /// </summary>
        /// <value>
        /// The current direction the Player is trying to move.
        /// </value>
        [PublicAPI]
        public Vector2 InputDirection { get; private set; }

        private void Update()
        {
            InputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}
