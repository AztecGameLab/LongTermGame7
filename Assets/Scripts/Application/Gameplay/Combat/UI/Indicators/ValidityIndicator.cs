namespace Application.Gameplay.Combat.UI.Indicators
{
    using UnityEngine;

    /// <summary>
    /// An indicator that can show if an action is valid or not.
    /// </summary>
    public class ValidityIndicator : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        [SerializeField]
        private Color validColor;

        [SerializeField]
        private Color invalidColor;

        [SerializeField]
        private float animationSpeed = 5f;

        /// <summary>
        /// Gets or sets a value indicating whether is this indicator is valid.
        /// </summary>
        public bool IsValid { get; set; }

        private void Update()
        {
            float t = animationSpeed * Time.deltaTime;
            Color targetColor = IsValid ? validColor : invalidColor;
            meshRenderer.material.color = Color.Lerp(meshRenderer.material.color, targetColor, t);
        }
    }
}
