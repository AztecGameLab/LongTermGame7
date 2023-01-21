namespace Application.Gameplay
{
    using Cinemachine;
    using poetools;
    using UnityEngine;

    [RequireComponent(typeof(Trigger))]
    public class CameraTrigger : MonoBehaviour
    {
        public static readonly int ActivePriority = 50;

        [SerializeField]
        private CinemachineVirtualCamera targetCamera;

        public void SetCamera(CinemachineVirtualCamera newCamera)
        {
            targetCamera = newCamera;
        }

        private void Awake()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter.AddListener(HandleCollisionEnter);
            trigger.CollisionExit.AddListener(HandleCollisionExit);
        }

        private void HandleCollisionEnter(Collider col)
        {
            targetCamera.Priority = ActivePriority;
        }

        private void HandleCollisionExit(Collider col)
        {
            targetCamera.Priority = 0;
        }
    }
}
