using System;

namespace Application.Gameplay.Dialogue
{
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Automatically binds the given custom yarn commands with the dialogue system.
    /// </summary>
    [RequireComponent(typeof(DialogueRunner))]
    public class CustomYarnCommandInjector : MonoBehaviour
    {
        [SerializeField]
        private CustomYarnCommands customCommands;

        private DialogueRunner _runner;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();

            foreach (IYarnCommandHandler commandHandler in customCommands.CommandHandlers)
            {
                commandHandler.RegisterCommands(_runner);
            }
        }

        private void OnDestroy()
        {
            foreach (IYarnCommandHandler commandHandler in customCommands.CommandHandlers)
            {
                commandHandler.UnregisterCommands(_runner);
            }
        }

        private void OnApplicationQuit()
        {
            foreach (IYarnCommandHandler commandHandler in customCommands.CommandHandlers)
            {
                commandHandler.UnregisterCommands(_runner);
            }
        }
    }
}
