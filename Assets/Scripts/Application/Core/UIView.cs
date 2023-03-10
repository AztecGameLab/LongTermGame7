namespace Application.Gameplay.Combat.UI
{
    using UnityEngine.EventSystems;

    /// <summary>
    /// A MonoBehaviour that is designed to display some data.
    /// </summary>
    /// <typeparam name="T">The type of data to display.</typeparam>
    public abstract class UIView<T> : UIBehaviour
    {
        /// <summary>
        /// Gets the object that this view is currently bound to.
        /// </summary>
        public T CurrentTarget { get; private set; }

        /// <summary>
        /// Associates this view with this data.
        /// </summary>
        /// <param name="target">The data to display.</param>
        public virtual void BindTo(T target)
        {
            CurrentTarget = target;
        }
    }
}