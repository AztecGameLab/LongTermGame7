namespace Application.Vfx
{
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    /// Creates a new, invisible shadow-only mesh.
    /// </summary>
    public class ShadowCaster : MonoBehaviour
    {
        [SerializeField]
        private Material material;

        private void Awake()
        {
            var originalFilter = GetComponent<MeshFilter>();
            var originalRenderer = GetComponent<MeshRenderer>();
            originalRenderer.shadowCastingMode = ShadowCastingMode.Off;

            var shadowCasterObject = new GameObject($"{gameObject.name}'s Shadow Caster");
            shadowCasterObject.transform.SetParent(transform, false);

            var newRenderer = shadowCasterObject.AddComponent<MeshRenderer>();
            var newFilter = shadowCasterObject.AddComponent<MeshFilter>();
            newFilter.sharedMesh = originalFilter.sharedMesh;
            newRenderer.material = material;
            newRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }
}
