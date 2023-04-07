using UniRx;

namespace Application.Gameplay.Dialogue
{
    using Core;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// A trigger that runs some dialogue when an object walks inside.
    /// </summary>
    public class DialogueTrigger : TriggerEffect
    {
        [SerializeField]
        private DialogueReference dialogue;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            base.HandleCollisionEnter(obj);
            Observable.FromMicroCoroutine(() => Services.DialogueSystem.RunDialogue(dialogue)).Subscribe();
        }
    }
}
