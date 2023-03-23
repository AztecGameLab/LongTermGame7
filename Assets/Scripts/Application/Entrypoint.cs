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
        private LevelLoader _levelLoader = new LevelLoader();

        [SerializeField]
        private GameplayLauncher launcher = new GameplayLauncher();

        private static bool Initialized { get; set; }

        private void Awake()
        {
            Initialize();
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

            launcher.Initialize();
            _levelLoader.Initialize();

            if (!Application.isEditor)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
