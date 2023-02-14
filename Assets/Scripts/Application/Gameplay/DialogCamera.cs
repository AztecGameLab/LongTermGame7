namespace Application.Gameplay
{
    using Cinemachine;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn camera commands.
    /// </summary>
    public class DialogCamera : MonoBehaviour
    {
        private static CinemachineVirtualCamera _cam;
        private static CinemachineFramingTransposer _camFramingTransposer;
        private static Transform _camTrans;
        private static Quaternion _startRotation;
        private static Quaternion _endRotation;
        private static bool _isRotating;
        private static float _rotationDuration = 1.0f;
        private static float _rotationTime;

        private static Vector3 _startMovement;
        private static Vector3 _endMovement;
        private static bool _isMoving;
        private static float _movementDuration = 1.0f;
        private static float _movementTime;

        /// <summary>
        /// Make the camera Look At the target game object. You must be using a camera with Aim that supports this.
        /// </summary>
        /// <param name="target">The name of the game object to look at.</param>
        [YarnCommand("cam-look-at")]
        public static void LookAt(GameObject target)
        {
            if (target == null)
            {
                Debug.LogWarning("Unable to find LookAt target from yarn!");
                return;
            }

            FindActiveCamera();
            _cam.LookAt = target.transform;
        }

        /// <summary>
        /// Make the camera Follow the target game object. You must be using a camera with Aim that supports this.
        /// </summary>
        /// <param name="target">The name of the game object to follow.</param>
        [YarnCommand("cam-follow")]
        public static void Follow(GameObject target)
        {
            if (target == null)
            {
                Debug.LogWarning("Unable to find Follow target from yarn!");
                return;
            }

            FindActiveCamera();
            _cam.Follow = target.transform;
        }

        /// <summary>
        /// Swivel the camera relative to the current rotation. If the camera is already rotating, the target rotation
        /// is relative to where that rotation would have ended.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <param name="timeScalar">Optional: The time to complete the swivel in seconds, default is 1.</param>
        [YarnCommand("swivel-rel")]
        public static void SwivelRel(float x, float y, float z, float timeScalar = 1)
        {
            FindActiveCamera();

            // Stop a rotation if it is in progress
            _isRotating = false;
            _rotationTime = 0;

            _startRotation = _camTrans.rotation;
            _endRotation = _startRotation;

            // If a rotation was in progress, _camTrans != _endRotation. It is assumed the yarn author wants the
            // current rotation to finish before applying a relative rotation. So, we calculate our target rotation
            // based on where the current one should have ended, but without any visible snapping.
            // If a rotation is not in progress, then _camTrans = _endRotation and has the expected effect.
            Vector3 newRotation = _endRotation.eulerAngles;
            newRotation += new Vector3(x, y, z);

            _endRotation.eulerAngles = newRotation;
            _rotationDuration = timeScalar;
            _isRotating = true;
        }

        /// <summary>
        /// Swivel the camera to the new rotation.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <param name="timeScalar">Optional: The time to complete the swivel in seconds, default is 1.</param>
        [YarnCommand("swivel-abs")]
        public static void SwivelAbs(float x, float y, float z, float timeScalar = 1)
        {
            FindActiveCamera();
            _startRotation = _camTrans.rotation;
            _endRotation = _startRotation;

            Vector3 newRotation = new Vector3(x, y, z);
            _endRotation.eulerAngles = newRotation;
            _rotationDuration = timeScalar;
            _isRotating = true;
        }

        /// <summary>
        /// Set the camera offset relative to the current offset. If the camera is already moving, the target movement
        /// is relative to where that movement would have ended.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <param name="timeScalar">Optional: The time to complete the movement in seconds, default is 1.</param>
        [YarnCommand("cam-offset-rel")]
        public static void MoveRel(float x, float y, float z, float timeScalar = 1)
        {
            FindActiveCamera();

            _isMoving = false;
            _movementTime = 0;

            _startMovement = _camFramingTransposer.m_TrackedObjectOffset;

            // If a movement was in progress, _camTrans != _endMovement. It is assumed the yarn author wants the
            // current movement to finish before applying a relative movement. So, we calculate our target movement
            // based on where the current one should have ended, but without any visible snapping.
            // If a movement is not in progress, then _camTrans = _endMovement and has the expected effect.
            Vector3 newMovement = new Vector3(x, y, z);
            _endMovement += newMovement;
            _movementDuration = timeScalar;
            _isMoving = true;
        }

        /// <summary>
        /// Set the camera offset to the new offset.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <param name="timeScalar">Optional: The time to complete the movement in seconds, default is 1.</param>
        [YarnCommand("cam-offset-abs")]
        public static void MoveAbs(float x, float y, float z, float timeScalar = 1)
        {
            FindActiveCamera();
            _startMovement = _camFramingTransposer.m_TrackedObjectOffset;

            Vector3 newMovement = new Vector3(x, y, z);
            _endMovement = newMovement;
            _movementDuration = timeScalar;
            _isMoving = true;
        }

        /// <summary>
        /// Find the current active Cinemachine camera and update the private variables.
        /// </summary>
        private static void FindActiveCamera()
        {
            CinemachineVirtualCamera[] cams = Object.FindObjectsOfType<CinemachineVirtualCamera>();
            foreach (CinemachineVirtualCamera c in cams)
            {
                if (c.isActiveAndEnabled)
                {
                    _cam = c;
                    _camTrans = c.transform;
                    _camFramingTransposer = _cam.GetComponentInChildren<CinemachineFramingTransposer>();

                    // Set _end values if not in the middle of moving to handle if a rel is called first
                    if (!_isMoving)
                    {
                        _endMovement = _camFramingTransposer.m_TrackedObjectOffset;
                    }

                    if (!_isRotating)
                    {
                        _endRotation = _camTrans.rotation;
                    }

                    return;
                }
            }
        }

        private void FixedUpdate()
        {
            if (_isRotating)
            {
                _camTrans.rotation = Quaternion.Slerp(_startRotation, _endRotation, _rotationTime / _rotationDuration);

                // Apply the time scalar to adjust how far to progress the Slerp.
                _rotationTime += Time.deltaTime;
                if (_rotationTime >= _rotationDuration)
                {
                    _isRotating = false;
                    _rotationTime = 0.0f;

                    // Snap to the final rotation as Slerp won't go all the way.
                    _camTrans.rotation = _endRotation;
                }
            }

            if (_isMoving)
            {
                _camFramingTransposer.m_TrackedObjectOffset = Vector3.Slerp(_startMovement, _endMovement, _movementTime / _movementDuration);

                // Apply the time scalar to adjust how far to progress the Slerp.
                _movementTime += Time.deltaTime;
                if (_movementTime >= _movementDuration)
                {
                    _isMoving = false;
                    _movementTime = 0.0f;

                    // Snap to the final offset as Slerp won't go all the way.
                    _camFramingTransposer.m_TrackedObjectOffset = _endMovement;
                }
            }
        }
    }
}