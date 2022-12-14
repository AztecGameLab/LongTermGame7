using System;
using UnityEngine;

namespace Application.Audio
{
    public class AudioSystem : IDisposable
    {
        public AudioSystem(AudioSettings settings)
        {
            Debug.Log($"Loaded audio system, with \"{settings.name}\".");
        }
    
        public void Dispose()
        {
            Debug.Log("Disposed audio system.");
        }
    }
}