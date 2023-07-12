namespace Application.Vfx
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// Binds an instance of the outliner to the global Services.
    /// </summary>
    public class OutlinerInitializer : MonoBehaviour
    {
        [SerializeField]
        private Material outlineMaterial;

        private void Start()
        {
            Services.Outliner = new Outliner(outlineMaterial);
        }
    }
}
