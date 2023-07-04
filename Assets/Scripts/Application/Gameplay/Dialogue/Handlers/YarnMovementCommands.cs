namespace Application.Gameplay.Dialogue.Handlers
{
    using System.Collections;
    using Core.Utility;
    using UniRx;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn commands for moving around characters in the world.
    /// </summary>
    public class YarnMovementCommands : IYarnCommandHandler
    {
        private DialogueRunner _runner;


        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _runner = runner;
            runner.AddCommandHandler<GameObject, GameObject, float>("move", MoveToCoroutine);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("move");
        }

        private static IEnumerator MoveTo(GameObject from, GameObject to, float speed)
        {
            yield return from.transform.PathFindTo(to.transform.position, speed);
        }

        private Coroutine MoveToCoroutine(GameObject from, GameObject to, float speed = 5) =>
            _runner.StartCoroutine(MoveTo(from, to, speed));
    }
}
