﻿using Application.Core;

namespace Application.Gameplay
{
    using Cinemachine;
    using UnityEngine;

    /// <summary>
    /// Hooks into a trigger to change the priority of a Cinemachine camera.
    /// </summary>
    public class ChangeCameraTrigger : TriggerEffect
    {
        private const int ActivePriority = 50;

        [SerializeField]
        private CinemachineVirtualCamera targetCamera;

        protected override void HandleCollisionEnter(GameObject obj)
        {
            targetCamera.Priority = ActivePriority;
        }

        protected override void HandleCollisionExit(GameObject obj)
        {
            targetCamera.Priority = 0;
        }
    }
}
