namespace Application.Gameplay
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using UnityEngine;
    using UnityEngine.SceneManagement;
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
            SceneManager.LoadScene(data.NextScene);
            await Task.Delay(100);

            var playerSpawner = Object.FindObjectOfType<PlayerSpawn>();
            playerSpawner.Spawn();

            LevelEntrance[] allEntrances = Object.FindObjectsOfType<LevelEntrance>();

            if (allEntrances.Length > 0)
            {
                LevelEntrance targetEntrance = null;
                LevelEntrance defaultEntrance = null;

                foreach (LevelEntrance entrance in allEntrances)
                {
                    if (entrance.EntranceID == data.TargetID)
                    {
                        targetEntrance = entrance;
                    }

                    if (entrance.DefaultEntrance)
                    {
                        defaultEntrance = entrance;
                    }
                }

                if (targetEntrance == null && defaultEntrance != null)
                {
                    Debug.LogWarning(
                        $"The entrance \"{data.TargetID}\" could not be found, falling back to \"{defaultEntrance.EntranceID}\"");
                    targetEntrance = defaultEntrance;
                }
                // else if (targetEntrance == null && allEntrances.Length > 0)
                // {
                //     Debug.LogWarning(
                //         $"The entrance \"{data.TargetID}\" could not be found, falling back to \"{allEntrances[0].EntranceID}\"");
                //     targetEntrance = allEntrances[0];
                // }

                if (targetEntrance != null)
                {
                    Vector3 spawnPosition = targetEntrance.transform.position;
                    playerSpawner.SpawnedPlayer.transform.position = spawnPosition;

                    foreach (TeamMemberWorldView spawnedMember in playerSpawner.SpawnedMembers)
                    {
                        spawnedMember.transform.position = spawnPosition;
                    }
                }

                playerSpawner.MonsterFollowPlayer.Target = playerSpawner.SpawnedPlayer.transform;
            }
        }
    }
}
