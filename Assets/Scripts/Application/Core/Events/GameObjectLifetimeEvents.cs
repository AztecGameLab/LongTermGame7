namespace Application.Core
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Handles Enable, Disable, and Destroy events, passing them to C# events.
    /// </summary>
    public class GameObjectLifetimeEvents : MonoBehaviour
    {
        /// <summary>
        /// Called when this GameObject is enabled.
        /// </summary>
        public event Action<GameObject> Enable;

        /// <summary>
        /// Called when this GameObject is disabled.
        /// </summary>
        public event Action<GameObject> Disable;

        /// <summary>
        /// Called when this GameObject is destroyed.
        /// </summary>
        public new event Action<GameObject> Destroy;

        private void OnEnable()
        {
            Enable?.Invoke(gameObject);
        }

        private void OnDisable()
        {
            Disable?.Invoke(gameObject);
        }

        private void OnDestroy()
        {
            Destroy?.Invoke(gameObject);
        }
    }
}
