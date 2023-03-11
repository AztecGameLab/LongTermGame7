namespace Application.Gameplay
{
    using Core;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Yarn.Unity;

    /// <summary>
    /// A trigger effect that runs some yarn dialogue when activated.
    /// </summary>
    [RequireComponent(typeof(Trigger))]
    public class StartDialogueTrigger : TriggerEffect
    {
        [SerializeField]
        private string nodeId;

        private DialogueRunner _dialogueRunner;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            _dialogueRunner.StartDialogue(nodeId);
            _dialogueRunner.onDialogueComplete.AddListener(HandleDialogueComplete);

            FindObjectOfType<PlayerInput>().enabled = false;
        }

        private static void HandleDialogueComplete()
        {
            FindObjectOfType<PlayerInput>().enabled = true;
        }

        private void Awake()
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>();
        }
    }
}
