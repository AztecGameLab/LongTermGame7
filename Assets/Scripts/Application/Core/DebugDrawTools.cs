namespace Application.Core
{
    using UnityEngine;

    /// <summary>
    /// Utilities for drawing special debugging information into the scene.
    /// </summary>
    public static class DebugDrawTools
    {
        /// <summary>
        /// Draws a triangle in the scene.
        /// </summary>
        /// <param name="ray">The direction to point.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="color">The color.</param>
        /// <param name="time">How long it should remain visible for.</param>
        /// <param name="depthTest">Whether it should be occluded by other objects or "XRay" through them.</param>
        public static void Triangle(Ray ray, float width, float height, Color color, float time = 1.0f, bool depthTest = false)
        {
            var matrix = Matrix4x4.identity;
            matrix *= Matrix4x4.Translate(ray.origin);
            matrix *= Matrix4x4.Rotate(Quaternion.FromToRotation(Vector3.up, ray.direction));

            var leftCorner = matrix.MultiplyPoint(-Vector3.right * (width / 2));
            var rightCorner = matrix.MultiplyPoint(Vector3.right * (width / 2));
            var topCorner = matrix.MultiplyPoint(Vector3.up * height);

            Debug.DrawLine(leftCorner, rightCorner, color, time, depthTest);
            Debug.DrawLine(rightCorner, topCorner, color, time, depthTest);
            Debug.DrawLine(topCorner, leftCorner, color, time, depthTest);
        }

        /// <summary>
        /// Draws an arrow in the scene.
        /// </summary>
        /// <param name="start">The beginning of the arrow.</param>
        /// <param name="end">The end of the arrow.</param>
        /// <param name="color">The color.</param>
        /// <param name="time">How long it should remain visible for.</param>
        /// <param name="depthTest">Whether it should be occluded by other objects or "XRay" through them.</param>
        public static void Arrow(Vector3 start, Vector3 end, Color color, float time = 1.0f, bool depthTest = false)
        {
            const float width = 0.25f;
            const float height = 0.25f;

            Vector3 direction = (end - start).normalized;
            Debug.DrawLine(start, end, color, time, false);
            Triangle(new Ray(end, direction), width, height, color, time, depthTest);
        }
    }
}
