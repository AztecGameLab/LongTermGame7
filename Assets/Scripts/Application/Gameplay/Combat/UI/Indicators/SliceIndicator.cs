namespace Application.Gameplay.Combat.UI.Indicators
{
    using Unity.Collections;
    using UnityEngine;

    /// <summary>
    /// An indicator that shows a slice of attack range.
    /// </summary>
    public class SliceIndicator : MonoBehaviour
    {
        [SerializeField]
        private MeshFilter meshFilter;

        /// <summary>
        /// Updates the visual representation of this indicator.
        /// </summary>
        /// <param name="origin">The starting point of the slice.</param>
        /// <param name="direction">The direction of the slice.</param>
        /// <param name="radius">How far out the slice should go.</param>
        /// <param name="spread">How wide the slice should be, in degrees.</param>
        public void UpdateView(Vector3 origin, Vector3 direction, float radius, float spread)
        {
            // do the math to calculate positions.
            var theta = Mathf.Deg2Rad * ((180 - spread) / 2);
            var x = radius * Mathf.Sin(theta);
            var y = radius * Mathf.Cos(theta);

            // dynamically generate the mesh
            NativeArray<Vector3> vertices = new NativeArray<Vector3>(3, Allocator.Temp);
            vertices[0] = Vector3.zero;
            vertices[1] = new Vector3(x, 0, y);
            vertices[2] = new Vector3(x, 0, -y);
            meshFilter.mesh.SetVertices(vertices);

            transform.SetPositionAndRotation(origin, Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0));
        }

        private void Awake()
        {
            var mesh = new Mesh();
            mesh.SetVertices(new Vector3[3]);
            mesh.SetIndices(new[] { 0, 1, 2 }, MeshTopology.Triangles, 0);
            meshFilter.mesh = mesh;
        }
    }
}
