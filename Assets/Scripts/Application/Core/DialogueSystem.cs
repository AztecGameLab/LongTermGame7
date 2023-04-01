namespace Application.Core
{
    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;
    using Yarn.Unity;

    /// <summary>
    /// A service for interacting with the yarn Dialogue system.
    /// </summary>
    [Serializable]
    public class DialogueSystem
    {
        [SerializeField]
        private DialogueRunner runner;

        /// <summary>
        /// Executes the desired yarn dialogue.
        /// </summary>
        /// <param name="reference">The specific dialogue to run.</param>
        /// <returns>An IEnumerator that completes when the dialogue is finished.</returns>
        public IEnumerator RunDialogue(DialogueReference reference)
        {
            if (runner.IsDialogueRunning)
            {
                yield break;
            }

            yield return null;
            var playerInput = UnityEngine.Object.FindObjectOfType<PlayerInput>();

            if (playerInput)
            {
                playerInput.enabled = false;
            }

            runner.SetProject(reference.project);
            runner.StartDialogue(reference.nodeName);

            while (runner.IsDialogueRunning)
            {
                yield return null;
            }

            if (playerInput)
            {
                playerInput.enabled = true;
            }
        }
    }
}
