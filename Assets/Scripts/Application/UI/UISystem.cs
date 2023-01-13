using System;
using UnityEngine;

namespace Application.UI
{
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
}