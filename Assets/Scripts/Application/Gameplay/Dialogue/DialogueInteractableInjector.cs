namespace Application.Gameplay.Dialogue
{
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Yarn.Unity;

    /// <summary>
    /// Whenever a scene is loaded, initialize all dialogue interactables.
    /// </summary>
    [RequireComponent(typeof(DialogueRunner))]
    public class DialogueInteractableInjector : MonoBehaviour
    {
        private DialogueRunner _dialogueRunner;

        private void Awake()
        {
            _dialogueRunner = GetComponent<DialogueRunner>();
            SceneManager.sceneLoaded += InjectInteractables;
        }

        private void InjectInteractables(Scene scene, LoadSceneMode loadSceneMode)
        {
            foreach (DialogueInteractable dialogueInteractable in FindObjectsOfType<DialogueInteractable>())
            {
                dialogueInteractable.Initialize(_dialogueRunner);
            }
        }
    }
}
