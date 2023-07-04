namespace Application.Gameplay
{
    using Core.Abstraction;
    using FMODUnity;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Tracks the distance walked and periodically emits footstep events.
    /// </summary>
    public class FootstepSource : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The distance between steps.")]
        private float stepDistance = 1;

        [SerializeField]
        private bool showDebug;

        [SerializeField]
        private UnityEvent onStep;

        private GroundCheck _groundCheck;

        // distance walked since last step.
        private float _elapsedDistance;
        private Vector3 _previousPosition = new Vector3(0, 0, 0);

        private void Start()
        {
            _groundCheck = GetComponent<GroundCheck>();
        }

        private void Update()
        {
            if (_groundCheck.IsGrounded)
            {
                // Update elapsedDistance with how far we have moved this frame.
                _elapsedDistance += Vector3.Distance(transform.position, _previousPosition);

                // Compare elapsedDistance to our step distance to see if we walked far enough
                if (_elapsedDistance >= stepDistance)
                {
                    onStep.Invoke();

                    // If we did, fire the event and reset elapsedDistance to 0
                    _elapsedDistance = 0;
                }

                _previousPosition = transform.position;
            }
        }

        private void OnGUI()
        {
            if (showDebug)
            {
                GUILayout.Label($"elapsedDistance: {_elapsedDistance}");
            }
        }
    }
}
