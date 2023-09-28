using System;
using UnityEngine;

namespace Application.Vfx
{
    public class CanvasCameraFinder : MonoBehaviour
    {
        private void Start()
        {
            if (TryGetComponent(out Canvas canvas))
                canvas.worldCamera = Camera.main;
        }
    }
}
