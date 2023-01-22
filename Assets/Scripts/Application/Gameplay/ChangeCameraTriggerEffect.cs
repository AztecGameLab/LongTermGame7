namespace Application.Gameplay
{
    using Cinemachine;
    using Core;
    using UnityEngine;

    /// <summary>
    /// Hooks into a trigger to change the priority of a Cinemachine camera.
    /// </summary>
    [RequireComponent(typeof(Trigger))]
    public class ChangeCameraTriggerEffect : MonoBehaviour
    {
        private static readonly int ActivePriority = 50;

        [SerializeField]
        private CinemachineVirtualCamera targetCamera;

        private Trigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<Trigger>();
        }

        private void OnEnable()
        {
            _trigger.CollisionEnter += HandleCollisionEnter;
            _trigger.CollisionExit += HandleCollisionExit;
        }

        private void OnDisable()
        {
            _trigger.CollisionEnter -= HandleCollisionEnter;
            _trigger.CollisionExit -= HandleCollisionExit;
        }

        private void HandleCollisionEnter(GameObject obj)
        {
            targetCamera.Priority = ActivePriority;
        }

        private void HandleCollisionExit(GameObject obj)
        {
            targetCamera.Priority = 0;
        }
    }
}
