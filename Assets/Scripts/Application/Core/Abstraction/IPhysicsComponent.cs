namespace Application.Core.Abstraction
{
    using UnityEngine;

    /// <summary>
    /// An object that can be moved with a velocity.
    /// </summary>
    public interface IPhysicsComponent
    {
        /// <summary>
        /// Gets or sets a value that represents the current velocity of this object, in Units per second.
        /// </summary>
        /// <value>
        /// A value that represents the current velocity of this object, in Units per second.
        /// </value>
        Vector3 Velocity { get; set; }
    }
}
