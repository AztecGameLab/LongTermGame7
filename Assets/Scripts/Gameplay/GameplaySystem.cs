using System;
using UnityEngine;

public class GameplaySystem : IDisposable
{
    public GameplaySystem(GameplaySettings settings)
    {
        Debug.Log($"Loaded gameplay system, with \"{settings.name}\".");
    }
    
    public void Dispose()
    {
        Debug.Log("Disposed gameplay system.");
    }
}