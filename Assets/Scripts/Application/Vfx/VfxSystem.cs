using System;
using UnityEngine;

namespace Application.Vfx
{
    public class VfxSystem : IDisposable
    {
        public VfxSystem(VfxSettings settings)
        {
            Debug.Log($"Loaded vfx system, with \"{settings.name}\".");
        }
    
        public void Dispose()
        {
            Debug.Log("Disposed vfx system.");
        }
    }
}