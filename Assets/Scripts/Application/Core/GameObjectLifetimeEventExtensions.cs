namespace Application.Core
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Static extension methods for hooking into the GameObject lifetime.
    /// </summary>
    public static class GameObjectLifetimeEventExtensions
    {
        /// <summary>
        /// Binds the lifetime of an IDisposable to the lifetime of a GameObject.
        /// </summary>
        /// <param name="disposable">The disposable to clean up.</param>
        /// <param name="component">The component attached to a GameObject that should be watched.</param>
        public static void AddTo(this IDisposable disposable, Component component)
        {
            if (component != null)
            {
                AddTo(disposable, component.gameObject);
            }
        }

        /// <summary>
        /// Binds the lifetime of an IDisposable to the lifetime of a GameObject.
        /// </summary>
        /// <param name="disposable">The disposable to clean up.</param>
        /// <param name="obj">The GameObject to associate the disposable with.</param>
        public static void AddTo(this IDisposable disposable, GameObject obj)
        {
            if (obj != null)
            {
                if (!obj.TryGetComponent(out GameObjectLifetimeEvents lifetimeEvents))
                {
                    lifetimeEvents = obj.AddComponent<GameObjectLifetimeEvents>();
                }

                lifetimeEvents.Destroy += _ => disposable.Dispose();
            }
        }
    }
}
