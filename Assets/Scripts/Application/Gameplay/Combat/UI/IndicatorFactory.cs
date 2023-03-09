namespace Application.Gameplay.Combat
{
    using System;
    using Core;
    using UnityEngine;
    using Object = UnityEngine.Object;

    // todo: move this out of the global namespace, and into a more combat restricted one.

    /// <summary>
    /// A global factory for creating and re-using common visual indicator, especially for combat.
    /// </summary>
    public class IndicatorFactory
    {
        private readonly ObjectPool<ValidityIndicator> _validityPool;
        private readonly ObjectPool<PathIndicator> _pathPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndicatorFactory"/> class.
        /// </summary>
        /// <param name="validity">The validity prefab.</param>
        /// <param name="path">The path prefab.</param>
        public IndicatorFactory(ValidityIndicator validity, PathIndicator path)
        {
            _validityPool = CreatePool(validity);
            _pathPool = CreatePool(path);
        }

        /// <summary>
        /// Gets an indicator.
        /// </summary>
        /// <param name="result">The indicator object. Do whatever you want EXCEPT destroying it.</param>
        /// <returns>A disposable to release control of the indicator once finished.</returns>
        public IDisposable GetValidityIndicator(out ValidityIndicator result) => GetHelper(out result, _validityPool);

        /// <inheritdoc cref="GetValidityIndicator"/>
        public IDisposable GetPathIndicator(out PathIndicator result) => GetHelper(out result, _pathPool);

        private static ObjectPool<T> CreatePool<T>(T prefab)
            where T : Component
        {
            return new ObjectPool<T>(() => Object.Instantiate(prefab));
        }

        private static IDisposable GetHelper<T>(out T result, ObjectPool<T> pool)
            where T : Component
        {
            result = pool.Get();
            result.gameObject.SetActive(true);
            return new DisposableHelper<T> { Source = result, ParentPool = pool };
        }

        private struct DisposableHelper<T> : IDisposable
            where T : Component
        {
            public ObjectPool<T> ParentPool;
            public T Source;

            public void Dispose()
            {
                Source.gameObject.SetActive(false);
                Source.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                ParentPool.Release(Source);
            }
        }
    }
}
