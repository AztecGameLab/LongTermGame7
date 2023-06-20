using System.Globalization;
using TriInspector;

namespace Application.Vfx.Animation
{
    using System.Text;
    using FSM;
    using UnityEngine;

    /// <summary>
    /// Runs the logic behind animating monster movement.
    /// </summary>
    public class MonsterMovementAnimator : MonoBehaviour
    {
        private const float Sensitivity = 0.5f;
        private readonly VelocityTracker _velocityTracker = new VelocityTracker();

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer flipSprite;

        [SerializeField]
        private string idleStateName = "idle";

        [SerializeField]
        private string movingStateName = "walk";

        [SerializeField]
        private float idleThreshold = 0.4f;

        [SerializeField]
        private float movingThreshold = 1f;

        private StateMachine _fsm;
        private bool _isFlipped;
        private bool _overrideAnimations;

        // Hashed versions of state names, for performance.
        private int _idleStateHash;
        private int _movingStateHash;

        private static string GetFullName(Transform target)
        {
            var builder = new StringBuilder(target.name);

            while (target.parent != null)
            {
                target = target.parent;
                builder.Insert(0, $"{target.name}/");
            }

            return builder.ToString();
        }

        private void AssertHasState(string stateName)
        {
            var hash = Animator.StringToHash(stateName);

            if (!animator.HasState(0, hash))
            {
                Debug.LogError($"{GetFullName(transform)} is missing the animation state \"{stateName}\"!", gameObject);
            }
        }

        private void Start()
        {
            _idleStateHash = Animator.StringToHash(idleStateName);
            _movingStateHash = Animator.StringToHash(movingStateName);

            AssertHasState(idleStateName);
            AssertHasState(movingStateName);

            _fsm = new StateMachine();
            _fsm.AddState("idle", onEnter: _ => animator.Play(_idleStateHash, 0));
            _fsm.AddState("walking", onEnter: _ => animator.Play(_movingStateHash, 0));
            _fsm.AddTransition(from: "idle", to: "walking", _ => !_overrideAnimations && _velocityTracker.Speed > movingThreshold);
            _fsm.AddTransition(from: "walking", to: "idle", _ => !_overrideAnimations && _velocityTracker.Speed < idleThreshold);
            _fsm.SetStartState("idle");
            _fsm.Init();

            _velocityTracker.Initialize(transform.position);
            _isFlipped = flipSprite.flipX;
        }

        [Button]
        private void ForceIdle()
        {
            _overrideAnimations = true;
            _fsm.RequestStateChange("idle");
        }

        [Button]
        private void ForceWalking()
        {
            _overrideAnimations = true;
            _fsm.RequestStateChange("walking");
        }

        [EnableIf("_overrideAnimations")]
        [Button]
        private void RestoreAnimations() => _overrideAnimations = false;

        private void Update()
        {
            _velocityTracker.Update(transform.position, Time.deltaTime);
            _fsm.OnLogic();
            UpdateFacing();
        }

        private void UpdateFacing()
        {
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
    }
}
