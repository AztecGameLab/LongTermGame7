using System;

namespace DefaultNamespace.Core
{
    /// <summary>
    /// A special type of disposable that can be given a reason for its disposal.
    /// </summary>
    public interface IDebugDisposable : IDisposable
    {
        /// <summary>
        /// Dispose this object, and explain why it was disposed.
        /// </summary>
        /// <param name="debugReason">A friendly message that explains why this object was disposed.</param>
        void Dispose(string debugReason);
    }
}