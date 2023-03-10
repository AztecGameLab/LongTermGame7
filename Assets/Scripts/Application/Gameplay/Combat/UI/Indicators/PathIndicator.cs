namespace Application.Gameplay.Combat
{
    using System;
    using UnityEngine;

    /// <summary>
    /// A visual indicator to draw a path at runtime.
    /// </summary>
    public class PathIndicator : MonoBehaviour
    {
        private readonly Vector3[] _empty = Array.Empty<Vector3>();

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Color validColor;

        [SerializeField]
        private Color invalidColor;

        [SerializeField]
        private float animationSpeed = 5f;

        private Vector3[] _targetCorners;
        private int _currentLength = 1;
        private Vector3[] _corners = { Vector3.zero };

        /// <summary>
        /// Gets or sets a value indicating whether this path is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Updates the path indicator with a new set of vertex information.
        /// </summary>
        /// <param name="corners">The corners of the path.</param>
        public void RenderPath(Vector3[] corners)
        {
            if (corners != null)
            {
                if (corners.Length > _currentLength)
                {
                    var diff = corners.Length - _currentLength;

                    Vector3[] newCorners = new Vector3[_currentLength + diff];

                    _corners.CopyTo(newCorners, 0);

                    for (int i = _currentLength; i < _currentLength + diff; i++)
                    {
                        newCorners[i] = _corners.Length <= 0 ? corners[0] : _corners[_currentLength - 1];
                    }

                    _corners = newCorners;
                    _currentLength += diff;

                    lineRenderer.positionCount = _currentLength;
                    lineRenderer.SetPositions(_corners);
                }

                _targetCorners = corners;
            }
        }

        /// <summary>
        /// Removes all vertices from the path.
        /// </summary>
        public void ClearPath()
        {
            _currentLength = 0;
            _targetCorners = _empty;
            _corners = _empty;
            lineRenderer.SetPositions(_empty);
        }

        private void Update()
        {
            float t = animationSpeed * Time.deltaTime;

            if (_targetCorners.Length > 0)
            {
                for (int i = 0; i < _currentLength; i++)
                {
                    Vector3 target = _targetCorners[Mathf.Min(_targetCorners.Length - 1, i)];
                    _corners[i] = Vector3.Lerp(_corners[i], target, t);
                }

                lineRenderer.positionCount = _currentLength;
                lineRenderer.SetPositions(_corners);
            }
            else
            {
                lineRenderer.positionCount = 0;
            }

            Color targetColor = IsValid ? validColor : invalidColor;
            lineRenderer.material.color = Color.Lerp(lineRenderer.material.color, targetColor, t);
        }
    }
}
