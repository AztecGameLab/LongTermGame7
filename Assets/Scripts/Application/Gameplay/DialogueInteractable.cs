namespace Application.Gameplay
{
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// An object that runs dialogue when interacted with.
    /// </summary>
    public class DialogueInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private DialogueReference reference;

        private DialogueRunner _runner;

        /// <summary>
        /// Sets up this interactable to pass events to a runner.
        /// </summary>
        /// <param name="runner">The runner that should display dialogue for this interactable.</param>
        public void Initialize(DialogueRunner runner)
        {
            _runner = runner;
        }

        /// <inheritdoc/>
        public void Interact(GameObject source)
        {
            _runner.SetProject(reference.project);
            _runner.StartDialogue(reference.nodeName);
        }
    }
}
