namespace Application.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Allows the player to interact with objects in the world.
    /// </summary>
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField]
        private float interactRange = 3;

        [SerializeField]
        private KeyCode interactKey = KeyCode.E;

        [SerializeField]
        private Vector3 offset = Vector3.up * 0.5f;

        [SerializeField]
        private PlayerMovement movement;

        private void Update()
        {
            if (Input.GetKeyDown(interactKey) && TryGetObject(out var result))
            {
                foreach (IInteractable interactable in result)
                {
                    interactable.Interact(gameObject);
                }
            }
        }

        private bool TryGetObject(out IEnumerable<IInteractable> result)
        {
            result = null;

            if (Physics.Raycast(new Ray(transform.position + offset, movement.FacingDirection), out RaycastHit info, interactRange))
            {
                GameObject parent = info.rigidbody != null
                    ? info.rigidbody.gameObject
                    : info.collider.gameObject;

                result = parent.GetComponentsInChildren<IInteractable>();
                return true;
            }

            return false;
        }
    }
}
