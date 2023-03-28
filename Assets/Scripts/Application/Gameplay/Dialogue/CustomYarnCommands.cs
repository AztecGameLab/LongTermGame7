namespace Application.Gameplay.Dialogue
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A project asset for saving custom yarn commands.
    /// </summary>
    [CreateAssetMenu]
    public class CustomYarnCommands : ScriptableObject
    {
        [SerializeReference]
        private List<IYarnCommandHandler> commandHandlers;

        /// <summary>
        /// Gets all of the custom command handlers.
        /// </summary>
        public IEnumerable<IYarnCommandHandler> CommandHandlers => commandHandlers;
    }
}
