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

        [SerializeField] private bool oneShot;
        private bool _usable = true;

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
            if (oneShot && !_usable)
            {
                return;
            }

            _usable = false;
            StartCoroutine(Services.DialogueSystem.RunDialogue(reference));
        }
    }
}
