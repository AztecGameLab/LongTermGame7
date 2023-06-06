namespace Application.Gameplay.Dialogue
{
    using Yarn.Unity;

    /// <summary>
    /// An object that can register yarn commands.
    /// Naming conventions for yarn commands:
    /// <remarks>
    ///     All lowercase. E.g. foobar,
    ///     Represent spaces with '-'. E.g. my-command-name,
    ///     Names should go from subject -> action. E.g. light-enable instead of enable-light
    /// </remarks>
    /// </summary>
    public interface IYarnCommandHandler
    {
        /// <summary>
        /// Register a set of new commands with the given DialogueRunner.
        /// </summary>
        /// <param name="runner">The runner to associate commands with.</param>
        void RegisterCommands(DialogueRunner runner);

        /// <summary>
        /// Unregister a set of commands from the given DialogueRunner.
        /// </summary>
        /// <param name="runner">The runner to unregister commands from.</param>
        void UnregisterCommands(DialogueRunner runner);
    }
}
