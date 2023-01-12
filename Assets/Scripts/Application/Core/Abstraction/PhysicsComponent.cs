namespace Application.Core.Abstraction
{
    using UnityEngine;

    public interface IPhysicsComponent
    {
        Vector3 Velocity { get; set; }
        bool IsGrounded { get; }
        float AirTime { get; }
    }
    
    /// <summary>
    /// An object that is a part of the physics world.
    /// </summary>
    public abstract class PhysicsComponent : MonoBehaviour, IPhysicsComponent
    {
        public abstract Vector3 Velocity { get; set; }
        public abstract bool IsGrounded { get; }
        public abstract float AirTime { get; }
    }

    
}