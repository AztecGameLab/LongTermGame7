namespace Application.Core.Abstraction
{
    using UnityEngine;

    /// <summary>
    /// Applies gravity to a physics component.
    /// </summary>
    public class Gravity : MonoBehaviour
    {
        [SerializeField]
        private Vector3 downDirection = Vector3.down;

        [SerializeField]
        private float amount = -Physics.gravity.y;

        private IPhysicsComponent _physicsComponent;

        private void Awake()
        {
            _physicsComponent = GetComponent<IPhysicsComponent>();
        }

        private void FixedUpdate()
        {
            _physicsComponent.Velocity += downDirection * (amount * Time.deltaTime);
        }
    }
}
