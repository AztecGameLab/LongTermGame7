namespace Application.Gameplay.Combat
{
    using Core;
    using Dialogue;
    using Yarn.Unity;

    /// <summary>
    /// Commands for manipulating respawn points.
    /// </summary>
    public class YarnRespawnCommands : IYarnCommandHandler
    {
        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            runner.AddCommandHandler<string>("respawn-set", HandleRespawnChange);
            runner.AddCommandHandler("respawn-load", HandleLoadRespawn);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("respawn-set");
            runner.RemoveCommandHandler("respawn-load");
        }

        private static void HandleRespawnChange(string respawnPointId)
        {
            Services.RespawnTracker.SetRespawnPoint(respawnPointId);
        }

        private static void HandleLoadRespawn()
        {
            Services.RespawnTracker.Respawn();
        }
    }
}
