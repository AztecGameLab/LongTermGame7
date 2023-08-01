namespace Application.Vfx.Animation
{
    using Core;
    using FSM;
    using JetBrains.Annotations;
    using TriInspector;
    using UnityEngine;
    using StateMachine = FSM.StateMachine;

    /// <summary>
    /// Runs the logic behind animating monsters.
    /// </summary>
    [DeclareHorizontalGroup("animation_controls")]
    public class MonsterAnimator : MonoBehaviour
    {
        private const float Sensitivity = 0.5f;
        private readonly VelocityTracker _velocityTracker = new VelocityTracker();

        [SerializeField]
        [Tooltip("The animator that should be used to control the animations.")]
        private Animator animator;

        [SerializeField]
        [Tooltip("The name of the state in the animator to play when the monster is not moving.")]
        private string idleStateName = "Idle";

        [SerializeField]
        [Tooltip("The name of the state in the animator to play when the monster is moving.")]
        private string movingStateName = "Walk";

        [SerializeField]
        [Tooltip("How slow the monster should be moving when starting the idle animation.")]
        private float idleThreshold = 0.4f;

        [SerializeField]
        [Tooltip("How fast the monster should be moving when it starts the moving animation.")]
        private float movingThreshold = 1f;

        [SerializeField]
        [Tooltip("Should scale flipping be smoothly animated?")]
        private bool smoothFlip;

        private StateMachine _fsm;
        private bool _overrideAnimations;
        private Vector3 _originalScale;
        private FacingDirection _direction = FacingDirection.Right;

        // Hashed versions of state names, for performance.
        private int _idleStateHash;
        private int _movingStateHash;

        private enum FacingDirection
        {
            Left,
            Right,
        }

        private void Start()
        {
            _idleStateHash = Animator.StringToHash(idleStateName);
            _movingStateHash = Animator.StringToHash(movingStateName);

            AssertHasState(idleStateName);
            AssertHasState(movingStateName);

            _fsm = new StateMachine();
            _fsm.AddState("idle", onEnter: _ => PlayAnimation(_idleStateHash));
            _fsm.AddState("walking", onEnter: _ => PlayAnimation(_movingStateHash));
            _fsm.AddTransition(from: "idle", to: "walking", _ => !_overrideAnimations && _velocityTracker.Speed > movingThreshold);
            _fsm.AddTransition(from: "walking", to: "idle", _ => !_overrideAnimations && _velocityTracker.Speed < idleThreshold);
            _fsm.SetStartState("idle");
            _fsm.Init();

            _velocityTracker.Initialize(transform.position);
            _originalScale = transform.localScale;
        }

        private void PlayAnimation(int hash)
        {
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.Play(hash, 0);
            }
        }

        private void Update()
        {
            _velocityTracker.Update(transform.position, Time.deltaTime);
            _fsm.OnLogic();
            UpdateFacing();
        }

        // Make sure the sprite is always flipped to face the direction we are moving, using the X scale axis.
        // Notes: We don't rotate because of the normals getting weird on the backside, and causing bad lighting.
        private void UpdateFacing()
        {
            if (_velocityTracker.Velocity.x > Sensitivity)
            {
                _direction = FacingDirection.Right;
            }
            else if (_velocityTracker.Velocity.x < -Sensitivity)
            {
                _direction = FacingDirection.Left;
            }
            else
            {
                // We not moving at all, and the sprite can just stay at whatever it was before.
            }

            Transform t = transform;
            Vector3 scale = t.localScale;
            scale.x = _originalScale.x * (_direction == FacingDirection.Left ? -1 : 1);
            t.localScale = smoothFlip ? Vector3.Lerp(t.localScale, scale, 15 * Time.deltaTime) : scale;
        }

        // Checks to make sure that our animator contains the desired state, providing a useful error message.
        private void AssertHasState(string stateName)
        {
            var hash = Animator.StringToHash(stateName);

            if (animator != null && animator.runtimeAnimatorController != null && !animator.HasState(0, hash))
            {
                Debug.LogError($"{transform.GetFullName()} is missing the animation state \"{stateName}\"!", gameObject);
            }
        }

        /* Inspector button methods - useful for debugging animations. */

        [Button]
        [Group("animation_controls")]
        [UsedImplicitly]
        public void ForceIdle()
        {
            _overrideAnimations = true;
            _fsm.RequestStateChange("idle");
        }

        [Button]
        [Group("animation_controls")]
        [UsedImplicitly]
        public void ForceWalking()
        {
            _overrideAnimations = true;
            _fsm.RequestStateChange("walking");
        }

        [ShowIf("_overrideAnimations")]
        [Button]
        [UsedImplicitly]
        public void RestoreAnimations() => _overrideAnimations = false;
    }
}
