﻿// using Core;
// using DefaultNamespace;
// using UnityEngine;
// using Logger = Core.Logger;
//
// /// <summary>
// /// This class should be the first one that is loaded in the game.
// /// It should persist for the entire application lifetime, only being destroyed when the application quits.
// /// It controls the startup, updating, and shutdown of the game sub-systems.
// /// </summary>
// public class Entrypoint : MonoBehaviour
// {
//     private GameplaySystem _gameplaySystem;
//     private AudioSystem _audioSystem;
//     private LevelSystem _levelSystem;
//     private VfxSystem _vfxSystem;
//     private UISystem _uiSystem;
//
//     #region Initialization Validation
//
//         private static bool _initialized;
//     
//         [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
//         private static void Init()
//         {
//             _initialized = false;
//         }
//         
//         private void Awake()
//         {
//             _initialized = true;
//         }
//         
//         [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
//         private static void CheckForAwake()
//         {
//             if (_initialized == false)
//             {
//                 Debug.LogError("Entrypoint must be initialized before anything else!");
//                 Application.Quit();
//
//                 #if UNITY_EDITOR
//                 
//                     UnityEditor.EditorApplication.isPlaying = false;                
//                 
//                 #endif
//             }
//         }
//
//     #endregion
//     
//     private void Start()
//     {
//         // Basic implementation of scene persistence. Could move to a dedicated persistent scene, but that is hard.
//         DontDestroyOnLoad(this);
//         
//         // Demo of how we could implement cross-cutting concerns.
//         // Ensures global access, polymorphism, and control over construction order + dependencies.
//         Services.Console = new Console();
//         Services.Logger = new Logger();
//         
//         // One approach to loading all our main settings.
//         var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);
//
//         // Demo of sub-system startup.
//         _audioSystem = new AudioSystem(settings.Audio);
//         _vfxSystem = new VfxSystem(settings.Vfx);
//         _gameplaySystem = new GameplaySystem(settings.Gameplay);
//         _uiSystem = new UISystem(settings.Ui);
//         _levelSystem = new LevelSystem(settings.Level);
//         
//         // A demo to showcase how all the sub-systems might come together to manage the game.
//         _levelSystem.RunDemo();
//     }
//
//     private void OnApplicationQuit()
//     {
//         // Shut down sub-systems in the reverse order they were created.
//         _levelSystem.Dispose();
//         _uiSystem.Dispose();
//         _gameplaySystem.Dispose();
//         _vfxSystem.Dispose();
//         _audioSystem.Dispose();
//     }
// }