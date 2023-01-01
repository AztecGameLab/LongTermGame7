using System;
using UnityEngine;

public class UISystem : IDisposable
{
    public UISystem(UISettings settings)
    {
        Debug.Log($"Loaded UI system, with \"{settings.name}\".");
    }
    
    public void Dispose()
    {
        Debug.Log("Disposed UI system.");
    }
}