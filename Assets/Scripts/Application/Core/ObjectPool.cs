namespace Application.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Generic object pool implementation.
    /// </summary>
    /// <typeparam name="T">Type of the object pool.</typeparam>
    public class ObjectPool<T> : IDisposable
        where T : class
    {
        private readonly Stack<T> _stack;
        private readonly bool _collectionCheck;
        private readonly Func<T> _createFunc;
        private readonly Action<T> _actionOnGet;
        private readonly Action<T> _actionOnRelease;
        private readonly Action<T> _actionOnDestroy;
        private readonly int _maxSize; // Used to prevent catastrophic memory retention.

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="createFunc">Use to create a new instance when the pool is empty. In most cases this will just be <code>() => new T()</code></param>
        /// <param name="actionOnGet">Called when the instance is being taken from the pool.</param>
        /// <param name="actionOnRelease">Called when the instance is being returned to the pool. This could be used to clean up or disable the instance.</param>
        /// <param name="actionOnDestroy">Called when the element can not be returned to the pool due to it being equal to the maxSize.</param>
        /// <param name="collectionCheck">Collection checks are performed when an instance is returned back to the pool. An exception will be thrown if the instance is already in the pool. Collection checks are only performed in the Editor.</param>
        /// <param name="defaultCapacity">The default capacity the stack will be created with.</param>
        /// <param name="maxSize">The maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size.</param>
        public ObjectPool(Func<T> createFunc, Action<T> actionOnGet = null, Action<T> actionOnRelease = null, Action<T> actionOnDestroy = null, bool collectionCheck = true, int defaultCapacity = 10, int maxSize = 10000)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max Size must be greater than 0", nameof(maxSize));
            }

            _stack = new Stack<T>(defaultCapacity);
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _maxSize = maxSize;
            _actionOnGet = actionOnGet;
            _actionOnRelease = actionOnRelease;
            _actionOnDestroy = actionOnDestroy;
            _collectionCheck = collectionCheck;
        }

        /// <summary>
        /// Gets the total number of active and inactive objects.
        /// </summary>
        public int CountAll { get; private set; }

        /// <summary>
        /// Gets the number of objects that have been created by the pool but are currently in use and have not yet been returned.
        /// </summary>
        public int CountActive
        {
            get { return CountAll - CountInactive; }
        }

        /// <summary>
        /// Gets the number of objects that are currently available in the pool.
        /// </summary>
        public int CountInactive
        {
            get { return _stack.Count; }
        }

        /// <summary>
        /// Get an object from the pool.
        /// </summary>
        /// <returns>A new object from the pool.</returns>
        public T Get()
        {
            T element;

            if (_stack.Count == 0)
            {
                element = _createFunc();
                CountAll++;
            }
            else
            {
                element = _stack.Pop();
            }

            _actionOnGet?.Invoke(element);
            return element;
        }

        /// <summary>
        /// Get a new <see cref="PooledObject{TO}"/> which can be used to return the instance back to the pool when the PooledObject is disposed.
        /// </summary>
        /// <param name="v">Output new typed object.</param>
        /// <returns>New PooledObject.</returns>
        public PooledObject<T> Get(out T v)
        {
            v = Get();
            return new PooledObject<T>(v, this);
        }

        /// <summary>
        /// Release an object to the pool.
        /// </summary>
        /// <param name="element">Object to release.</param>
        public void Release(T element)
        {
            if (_collectionCheck && _stack.Count > 0 && _stack.Contains(element))
            {
                throw new InvalidOperationException("Trying to release an object that has already been released to the pool.");
            }

            _actionOnRelease?.Invoke(element);

            if (CountInactive < _maxSize)
            {
                _stack.Push(element);
            }
            else
            {
                _actionOnDestroy?.Invoke(element);
            }
        }

        /// <summary>
        /// Releases all pooled objects so they can be garbage collected.
        /// </summary>
        public void Clear()
        {
            if (_actionOnDestroy != null)
            {
                foreach (var item in _stack)
                {
                    _actionOnDestroy(item);
                }
            }

            _stack.Clear();
            CountAll = 0;
        }

        /// <summary>
        /// Releases all pooled objects so they can be garbage collected.
        /// </summary>
        public void Dispose()
        {
            // Ensure we do a clear so the destroy action can be called.
            Clear();
        }

        /// <summary>
        /// An instance of data that has been pooled, and can be re-used.
        /// </summary>
        /// <typeparam name="TO">The type of data to store.</typeparam>
        public readonly struct PooledObject<TO> : IDisposable
            where TO : class
        {
            private readonly TO _toReturn;
            private readonly ObjectPool<TO> _pool;

            /// <summary>
            /// Initializes a new instance of the <see cref="PooledObject{T}"/> struct.
            /// </summary>
            /// <param name="value">The data to store in this object.</param>
            /// <param name="pool">The pool this object belongs to.</param>
            internal PooledObject(TO value, ObjectPool<TO> pool)
            {
                _toReturn = value;
                _pool = pool;
            }

            /// <summary>
            /// Return this object to its pool, so it can be reused.
            /// </summary>
            void IDisposable.Dispose() => _pool.Release(_toReturn);
        }
    }
}