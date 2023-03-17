namespace Application.Gameplay.Dialogue
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A view of an image that can be displayed with yarn.
    /// </summary>
    public class YarnImageView : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private CanvasGroup canvasGroup;

        /// <summary>
        /// Gets a unity UI image.
        /// </summary>
        public Image Image => image;

        /// <summary>
        /// Gets a unity UI canvas group for this image.
        /// </summary>
        public CanvasGroup CanvasGroup => canvasGroup;
    }
}
