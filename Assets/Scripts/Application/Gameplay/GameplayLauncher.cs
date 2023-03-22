namespace Application.Gameplay
{
    using System;
    using Application.Core.Utility;
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

        private IDisposable _disposable;

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
