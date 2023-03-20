namespace Application.Gameplay
{
    using System;
    using Core;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles commands to start the game.
    /// </summary>
    public class GameplayLauncher : IDisposable
    {
        private IDisposable _disposable;

        public void Initialize()
        {
            _disposable = Services.EventBus.AddListener<StartGameCommand>(HandleStartGame, "Gameplay Launcher");
        }

        private void HandleStartGame(StartGameCommand command)
        {
            var message = new LevelChangeEvent
            {
                // todo: load this info from a save file instead
                NextScene = "CombatFramework",
                SpawningStrategy = new OriginSpawningStrategy(), // todo: use a position strat loading from a file
            };

            Services.EventBus.Invoke(message, "Gameplay Launcher");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable?.Dispose();
        }
    }
}
