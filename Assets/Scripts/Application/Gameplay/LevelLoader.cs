namespace Application.Gameplay
{
    using System;
    using System.Linq;
    using Core;
    using Core.Utility;
    using UniRx;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Responds to LevelChangeEvents and performs the logic needed to load and position the
    /// player in the new scene.
    /// </summary>
    public class LevelLoader : IDisposable
    {
        private IDisposable _disposable;

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposable.Dispose();
        }

        /// <summary>
        /// Sets up the Level Loader.
        /// </summary>
        /// <returns>This instance.</returns>
        public LevelLoader Init()
        {
            _disposable = Services.EventBus.AddListener<LevelChangeEvent>(HandleSceneChange, "LevelLoading");
            return this;
        }

        private static async void HandleSceneChange(LevelChangeEvent data)
        {
            // todo: screen transitions
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
        }
    }
}
