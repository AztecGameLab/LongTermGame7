namespace Application.Core.Utility
{
    using System;
    using System.Collections;
    using UniRx;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// Utility methods for working with the scene manager.
    /// </summary>
    public static class LevelLoadingUtil
    {
        /// <summary>
        /// Does a better job at loading a scene, ensuring that all FindObject calls work after completing.
        /// </summary>
        /// <param name="sceneName">The scene to load.</param>
        /// <param name="parameters">The parameters with which to load the scene.</param>
        /// <returns>An observable for when the scene fully finishes loading.</returns>
        public static IObservable<Unit> LoadFully(string sceneName, LoadSceneParameters parameters = default)
        {
            return LoadFullyCoroutine(sceneName, parameters).ToObservable();
        }

        private static IEnumerator LoadFullyCoroutine(string sceneName, LoadSceneParameters parameters = default)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, parameters);
        }
    }
}
