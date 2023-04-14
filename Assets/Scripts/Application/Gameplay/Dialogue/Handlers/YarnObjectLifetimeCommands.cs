using System.Linq;

namespace Application.Gameplay.Dialogue.Handlers
{
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn commands for turning objects on and off.
    /// </summary>
    public class YarnObjectLifetimeCommands : IYarnCommandHandler
    {
        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            runner.AddCommandHandler<string, bool>("set-active", HandleSetActive);
            runner.AddCommandHandler<string>("enable", obj => HandleSetActive(obj, true));
            runner.AddCommandHandler<string>("disable", obj => HandleSetActive(obj, false));
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("set-active");
            runner.RemoveCommandHandler("enable");
            runner.RemoveCommandHandler("disable");
        }

        private static void HandleSetActive(string target, bool isActive)
        {
            var targetObj = Object.FindObjectsOfType<Transform>(true).FirstOrDefault(t => t.name == target);

            if (targetObj != null)
            {
                targetObj.gameObject.SetActive(isActive);
            }
        }
    }
}
