namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Represents an object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Run interaction logic for this entity.
        /// </summary>
        /// <param name="source">The object that is interacting with this object.</param>
        void Interact(GameObject source);
    }
}
