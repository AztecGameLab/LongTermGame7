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
        public void Init()
        {
            _disposable = Services.EventBus.AddListener<LevelChangeEvent>(HandleSceneChange, "LevelLoading");
        }

        private static async void HandleSceneChange(LevelChangeEvent data)
        {
            SceneManager.LoadScene(data.NextScene);
            await Task.Delay(1);

            LevelEntrance[] allEntrances = Object.FindObjectsOfType<LevelEntrance>();
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
                Debug.LogWarning($"The entrance \"{data.TargetID}\" could not be found, falling back to \"{defaultEntrance.EntranceID}\"");
                targetEntrance = defaultEntrance;
            }
            else if (targetEntrance == null && allEntrances.Length > 0)
            {
                Debug.LogWarning($"The entrance \"{data.TargetID}\" could not be found, falling back to \"{allEntrances[0].EntranceID}\"");
                targetEntrance = allEntrances[0];
            }
            else
            {
                Debug.LogError("No entrances have been found!");
            }

            if (targetEntrance != null)
            {
                PlayerMovement playerInfo = Object.FindObjectOfType<PlayerMovement>();
                playerInfo.transform.position = targetEntrance.transform.position;
            }
        }
    }
}
