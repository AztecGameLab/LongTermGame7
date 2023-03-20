using System;
using Unity.Collections;

namespace Application.Gameplay.Combat.UI.Indicators
{
    using UnityEngine;

    public class SliceIndicator : MonoBehaviour
    {
        [SerializeField] private MeshFilter meshFilter;

        private NativeArray<Vector3> _vertices;

        private void Awake()
        {
            var mesh = new Mesh();
            _vertices = new NativeArray<Vector3>(3, Allocator.Persistent);
            mesh.SetVertices(_vertices);
            mesh.SetIndices(new[] { 0, 1, 2 }, MeshTopology.Triangles, 0);
            meshFilter.mesh = mesh;
        }

        public void UpdateView(Vector3 origin, Vector3 direction, float radius, float spread)
        {
            var theta = Mathf.Deg2Rad * ((180 - spread) / 2);
            var x = radius * Mathf.Sin(theta);
            var y = radius * Mathf.Cos(theta);

            _vertices[0] = Vector3.zero;
            _vertices[1] = new Vector3(x, 0, y);
            _vertices[2] = new Vector3(x, 0, -y);

            transform.position = origin;
            transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);

            meshFilter.mesh.SetVertices(_vertices);
        }
    }
}
