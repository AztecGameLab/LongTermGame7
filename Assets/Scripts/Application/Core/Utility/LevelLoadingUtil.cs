namespace Application.Core.Utility
{
    using System;
    using UniRx;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public static class LevelLoadingUtil
    {
        public static IObservable<AsyncOperation> LoadFully(string sceneName, IProgress<float> progress = null, LoadSceneParameters parameters = default)
        {
            var op = SceneManager.LoadSceneAsync(sceneName, parameters);
            return op.AsAsyncOperationObservable(progress);
        }
    }
}
