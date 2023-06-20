using System.Text;

namespace Application.Core
{
    using System;
    using UnityEngine;

    /// <summary>
    /// GameObject related extension methods.
    /// </summary>
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Gets a full hierarchy path for naming a transform.
        /// </summary>
        /// <param name="target">The transform to start with.</param>
        /// <returns>The unique path of this transform in the hierarchy.</returns>
        public static string GetFullName(this Transform target)
        {
            var builder = new StringBuilder(target.name);

            while (target.parent != null)
            {
                target = target.parent;
                builder.Insert(0, $"{target.name}/");
            }

            return builder.ToString();
        }

        /// <inheritdoc cref="GetFullName(UnityEngine.Transform)"/>
        public static string GetFullName(this GameObject target)
        {
            return GetFullName(target.transform);
        }

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
