namespace Application.Gameplay
{
    using Core;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// An object that runs dialogue when interacted with.
    /// </summary>
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private DialogueReference reference;

        /// <inheritdoc/>
        public void Interact(GameObject source)
        {
            StartCoroutine(Services.DialogueSystem.RunDialogue(reference));
        }
    }
}
