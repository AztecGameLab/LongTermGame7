namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections.Generic;
    using Core;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// A global factory for creating and re-using common visual indicator, especially for combat.
    /// </summary>
    public class IndicatorFactory
    {
        private readonly Dictionary<Type, object> _pools = new Dictionary<Type, object>();

        /// <summary>
        /// Allows the factory to create prefabs of a certain type.
        /// </summary>
        /// <param name="prefab">The prefab data to use when pooling.</param>
        /// <typeparam name="T">The type of data to pool.</typeparam>
        public void RegisterIndicator<T>(T prefab)
            where T : Component
        {
            _pools.Add(typeof(T), CreatePool(prefab));
        }

        /// <summary>
        /// Gets an indicator.
        /// </summary>
        /// <typeparam name="T">The type of indicator you want.</typeparam>
        /// <returns>An instance of the requested indicator, and a disposable to
        /// release control of the indicator once finished.</returns>
        public IPooledObject<T> Borrow<T>()
            where T : Component
        {
            ObjectPool<T> pool = (ObjectPool<T>)_pools[typeof(T)];
            T instance = pool.Get();
            instance.gameObject.SetActive(true);
            return new DisposableHelper<T> { Instance = instance, Parent = this };
        }

        /// <summary>
        /// Releases control of an indicator.
        /// </summary>
        /// <param name="indicator">The indicator you are done using.</param>
        /// <typeparam name="T">The type of the indicator.</typeparam>
        public void Release<T>(T indicator)
            where T : Component
        {
            if (indicator != null)
            {
                indicator.gameObject.SetActive(false);
                indicator.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

                ObjectPool<T> pool = (ObjectPool<T>)_pools[typeof(T)];
                pool.Release(indicator);
            }
        }

        private static ObjectPool<T> CreatePool<T>(T prefab)
            where T : Component
        {
            return new ObjectPool<T>(() => Object.Instantiate(prefab));
        }

        private struct DisposableHelper<T> : IPooledObject<T>
            where T : Component
        {
            public IndicatorFactory Parent;

            public T Instance { get; set; }

            public void Dispose()
            {
                Parent.Release(Instance);
            }
        }
    }
}
