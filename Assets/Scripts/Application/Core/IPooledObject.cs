namespace Application.Gameplay.Combat
{
    using System;

    /// <summary>
    /// An object that can be pooled.
    /// </summary>
    /// <typeparam name="T">The type of object to pool.</typeparam>
    public interface IPooledObject<out T> : IDisposable
    {
        /// <summary>
        /// Gets the instance associated with this object.
        /// </summary>
        public T Instance { get; }
    }
}
