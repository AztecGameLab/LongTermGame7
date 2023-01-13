namespace Application.Core.Abstraction
{
    using UnityEngine;

    /// <inheritdoc cref="IPhysicsComponent" />
    public abstract class PhysicsComponent : MonoBehaviour, IPhysicsComponent
    {
        /// <inheritdoc/>
        public abstract Vector3 Velocity { get; set; }
    }
}
