namespace Application.Gameplay.Dialogue.Handlers
{
    using System;
    using Combat;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Commands for starting combat encounters via yarn.
    /// </summary>
    public class YarnCombatCommands : IYarnCommandHandler
    {
        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            runner.AddCommandHandler<GameObject>("combat-start", HandleCombatStart);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("combat-start");
        }

        private static void HandleCombatStart(GameObject target)
        {
            if (target.TryGetComponent(out OverworldCombatDefinition combatDefinition))
            {
                combatDefinition.EnterCombat();
            }
            else
            {
                throw new ArgumentException("Yarn tried to start a combat encounter, but it didn't exist!");
            }
        }
    }
}
