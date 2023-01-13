namespace Application.Vfx
{
    using UnityEngine;
    using Random = UnityEngine.Random;

    /// <summary>
    /// Randomly assigns a color to each vertex in an object.
    /// </summary>
    public class RandomVertexColor : MonoBehaviour
    {
        // I don't think we don't have to do this at runtime. We can make an editor utility that bakes this information.
        private void Awake()
        {
            var filter = GetComponent<MeshFilter>();

            Color[] colors = new Color[filter.mesh.vertexCount];
            var r = Random.ColorHSV();

            for (int i = 0; i < filter.mesh.vertexCount; i++)
            {
                colors[i] = r;
            }

            filter.mesh.SetColors(colors);
        }
    }
}
