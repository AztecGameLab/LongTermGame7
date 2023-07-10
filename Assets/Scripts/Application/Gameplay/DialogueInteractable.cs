namespace Application.Gameplay
{
    using Core;
    using UnityEngine;
    using Vfx;
    using Yarn.Unity;

    /// <summary>
    /// An object that runs dialogue when interacted with.
    /// </summary>
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private DialogueReference reference;

        private void Start()
        {
            var hintView = GetComponentInChildren<HintView>(true);

            if (hintView != null)
            {
                hintView.gameObject.SetActive(true);
            }
        }

        /// <inheritdoc/>
        public void Interact(GameObject source)
        {
            StartCoroutine(Services.DialogueSystem.RunDialogue(reference));
        }
    }
}
