using System;
using System.Collections;
using System.Collections.Generic;

namespace Application.Gameplay
{
    using Cinemachine;
    using Core;
    using UnityEngine;

    /// <summary>
    /// Hooks into a trigger to change the priority of a Cinemachine camera.
    /// </summary>
    public class ChangeCameraTrigger : TriggerEffect
    {
        private const int ActivePriority = 50;

        [SerializeField]
        private CinemachineVirtualCamera targetCamera;

        private void Awake()
        {
            targetCamera.gameObject.SetActive(false);
        }

        private IEnumerator Start()
        {
            yield return null;
            targetCamera.gameObject.SetActive(true);
        }

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            targetCamera.Priority = ActivePriority;
        }

        /// <inheritdoc/>
        protected override void HandleCollisionExit(GameObject obj)
        {
            targetCamera.Priority = 0;
        }
    }
}
