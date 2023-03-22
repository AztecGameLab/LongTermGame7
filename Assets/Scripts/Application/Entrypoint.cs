namespace Application
{
    using Core;
    using Core.Serialization;
    using Gameplay;
    using Gameplay.Regions;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// This class should be the first one that is loaded in the game.
    /// It should persist for the entire application lifetime, only being destroyed when the application quits.
    /// It controls the startup, updating, and shutdown of the game sub-systems.
    /// </summary>
    public partial class Entrypoint : MonoBehaviour
    {
        [SerializeField]
        private GameplaySystem gameplaySystem = new GameplaySystem();

        private static bool Initialized { get; set; }

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            gameplaySystem.Dispose();
        }

        private void Initialize()
        {
            Initialized = true;
            DontDestroyOnLoad(this);

            Services.EventBus = new EventBus();
            Services.RegionTracker = new RegionTracker();
            Services.Serializer = new Serializer();

            var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);
            Debug.Log($"Loaded settings: {settings.name}");

            gameplaySystem.Init();

            if (!Application.isEditor)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
