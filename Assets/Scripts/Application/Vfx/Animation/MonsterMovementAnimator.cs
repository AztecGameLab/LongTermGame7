using System;

namespace Application.Vfx.Animation
{
    using UnityEngine;

    /// <summary>
    /// Runs the logic behind animating monster movement.
    /// </summary>
    public class MonsterMovementAnimator : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("speed");

        private readonly VelocityTracker _velocityTracker = new VelocityTracker();

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer flipSprite;

        private bool _isFlipped;

        private void Awake()
        {
            _velocityTracker.Initialize(transform.position);
            _isFlipped = flipSprite.flipX;
        }

        private const float Sensitivity = 0.5f;

        private void Update()
        {
            _velocityTracker.Update(transform.position, Time.deltaTime);
            animator.SetFloat(Speed, _velocityTracker.Velocity.magnitude);

            if (_velocityTracker.Velocity.x > Sensitivity)
            {
                flipSprite.flipX = _isFlipped;
            }
            else if (_velocityTracker.Velocity.x < -Sensitivity)
            {
                flipSprite.flipX = !_isFlipped;
            }
            else
            {
                // We not moving at all, and the sprite can just stay at whatever it was before.
            }
        }

        // private void OnGUI()
        // {
        //     GUILayout.Label($"{_velocityTracker.Velocity.x < Sensitivity}");
        //     GUILayout.Label($"{_velocityTracker.Velocity.x > -Sensitivity}");
        // }
    }
}
