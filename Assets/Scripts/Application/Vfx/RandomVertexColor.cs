using UnityEngine;
using Random = UnityEngine.Random;

namespace Application.Vfx
{
    public class RandomVertexColor : MonoBehaviour
    {
        // todo: we don't have to do this at runtime. We can make an editor utility that bakes this information.
        
        private void Awake()
        {
            var filter = GetComponent<MeshFilter>();

            var colors = new Color[filter.mesh.vertexCount];
            var r = Random.ColorHSV();

            for (int i = 0; i < filter.mesh.vertexCount; i++)
                colors[i] = r;
            
            filter.mesh.SetColors(colors);
        }
    }
}