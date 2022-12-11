using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSystem : IDisposable
{
    private readonly LevelSettings _settings;
    
    public LevelSystem(LevelSettings settings)
    {
        _settings = settings;
        
        Debug.Log($"Loaded level system, with \"{settings.name}\".");
    }
    
    public void Dispose()
    {
        Debug.Log("Disposed level system.");
    }

    public void RunDemo()
    {
        SceneManager.LoadScene("SampleScene");
    }
}