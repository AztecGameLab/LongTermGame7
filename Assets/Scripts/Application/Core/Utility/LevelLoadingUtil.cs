namespace Application.Core.Utility
{
    using System;
    using System.Collections;
    using UniRx;
    using UnityEngine.SceneManagement;

    public static class LevelLoadingUtil
    {
        public static IObservable<Unit> LoadFully(string sceneName, LoadSceneParameters parameters = default)
        {
            return LoadFullyCoroutine(sceneName, parameters).ToObservable();
        }

        private static IEnumerator LoadFullyCoroutine(string sceneName, LoadSceneParameters parameters = default)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, parameters);
            // yield return null;
        }
    }
}
