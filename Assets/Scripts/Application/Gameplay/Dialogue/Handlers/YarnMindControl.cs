namespace Application.Gameplay.Dialogue.Handlers
{
    using Cysharp.Threading.Tasks;
    using UniRx;
    using UnityEngine;
    using Vfx;
    using Yarn.Unity;

    public class YarnMindControl : IYarnCommandHandler
    {
        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            runner.AddCommandHandler<GameObject>(
                "mind-control",
                target => MainThreadDispatcher.StartCoroutine(HandleMindControl(target).ToCoroutine()));
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("mind-control");
        }

        private static async UniTask HandleMindControl(GameObject target)
        {
            if (target.TryGetComponent(out MindControlEffect mindControlEffect))
            {
                await mindControlEffect.ApplyEffect();
            }
        }
    }
}
