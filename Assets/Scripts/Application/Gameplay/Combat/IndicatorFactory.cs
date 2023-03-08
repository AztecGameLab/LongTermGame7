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
        private readonly ObjectPool<GameObject> _cubePool;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndicatorFactory"/> class.
        /// </summary>
        /// <param name="cubePrefab">The prefab to be used for the cube indicator.</param>
        public IndicatorFactory(GameObject cubePrefab)
        {
            _cubePool = new ObjectPool<GameObject>(() => Object.Instantiate(cubePrefab));
        }

        /// <summary>
        /// Get a cube indicator.
        /// </summary>
        /// <param name="result">The cube indicator object. Do whatever you want EXCEPT destroying it.</param>
        /// <returns>A disposable to release control of the indicator once finished.</returns>
        public IDisposable GetCube(out GameObject result)
        {
            result = _cubePool.Get().gameObject;

            if (result != null)
            {
                result.SetActive(true);
                return new DisposableHelper { Source = result, ParentPool = _cubePool };
            }

            return null;
        }

        private struct DisposableHelper : IDisposable
        {
            public ObjectPool<GameObject> ParentPool;
            public GameObject Source;

            public void Dispose()
            {
                Source.SetActive(false);
                ParentPool.Release(Source);
            }
        }
    }
}
