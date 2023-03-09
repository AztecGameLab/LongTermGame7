namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// Allows level designer's to define skybox information in levels.
    /// </summary>
    public class CameraSkyboxAuthoring : MonoBehaviour
    {
        [SerializeField]
        private CameraClearFlags flags;

        [SerializeField]
        private Color color;

        private void Start()
        {
            foreach (Camera targetCamera in FindObjectsOfType<Camera>())
            {
                targetCamera.clearFlags = flags;
                targetCamera.backgroundColor = color;
            }
        }
    }
}
