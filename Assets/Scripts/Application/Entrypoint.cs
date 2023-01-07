using System.Threading.Tasks;
using Application.Audio;
using Application.Core;
using Application.Core.Events;
using Application.Gameplay;
using Application.Level;
using Application.UI;
using Application.Vfx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
    /// <summary>
    /// This class should be the first one that is loaded in the game.
    /// It should persist for the entire application lifetime, only being destroyed when the application quits.
    /// It controls the startup, updating, and shutdown of the game sub-systems.
    /// </summary>
    public class Entrypoint : MonoBehaviour
    {
        private GameplaySystem _gameplaySystem;
        private AudioSystem _audioSystem;
        private LevelSystem _levelSystem;
        private VfxSystem _vfxSystem;
        private UISystem _uiSystem;

        #region Initialization Validation

            private static bool _initialized;
        
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
            private static void ResetStatics()
            {
                _initialized = false;
            }
            
            private void Awake()
            {
                Initialize();
            }

            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
            private static async void CheckForAwake()
            {
                if (_initialized == false)
                {
                    Debug.LogWarning("Entrypoint must be initialized before anything else!");
                    UnityEngine.Application.Quit();
#if UNITY_EDITOR
                    Debug.Log($"{"[EDITOR ONLY]".Bold()} Loading Entrypoint...");
                    string originalScene = SceneManager.GetActiveScene().name;
                    SceneManager.LoadScene("Entrypoint", LoadSceneMode.Single);
                    await Task.Yield(); // We have to wait one frame here, so the Entrypoint can initialize itself
                    Debug.Log($"{"[EDITOR ONLY]".Bold()} Trying to load {originalScene} after Entrypoint...");
                    var loadLevelEvent = new LoadLevelEvent { LevelName = originalScene };
                    Services.EventBus.Invoke(loadLevelEvent, "Editor Entrypoint Setup");
#endif
                }
            }

        #endregion

        private void Initialize()
        {
            _initialized = true;
            
            // Basic implementation of scene persistence. Could move to a dedicated persistent scene, but that is hard.
            DontDestroyOnLoad(this);
        
            // Demo of how we could implement cross-cutting concerns.
            // Ensures global access, polymorphism, and control over construction order + dependencies.
            Services.EventBus = new EventBus();
        
            // One approach to loading all our main settings.
            var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);

            // Demo of sub-system startup.
            _audioSystem = new AudioSystem(settings.Audio);
            _vfxSystem = new VfxSystem(settings.Vfx);
            _gameplaySystem = new GameplaySystem(settings.Gameplay);
            _uiSystem = new UISystem(settings.Ui);
            _levelSystem = new LevelSystem(settings.Level);
        }
        
        private void Start()
        {
            // A demo to showcase how all the sub-systems might come together to manage the game.
            _levelSystem.RunDemo();
        }
        
        private void OnDestroy()
        {
            // Shut down sub-systems in the reverse order they were created.
            _levelSystem.Dispose();
            _uiSystem.Dispose();
            _gameplaySystem.Dispose();
            _vfxSystem.Dispose();
            _audioSystem.Dispose();
        }
    }
}