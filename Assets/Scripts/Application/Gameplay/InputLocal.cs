using UnityEngine;

namespace Application.Gameplay
{
    public class InputLocal : MonoBehaviour
    {
        public Vector2 InputDirection { get; private set; }

        // and so on, with different input data.

        private void Update()
        {
            InputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    }
}