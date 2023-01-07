using UnityEngine;

namespace Application.Gameplay
{
    public class InputLocal : MonoBehaviour
    {
        public Vector2 InputDirection => 
            new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // and so on, with different input data.
    }
}