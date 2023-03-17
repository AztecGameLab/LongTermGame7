namespace Application.Core.Abstraction
{
    using System;
    using UnityEngine;
    using UnityEngine.Assertions;
    using Utility;

    /// <summary>
    /// Stores and updated information about "ground" surfaces, usually below an object.
    /// </summary>
    public class GroundCheck : MonoBehaviour
    {
        // The most hits that can be tracked in the raycast buffer.
        private const int MaxHits = 10;
        private readonly RaycastHit[] _hits = new RaycastHit[MaxHits];

        [SerializeField]
        [Tooltip("The downwards direction used to check if we are grounded.")]
        private Vector3 gravityDirection = Vector3.down;

        [SerializeField]
        [Tooltip("How steep a slope we can climb without slipping.")]
        private float slopeLimitDegrees = 45f;

        [SerializeField]
        [Tooltip("Draws debug information to the screen.")]
        private bool showDebug;

        [SerializeField]
        [Tooltip("How far below the origin should be checked.")]
        private float groundDistance = 1;

        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private bool _wasGroundedLastFrame;

        /// <summary>
        /// Fired the frame this object touches the ground.
        /// </summary>
        public event Action OnTouchGround;

        /// <summary>
        /// Fired the frame this object leaves the ground.
        /// </summary>
        public event Action OnLeaveGround;

        /// <summary>
        /// Gets a value indicating whether this object is currently touching the ground.
        /// </summary>
        /// <value>
        /// Whether this object is currently touching the ground.
        /// </value>
        public bool IsGrounded { get; private set; }

        /// <summary>
        /// Gets a value indicating how much time has elapsed while grounded.
        /// </summary>
        /// <value>
        /// A value indicating how much time has elapsed while grounded.
        /// </value>
        public float TimeSpentGrounded { get; private set; }

        /// <summary>
        /// Gets a value indicating how much time has elapsed while in the air.
        /// </summary>
        /// <value>
        /// A value indicating how much time has elapsed while in the air.
        /// </value>
        public float TimeSpentFalling { get; private set; }

        /// <summary>
        /// Gets the normal of the surface that this object is currently colliding with.
        /// <remarks>If this object is not colliding with anything, this parameter returns Vector3.zero.</remarks>
        /// </summary>
        /// <value>
        /// The normal of the surface that this object is currently colliding with.
        /// </value>
        public Vector3 ContactNormal { get; private set; }

        /// <summary>
        /// Gets the collider of the surface that this object is currently colliding with.
        /// <remarks>If this object is not colliding with anything, this parameter returns "null".</remarks>
        /// </summary>
        /// <value>
        /// The collider of the surface that this object is currently colliding with.
        /// </value>
        public Collider ConnectedCollider { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this object has just started touching the ground this frame.
        /// </summary>
        /// <value>
        /// A value indicating whether this object has just started touching the ground this frame.
        /// </value>
        public bool JustEntered => IsGrounded && !_wasGroundedLastFrame;

        /// <summary>
        /// Gets a value indicating whether this object has just started touching the ground this frame.
        /// </summary>
        /// <value>
        /// A value indicating whether this object has just started touching the ground this frame.
        /// </value>
        public bool JustExited => !IsGrounded && _wasGroundedLastFrame;

        private void FixedUpdate()
        {
            UpdateIsGrounded();
        }

        private void UpdateIsGrounded()
        {
            _wasGroundedLastFrame = IsGrounded;
            CheckIsGrounded();

            if (JustEntered)
            {
                OnTouchGround?.Invoke();
                TimeSpentFalling = 0;
            }

            if (JustExited)
            {
                OnLeaveGround?.Invoke();
                TimeSpentGrounded = 0;
            }

            if (IsGrounded)
            {
                TimeSpentGrounded += Time.deltaTime;
            }
            else
            {
                TimeSpentFalling += Time.deltaTime;
            }
        }

        private void CheckIsGrounded()
        {
            // todo: better way to specifying different raycast volumes / strategies
            int hits = Physics.BoxCastNonAlloc(transform.position, new Vector3(0.25f, Mathf.Abs(groundDistance / 2), 0.25f) * 0.99f, -transform.up, _hits, Quaternion.identity, groundDistance / 2);
            Assert.IsTrue(hits <= MaxHits);

            // Find the "Best Hit", which is the closest contact to the player.
            int bestHitIndex = -1;
            float closestDistance = float.PositiveInfinity;

            for (int i = 0; i < hits; i++)
            {
                var cur = _hits[i];

                // We cannot stand on triggers, or our own collider, so early out.
                if (cur.collider.isTrigger || cur.transform == transform)
                {
                    continue;
                }

                // We can only stand on slopes with the desired steepness
                Vector3 upwardsDirection = -gravityDirection;
                Vector3 normalDirection = cur.normal;
                float slopeAngle = Vector3.Angle(upwardsDirection, normalDirection);

                // We only want to check the nearest collider we hit.
                if (slopeAngle <= slopeLimitDegrees && cur.distance < closestDistance)
                {
                    bestHitIndex = i;
                    closestDistance = cur.distance;
                }
            }

            if (bestHitIndex >= 0)
            {
                IsGrounded = true;
                ConnectedCollider = _hits[bestHitIndex].collider;
                ContactNormal = _hits[bestHitIndex].normal;
            }
            else
            {
                IsGrounded = false;
                ConnectedCollider = null;
                ContactNormal = Vector3.zero;
            }

            if (showDebug)
            {
                var t = transform;
                var pos = t.position;
                DebugDrawTools.Arrow(pos, pos - (t.up * groundDistance), IsGrounded ? Color.green : Color.red, 0);
            }
        }

        private void OnGUI()
        {
            if (showDebug)
            {
                string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";

                GUILayout.Label($"IsGrounded: {IsGrounded}");
                GUILayout.Label($"Was Grounded Last Frame: {_wasGroundedLastFrame}");
                GUILayout.Label($"Connected Collider: {connectedCollider}");
                GUILayout.Label($"Contact Normal: {ContactNormal}");
                GUILayout.Label($"Time spent grounded: {TimeSpentGrounded}");
                GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
            }
        }
    }
}
