namespace Application.Gameplay.Combat.UI.Indicators
{
    using UnityEngine;

    /// <summary>
    /// An indicator for displaying arrows.
    /// </summary>
    public class ArrowIndicator : MonoBehaviour
    {
        private readonly Vector3[] _linePositions = new Vector3[2];

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private GameObject arrowHead;

        /// <summary>
        /// Updates the visual state of this indicator based on the start and end positions.
        /// </summary>
        /// <param name="start">The position to begin the arrow.</param>
        /// <param name="end">The position to end the arrow.</param>
        public void UpdateView(Vector3 start, Vector3 end)
        {
            _linePositions[0] = start;
            _linePositions[1] = end;
            lineRenderer.SetPositions(_linePositions);

            arrowHead.transform.rotation = Quaternion.LookRotation(end - start, Vector3.up);
            arrowHead.transform.position = end;
        }

        /// <summary>
        /// Updates the visual state of this indicator based on a ray.
        /// </summary>
        /// <param name="ray">The ray to base the indicator off of.</param>
        public void UpdateView(Ray ray)
        {
            UpdateView(ray.origin, ray.origin + ray.direction);
        }
    }
}
