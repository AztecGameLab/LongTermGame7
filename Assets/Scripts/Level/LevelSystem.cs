using System;
using UnityEngine;

public class LevelSystem : IDisposable
{
    public LevelSystem(LevelSettings settings)
    {
        Debug.Log($"Loaded level system, with \"{settings.name}\".");
    }
    
    public void Dispose()
    {
        Debug.Log("Disposed level system.");
    }
}