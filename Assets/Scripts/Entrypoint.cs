using Core;
using DefaultNamespace;
using UnityEngine;
using Logger = Core.Logger;

/// <summary>
/// This class should be the first one that is loaded in the game.
/// It controls the startup for the game systems.
/// </summary>
public class Entrypoint : MonoBehaviour
{
    private GameplaySystem _gameplaySystem;
    private AudioSystem _audioSystem;
    private LevelSystem _levelSystem;
    private VfxSystem _vfxSystem;
    private UISystem _uiSystem;

    private void Start()
    {
        // Demo of how we could implement cross-cutting concerns.
        // Ensures global access, polymorphism, and control over construction order + dependencies.
        Services.Console = new Console();
        Services.Logger = new Logger();
        
        // One approach to loading all our main settings.
        var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);

        // Demo of sub-system startup.
        _audioSystem = new AudioSystem(settings.Audio);
        _vfxSystem = new VfxSystem(settings.Vfx);
        _gameplaySystem = new GameplaySystem(settings.Gameplay);
        _uiSystem = new UISystem(settings.Ui);
        _levelSystem = new LevelSystem(settings.Level);
    }

    private void OnApplicationQuit()
    {
        // Shut down sub-systems in the reverse order they were created.
        _levelSystem.Dispose();
        _uiSystem.Dispose();
        _gameplaySystem.Dispose();
        _vfxSystem.Dispose();
        _audioSystem.Dispose();
    }
}