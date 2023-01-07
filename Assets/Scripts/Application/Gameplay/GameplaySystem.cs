using System;
using UnityEngine;

namespace Application.Gameplay
{
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
}