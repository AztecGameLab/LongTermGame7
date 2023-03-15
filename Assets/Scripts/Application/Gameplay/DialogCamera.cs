﻿namespace Application.Gameplay
{
    using Cinemachine;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn camera commands.
    /// </summary>
    public class DialogCamera : MonoBehaviour
    {
        private const int ActivePriority = 10;

        [SerializeField]
        private CinemachineVirtualCamera dialogueCamera;

        private CinemachineVirtualCamera _cam;
        private CinemachineFramingTransposer _camFramingTransposer;
        private Transform _camTrans;
        private Quaternion _startRotation;
        private Quaternion _endRotation;
        private bool _isRotating;
        private float _rotationDuration = 1.0f;
        private float _rotationTime;

        private Quaternion _originalRotation;
        private Vector3 _startMovement;
        private Vector3 _endMovement;
        private bool _isMoving;
        private float _movementDuration = 1.0f;
        private float _movementTime;

        /// <summary>
        /// Ensure that the duration is positive, warn if it is not.
        /// </summary>
        /// <param name="input">The duration to check.</param>
        /// <returns>The absolute value of the duration.</returns>
        private static float MakePositive(float input)
        {
            if (input < 0)
            {
                Debug.LogWarning("DialogCamera: A negative duration was provided from Yarn!");
                return System.Math.Abs(input);
            }

            return input;
        }

        /// <summary>
        /// Make the camera Look At the target game object. You must be using a camera with Aim that supports this.
        /// </summary>
        /// <param name="target">The name of the game object to look at.</param>
        private void LookAt(GameObject target)
        {
            if (target == null)
            {
                Debug.LogWarning("Unable to find LookAt target from yarn!");
                return;
            }

            UpdateEndState();
            _cam.LookAt = target.transform;
            _cam.AddCinemachineComponent<CinemachineComposer>();
        }

        /// <summary>
        /// Make the camera Follow the target game object. You must be using a camera with Aim that supports this.
        /// </summary>
        /// <param name="target">The name of the game object to follow.</param>
        private void Follow(GameObject target)
        {
            if (target == null)
            {
                Debug.LogWarning("Unable to find Follow target from yarn!");
                return;
            }

            UpdateEndState();
            _cam.Follow = target.transform;
        }

        /// <summary>
        /// Swivel the camera relative to the current rotation. If the camera is already rotating, the target rotation
        /// is relative to where that rotation would have ended.
        /// </summary>
        /// <param name="x">x.</param>
        /// <param name="y">y.</param>
        /// <param name="z">z.</param>
        /// <param name="timeScalar">Optional: The time to complete the swivel in seconds, default is 0 - instant.</param>
        private void SwivelRel(float x, float y, float z, float timeScalar = 0)
        {
            timeScalar = MakePositive(timeScalar);
            UpdateEndState();

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
        /// <param name="timeScalar">Optional: The time to complete the swivel in seconds, default is 0 - instant.</param>
        private void SwivelAbs(float x, float y, float z, float timeScalar = 0)
        {
            timeScalar = MakePositive(timeScalar);
            UpdateEndState();
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
        /// <param name="timeScalar">Optional: The time to complete the movement in seconds, default is 0 - instant.</param>
        private void MoveRel(float x, float y, float z, float timeScalar = 0)
        {
            timeScalar = MakePositive(timeScalar);
            UpdateEndState();

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
        /// <param name="timeScalar">Optional: The time to complete the movement in seconds, default is 0 - instant.</param>
        private void MoveAbs(float x, float y, float z, float timeScalar = 0)
        {
            timeScalar = MakePositive(timeScalar);
            UpdateEndState();
            _startMovement = _camFramingTransposer.m_TrackedObjectOffset;

            Vector3 newMovement = new Vector3(x, y, z);
            _endMovement = newMovement;
            _movementDuration = timeScalar;
            _isMoving = true;
        }

        private void ResetRotation()
        {
            _cam.LookAt = null;
            _cam.transform.rotation = _originalRotation;
            _cam.DestroyCinemachineComponent<CinemachineComposer>();
        }

        /// <summary>
        /// Find the current active Cinemachine camera and update the private variables.
        /// </summary>
        private void UpdateEndState()
        {
            // Set _end values if not in the middle of moving to handle if a rel is called first
            if (!_isMoving)
            {
                _endMovement = _camFramingTransposer.m_TrackedObjectOffset;
            }

            if (!_isRotating)
            {
                _endRotation = _camTrans.rotation;
            }
        }

        private void Awake()
        {
            _originalRotation = dialogueCamera.transform.rotation;

            var dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler<float, float, float, float>("cam-offset-abs", MoveAbs);
            dialogueRunner.AddCommandHandler<float, float, float, float>("cam-offset-rel", MoveRel);
            dialogueRunner.AddCommandHandler<float, float, float, float>("cam-swivel-abs", SwivelAbs);
            dialogueRunner.AddCommandHandler<float, float, float, float>("cam-swivel-rel", SwivelRel);
            dialogueRunner.AddCommandHandler<GameObject>("cam-follow", Follow);
            dialogueRunner.AddCommandHandler<GameObject>("cam-lookAt", LookAt);
            dialogueRunner.AddCommandHandler("cam-reset-rotation", ResetRotation);

            dialogueRunner.onNodeStart.AddListener(ActivateDialogueCamera);
            dialogueRunner.onNodeComplete.AddListener(DeactivateDialogueCamera);

            _cam = dialogueCamera;
            _camTrans = _cam.transform;
            _camFramingTransposer = _cam.GetComponentInChildren<CinemachineFramingTransposer>();
        }

        private void OnDestroy()
        {
            var dialogueRunner = FindObjectOfType<DialogueRunner>();

            if (dialogueRunner != null)
            {
                dialogueRunner.RemoveCommandHandler("cam-offset-abs");
                dialogueRunner.RemoveCommandHandler("cam-offset-rel");
                dialogueRunner.RemoveCommandHandler("cam-swivel-abs");
                dialogueRunner.RemoveCommandHandler("cam-swivel-rel");
                dialogueRunner.RemoveCommandHandler("cam-follow");
                dialogueRunner.RemoveCommandHandler("cam-lookAt");
                dialogueRunner.RemoveCommandHandler("cam-reset-rotation");

                dialogueRunner.onNodeStart.RemoveListener(ActivateDialogueCamera);
                dialogueRunner.onNodeComplete.RemoveListener(DeactivateDialogueCamera);
            }
        }

        private void ActivateDialogueCamera(string node)
        {
            dialogueCamera.Priority = ActivePriority;
        }

        private void DeactivateDialogueCamera(string node)
        {
            dialogueCamera.Priority = 0;
        }

        private void FixedUpdate()
        {
            if (_isRotating)
            {
                if (_rotationTime >= _rotationDuration)
                {
                    _isRotating = false;
                    _rotationTime = 0.0f;

                    // Snap to the final rotation as Slerp won't go all the way.
                    _camTrans.rotation = _endRotation;
                    _cam.PreviousStateIsValid = false;
                }
                else
                {
                    _camTrans.rotation = Quaternion.Slerp(_startRotation, _endRotation, _rotationTime / _rotationDuration);

                    // Apply the time scalar to adjust how far to progress the Slerp.
                    _rotationTime += Time.deltaTime;
                }
            }

            if (_isMoving)
            {
                if (_movementTime >= _movementDuration)
                {
                    _isMoving = false;
                    _movementTime = 0.0f;

                    // Snap to the final offset as Slerp won't go all the way.
                    // Remove damping so this is instant, but remember the original values.
                    _camFramingTransposer.m_TrackedObjectOffset = _endMovement;
                    _cam.PreviousStateIsValid = false;
                }
                else
                {
                    _camFramingTransposer.m_TrackedObjectOffset = Vector3.Slerp(_startMovement, _endMovement, _movementTime / _movementDuration);

                    // Apply the time scalar to adjust how far to progress the Slerp.
                    _movementTime += Time.deltaTime;
                }
            }
        }
    }
}