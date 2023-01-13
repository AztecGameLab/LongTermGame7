namespace Application.Core
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

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
        private int _maxSize = 10000;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPool{T}"/> class.
        /// </summary>
        /// <param name="createFunc">Use to create a new instance when the pool is empty. In most cases this will just be: <code>() => new T()</code></param>
        /// <param name="collectionCheck">Collection checks are performed when an instance is returned back to the pool.
        /// An exception will be thrown if the instance is already in the pool. Collection checks are only performed in the Editor.</param>
        /// <param name="defaultCapacity">The default capacity the stack will be created with.</param>
        public ObjectPool(
            Func<T> createFunc,
            bool collectionCheck = true,
            int defaultCapacity = 10)
        {
            _stack = new Stack<T>(defaultCapacity);
            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _collectionCheck = collectionCheck;
        }

        /// <summary>
        /// Gets or sets called when the instance is being taken from the pool.
        /// </summary>
        /// <value>
        /// Called when the instance is being taken from the pool.
        /// </value>
        [PublicAPI]
        public event Action<T> ActionOnGet;

        /// <summary>
        /// Gets or sets called when the instance is being returned to the pool. This could be used to clean up or disable the instance.
        /// </summary>
        /// <value>
        /// Called when the instance is being returned to the pool. This could be used to clean up or disable the instance.
        /// </value>
        [PublicAPI]
        public event Action<T> ActionOnRelease;

        /// <summary>
        /// Gets or sets called when the element can not be returned to the pool due to it being equal to the maxSize.
        /// </summary>
        /// <value>
        /// Called when the element can not be returned to the pool due to it being equal to the maxSize.
        /// </value>
        [PublicAPI]
        public event Action<T> ActionOnDestroy;

        /// <summary>
        /// Gets or sets the maximum size of the pool. When the pool reaches the max size then any further.
        /// </summary>
        /// <value>
        /// The maximum size of the pool. When the pool reaches the max size then any further.
        /// </value>
        [PublicAPI]
        public int MaxSize
        {
            get => _maxSize;
            set
            {
                if (MaxSize <= 0)
                {
                    throw new ArgumentException("Max Size must be greater than 0", nameof(value));
                }

                _maxSize = value;
            }
        }

        /// <summary>
        /// Gets the total number of active and inactive objects.
        /// </summary>
        /// <value>
        /// The total number of active and inactive objects.
        /// </value>
        [PublicAPI]
        public int CountAll { get; private set; }

        /// <summary>
        /// Gets the number of objects that have been created by the pool but are currently in use and have not yet been returned.
        /// </summary>
        /// <value>
        /// The number of objects that have been created by the pool but are currently in use and have not yet been returned.
        /// </value>
        [PublicAPI]
        public int CountActive
        {
            get { return CountAll - CountInactive; }
        }

        /// <summary>
        /// Gets the number of objects that are currently available in the pool.
        /// </summary>
        /// <value>
        /// The number of objects that are currently available in the pool.
        /// </value>
        [PublicAPI]
        public int CountInactive
        {
            get { return _stack.Count; }
        }

        /// <summary>
        /// Get an object from the pool.
        /// </summary>
        /// <returns>A new object from the pool.</returns>
        [PublicAPI]
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

            ActionOnGet?.Invoke(element);
            return element;
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

            ActionOnRelease?.Invoke(element);

            if (CountInactive < _maxSize)
            {
                _stack.Push(element);
            }
            else
            {
                ActionOnDestroy?.Invoke(element);
            }
        }

        /// <summary>
        /// Releases all pooled objects so they can be garbage collected.
        /// </summary>
        public void Clear()
        {
            if (ActionOnDestroy != null)
            {
                foreach (var item in _stack)
                {
                    ActionOnDestroy(item);
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
        public readonly struct PooledObject<TO> : IDisposable, IEquatable<PooledObject<TO>>
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

            /// <inheritdoc/>
            public bool Equals(PooledObject<TO> other)
            {
                return EqualityComparer<TO>.Default.Equals(_toReturn, other._toReturn) && Equals(_pool, other._pool);
            }

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                return obj is PooledObject<TO> other && Equals(other);
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                unchecked
                {
                    return (EqualityComparer<TO>.Default.GetHashCode(_toReturn) * 397) ^ (_pool != null ? _pool.GetHashCode() : 0);
                }
            }
        }
    }
}
