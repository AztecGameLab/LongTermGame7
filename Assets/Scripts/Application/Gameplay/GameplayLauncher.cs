namespace Application.Gameplay
{
    using System;
    using Core.Utility;
    using Core;
    using ImGuiNET;
    using TriInspector;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Handles commands to start the game.
    /// </summary>
    [Serializable]
    public class GameplayLauncher : IDisposable, IDebugImGui
    {
        private const string SceneId = "saved_scene";
        private const string PositionId = "saved_position";

        [SerializeField]
        [Scene]
        private string firstSceneName;

        [SerializeField]
        private TeamDataLoader dataLoader;

        [SerializeField]
        private GameObject gameplaySystemPrefab;

        private IDisposable _disposable;
        private GameObject _gameplaySystemInstance;

        /// <summary>
        /// Sets up the gameplay launcher.
        /// </summary>
        public void Initialize()
        {
            _disposable = Services.EventBus.AddListener<StartGameCommand>(HandleStartGame, "Gameplay Launcher");
            ImGuiUtil.Register(this);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable?.Dispose();
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Gameplay Launcher");

            if (ImGui.Button("Save Scene And Position"))
            {
                Services.Serializer.Store(SceneId, SceneManager.GetActiveScene().name);
                Services.Serializer.Store(PositionId, GameObject.FindGameObjectWithTag("Player").transform.position);
            }

            ImGui.End();
        }

        private void HandleStartGame(StartGameCommand command)
        {
            dataLoader.Init();

            if (_gameplaySystemInstance != null)
            {
                UnityEngine.Object.Destroy(_gameplaySystemInstance);
            }

            _gameplaySystemInstance = UnityEngine.Object.Instantiate(gameplaySystemPrefab);

            if (command.InitialScene != string.Empty)
            {
                ISpawningStrategy spawningStrategy = new OriginSpawningStrategy();
                string s = command.InitialScene;

                var m = new LevelChangeEvent { NextScene = s, SpawningStrategy = spawningStrategy };

                Services.EventBus.Invoke(m, "Gameplay Launcher");
                return;
            }

            var spawnStrategy = Services.Serializer.TryLookup(PositionId, out Vector3 scenePosition)
                ? new PositionSpawningStrategy(scenePosition) as ISpawningStrategy
                : new OriginSpawningStrategy();

            if (!Services.Serializer.TryLookup(SceneId, out string sceneName))
            {
                sceneName = firstSceneName;
            }

            var message = new LevelChangeEvent
            {
                NextScene = sceneName,
                SpawningStrategy = spawnStrategy,
            };

            Services.EventBus.Invoke(message, "Gameplay Launcher");
        }
    }
}
