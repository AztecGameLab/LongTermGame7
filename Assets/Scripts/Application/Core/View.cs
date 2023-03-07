namespace Application.Gameplay.Combat.UI
{
    using UnityEngine;

    /// <summary>
    /// A MonoBehaviour that is designed to display some data.
    /// </summary>
    /// <typeparam name="T">The type of data to display.</typeparam>
    public abstract class View<T> : MonoBehaviour
    {
        /// <summary>
        /// Associates this view with this data.
        /// </summary>
        /// <param name="target">The data to display.</param>
        public abstract void BindTo(T target);
    }
}
