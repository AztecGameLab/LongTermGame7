namespace Application.Gameplay
{
    using UnityEngine;
    using Vfx;

    /// <summary>
    /// Allows the player to interact with objects in the world.
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private KeyCode interactKey = KeyCode.E;

        [SerializeField]
        private InteractableHints hints;

        private void Update()
        {
            if (Input.GetKeyDown(interactKey))
            {
                hints.GetNearest(transform.position)?.Interact(gameObject);
            }
        }
    }
}
