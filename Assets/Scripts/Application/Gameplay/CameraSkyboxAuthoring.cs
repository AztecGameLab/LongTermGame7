using System;
using UnityEngine;

namespace Application.Gameplay
{
    public class CameraSkyboxAuthoring : MonoBehaviour
    {
        public CameraClearFlags flags;
        public Color color;
        
        private void Awake()
        {
            Camera.main.clearFlags = flags;
            Camera.main.backgroundColor = color;
        }
    }
}