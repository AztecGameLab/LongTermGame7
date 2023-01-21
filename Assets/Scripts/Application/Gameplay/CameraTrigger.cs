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
        private new CinemachineVirtualCamera camera;

        public void SetCamera(CinemachineVirtualCamera newCamera)
        {
            camera = newCamera;
        }

        private void Awake()
        {
            var trigger = GetComponent<Trigger>();
            trigger.CollisionEnter.AddListener(HandleCollisionEnter);
            trigger.CollisionExit.AddListener(HandleCollisionExit);
        }

        private void HandleCollisionEnter(Collider col)
        {
            camera.Priority = ActivePriority;
        }

        private void HandleCollisionExit(Collider col)
        {
            camera.Priority = 0;
        }
    }
}
