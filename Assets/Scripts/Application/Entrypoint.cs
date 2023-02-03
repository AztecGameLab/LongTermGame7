using UnityEngine.Rendering;

namespace Application
{
    using System.Threading.Tasks;
    using Core;
    using Core.Events;
    using Core.Rtf;
    using Gameplay.Landmarks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// This class should be the first one that is loaded in the game.
    /// It should persist for the entire application lifetime, only being destroyed when the application quits.
    /// It controls the startup, updating, and shutdown of the game sub-systems.
    /// </summary>
    public class Entrypoint : MonoBehaviour
    {
        private static bool Initialized { get; set; }

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
                var loadLevelEvent = new LoadLevelEvent(originalScene);
                Services.EventBus.Invoke(loadLevelEvent, "Editor Entrypoint Setup");
#endif
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Initialized = true;

            // Basic implementation of scene persistence.
            DontDestroyOnLoad(this);

            // Demo of how we could implement cross-cutting concerns.
            // Ensures global access, polymorphism, and control over construction order + dependencies.
            Services.EventBus = new EventBus();

            // One approach to loading all our main settings.
            var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);
            Debug.Log($"Loaded settings: {settings.name}");

            var landmarkViewer = new LandmarkViewer();
            landmarkViewer.Init();

            var levelDesignUtil = new LevelDesignUtil();
            levelDesignUtil.Init();

            var levelLoader = new LevelLoader();
            levelLoader.Init();
            
            Services.EventBus.AddListener<LoadLevelEvent>(@event => SceneManager.LoadScene(@event.LevelName), "Level Loader");

            if (!Application.isEditor)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
