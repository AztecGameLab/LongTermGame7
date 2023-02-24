using Application.Core.Serialization;
using Application.Gameplay.Regions;
using System;
using UnityEngine.Rendering;

namespace Application
{
    using System.Threading.Tasks;
    using Core;
    using Core.Events;
    using Core.Rtf;
    using Gameplay;
    using Gameplay.Landmarks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// This class should be the first one that is loaded in the game.
    /// It should persist for the entire application lifetime, only being destroyed when the application quits.
    /// It controls the startup, updating, and shutdown of the game sub-systems.
    /// </summary>
    public partial class Entrypoint : MonoBehaviour
    {
        private GameplaySystem _gameplaySystem = new GameplaySystem();
        
        private static bool Initialized { get; set; }

        private void Awake()
        {
            Initialize();
        }

        private void OnDestroy()
        {
            Services.Serializer.WriteToDisk("TestingSave");
            
            _gameplaySystem.Dispose();
        }

        private void Initialize()
        {
            Initialized = true;
            DontDestroyOnLoad(this);

            Services.EventBus = new EventBus();
            Services.RegionTracker = new RegionTracker();
            Services.Serializer = new Serializer();
            Services.Serializer.ReadFromDisk("TestingSave");

            var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);
            Debug.Log($"Loaded settings: {settings.name}");

            // todo: unify level loading to clear confusion
            Services.EventBus.AddListener<LoadLevelEvent>(@event => SceneManager.LoadScene(@event.LevelName), "Level Loader");
            
            _gameplaySystem.Init();

            // todo: is this important?
            if (!Application.isEditor)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
