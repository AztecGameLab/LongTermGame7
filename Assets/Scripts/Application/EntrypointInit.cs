namespace Application
{
    using System.Threading.Tasks;
    using Core;
    using Core.Rtf;
    using Gameplay;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// The part of the entrypoint that ensures it remains loaded.
    /// </summary>
    public partial class Entrypoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Initialized = false;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static async Task CheckForAwakeAsync()
        {
            if (!Initialized)
            {
                Debug.LogWarning("Entrypoint must be initialized before anything else!");
                Application.Quit();
#if UNITY_EDITOR
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                string originalScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
                await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
                var message = new StartGameCommand { InitialScene = originalScene };
                Services.EventBus.Invoke(message, "Editor Entrypoint Setup");
#endif
            }
        }
    }
}
