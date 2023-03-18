using UnityEngine;

namespace Application.Vfx.Animation
{
    /// <summary>
    /// Keep track of the current velocity of an object, based on its positions.
    /// </summary>
    public class VelocityTracker
    {
        private bool _isInitialized;

        /// <summary>
        /// Gets the current velocity of this object.
        /// </summary>
        public Vector3 Velocity { get; private set; }

        /// <summary>
        /// Gets the position of this object before <see cref="Update"/> was called.
        /// </summary>
        public Vector3 PreviousPosition { get; private set; }

        /// <summary>
        /// Gets the position of this object after <see cref="Update"/> was called.
        /// </summary>
        public Vector3 CurrentPosition { get; private set; }

        /// <summary>
        /// Sets an initial position for this tracker.
        /// Can prevent incorrect velocity readings when abrupt changes are made.
        /// </summary>
        /// <param name="position">The initial position.</param>
        public void Initialize(Vector3 position)
        {
            CurrentPosition = position;
            PreviousPosition = position;
            _isInitialized = true;
        }

        /// <summary>
        /// Updates the state of this velocity tracker. Should be called frequently
        /// for accurate results.
        /// </summary>
        /// <param name="currentPosition">The current position of the tracked object.</param>
        /// <param name="deltaTime">The time since the previous frame.</param>
        public void Update(Vector3 currentPosition, float deltaTime)
        {
            if (!_isInitialized)
            {
                PreviousPosition = currentPosition;
                _isInitialized = true;
            }

            CurrentPosition = currentPosition;
            Velocity = (CurrentPosition - PreviousPosition) / deltaTime;
            PreviousPosition = CurrentPosition;
        }
    }
}