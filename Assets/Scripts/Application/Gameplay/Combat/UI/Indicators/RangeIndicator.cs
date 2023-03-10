namespace Application.Gameplay.Combat.UI.Indicators
{
    using UnityEngine;

    /// <summary>
    /// A visual indicator for the range of something.
    /// </summary>
    public class RangeIndicator : MonoBehaviour
    {
        [SerializeField]
        private Transform scaleTarget;

        [SerializeField]
        private float animationSpeed = 15;

        /// <summary>
        /// Gets or sets the range that this indicator will display.
        /// </summary>
        public float Range { get; set; } = 1;

        private void Update()
        {
            var localScale = scaleTarget.localScale;
            var targetScale = new Vector3(Range * 2, localScale.y, Range * 2);
            localScale = Vector3.Lerp(localScale, targetScale, animationSpeed * Time.deltaTime);
            scaleTarget.localScale = localScale;
        }
    }
}
