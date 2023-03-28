namespace Application.Gameplay
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// An easy way for designers to bind UnityEvents with interactables.
    /// </summary>
    public class InteractableAuthoring : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private UnityEvent<GameObject> onInteract;

        /// <inheritdoc/>
        public void Interact(GameObject source)
        {
            onInteract.Invoke(source);
        }
    }
}
