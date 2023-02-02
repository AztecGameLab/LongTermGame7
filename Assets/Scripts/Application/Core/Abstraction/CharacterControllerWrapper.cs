namespace Application.Core.Abstraction
{
    using UnityEngine;

    /// <summary>
    /// Wraps the Unity CharacterController as a physics object.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class CharacterControllerWrapper : PhysicsComponent
    {
        private CharacterController _character;

        /// <inheritdoc/>
        public override Vector3 Velocity { get; set; }

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            _character.Move(Velocity * Time.deltaTime);

            if (Mathf.Round(_character.velocity.sqrMagnitude) < Mathf.Round(Velocity.sqrMagnitude))
            {
                Velocity = _character.velocity;
            }
        }
    }
}
