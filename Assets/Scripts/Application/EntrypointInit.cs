using Application.Core;
using Application.Core.Events;
using Application.Core.Rtf;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
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
                UnityEngine.Application.Quit();
#if UNITY_EDITOR
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                string originalScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
                await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
                var loadLevelEvent = new LoadLevelEvent(originalScene);
                Services.EventBus.Invoke(loadLevelEvent, "Editor Entrypoint Setup");
#endif
            }
        }
    }
}
