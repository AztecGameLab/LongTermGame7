using DefaultNamespace;
using DefaultNamespace.Core;
using UnityEngine;

/// <summary>
/// This class should be the first one that is loaded in the game.
/// It controls the startup for the game systems.
/// </summary>
public class Entrypoint : MonoBehaviour
{
    public Event<int> test;
    
    private GameplaySystem _gameplaySystem;
    private AudioSystem _audioSystem;
    private LevelSystem _levelSystem;
    private VfxSystem _vfxSystem;
    private UISystem _uiSystem;

    private void TestListener(int data)
    {
        Debug.Log($"Received test data: {data}");
    }
    
    private void Start()
    {
        test.AddListener(TestListener, "Entrypoint");
        test.Invoke(5, "Entrypoint");
        test.RemoveListener(TestListener, "Entrypoint", "Testing removing a listener!");
        test.Invoke(-125, "Entrypoint");
        var settings = Resources.Load<ApplicationSettings>(ApplicationConstants.ApplicationSettingsPath);

        // Create systems.
        _audioSystem = new AudioSystem(settings.Audio);
        _vfxSystem = new VfxSystem(settings.Vfx);
        _gameplaySystem = new GameplaySystem(settings.Gameplay);
        _uiSystem = new UISystem(settings.Ui);
        _levelSystem = new LevelSystem(settings.Level);
    }

    private void OnApplicationQuit()
    {
        // Dispose in the reverse order they were created.
        _levelSystem.Dispose();
        _uiSystem.Dispose();
        _gameplaySystem.Dispose();
        _vfxSystem.Dispose();
        _audioSystem.Dispose();
    }
}