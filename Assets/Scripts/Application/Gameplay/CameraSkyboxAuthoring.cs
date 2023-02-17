using System;
using UnityEngine;

namespace Application.Gameplay
{
    public class CameraSkyboxAuthoring : MonoBehaviour
    {
        public CameraClearFlags flags;
        public Color color;
        
        private void Start()
        {
            Camera.main.clearFlags = flags;
            Camera.main.backgroundColor = color;
        }
    }
}