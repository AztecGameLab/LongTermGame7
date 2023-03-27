using UnityEngine.InputSystem;

namespace Application.Gameplay
{
    using System;
    using Core;
    using Core.Utility;
    using UniRx;
    using UnityEngine;
    using Vfx;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Responds to LevelChangeEvents and performs the logic needed to load and position the
    /// player in the new scene.
    /// </summary>
    [Serializable]
    public class LevelLoader : IDisposable
    {
        [SerializeField]
        private CanvasGroup fadeImage;

        private IDisposable _disposable;
        private FadeTransition _fadeTransition;

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        /// <summary>
        /// Sets up the Level Loader.
        /// </summary>
        /// <returns>This instance.</returns>
        public LevelLoader Initialize()
        {
            _disposable = Services.EventBus.AddListener<LevelChangeEvent>(HandleSceneChange, "LevelLoading");
            _fadeTransition = new FadeTransition(1, 1, fadeImage);
            return this;
        }

        private async void HandleSceneChange(LevelChangeEvent data)
        {
            var playerInput = Object.FindObjectOfType<PlayerInput>();

            if (playerInput != null)
            {
                playerInput.enabled = false;
            }

            await _fadeTransition.ShowEffect();
            await LevelLoadingUtil.LoadFully(data.NextScene).ToTask();

            var playerSpawner = Object.FindObjectOfType<PlayerSpawn>();

            // Try to spawn the player, if set up correctly.
            if (playerSpawner != null)
            {
                playerSpawner.Spawn();
                var spawnPosition = data.SpawningStrategy.GetSpawnPosition();

                // Now update all party member transforms to the correct spawn position.
                playerSpawner.SpawnedPlayer.transform.position = spawnPosition;

                foreach (TeamMemberWorldView spawnedMember in playerSpawner.SpawnedMembers)
                {
                    spawnedMember.transform.position = spawnPosition;
                }

                // We need to initialize this logic last, after all positions are fully set to avoid problems.
                playerSpawner.MonsterFollowPlayer.Target = playerSpawner.SpawnedPlayer.transform;
            }
            else
            {
                var legacyPlayer = Object.FindObjectOfType<PlayerMovement>();

                try
                {
                    legacyPlayer.transform.position = data.SpawningStrategy.GetSpawnPosition();
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            await _fadeTransition.HideEffect();
        }
    }
}
