namespace Application.Gameplay
{
    using System;
    using System.Collections;
    using Core;
    using Core.Utility;
    using UniRx;
    using UnityEngine;
    using UnityEngine.InputSystem;
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
            _disposable = Services.EventBus.AddListener<LevelChangeEvent>(
                data => HandleSceneChange(data).ToObservable().Subscribe(), "LevelLoading");

            _fadeTransition = new FadeTransition(1, 1, fadeImage);
            return this;
        }

        private IEnumerator HandleSceneChange(LevelChangeEvent data)
        {
            var playerInput = Object.FindObjectOfType<PlayerInput>();

            if (playerInput != null)
            {
                playerInput.enabled = false;
            }

            yield return _fadeTransition.ShowEffect().ToYieldInstruction();
            yield return LevelLoadingUtil.LoadFully(data.NextScene).ToYieldInstruction();

            // Try to spawn the player, if set up correctly.
            var playerSpawner = Object.FindObjectOfType<PlayerSpawn>();

            if (playerSpawner != null)
            {
                playerSpawner.Spawn();
                var spawnPosition = data.SpawningStrategy.CalculateSpawnPosition();

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
                // We are using the legacy player system (e.g. no PlayerSpawn).
                // In this case, the player is probably positioned manually, and we should let it be.
                Debug.LogError("LEVEL DESIGNERS: This scene is not using a PlayerSpawn prefab to spawn players." +
                               "Please add one to the scene ASAP to get rid of this message and ensure everything works.");
            }

            yield return _fadeTransition.HideEffect().ToYieldInstruction();
        }
    }
}
