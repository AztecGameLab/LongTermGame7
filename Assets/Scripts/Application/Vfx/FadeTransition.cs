namespace Application.Vfx
{
    using System.Collections;
    using ElRaccoone.Tweens;
    using UnityEngine;

    /// <summary>
    /// A transition effect that fades an image over the screen.
    /// </summary>
    public class FadeTransition : SceneTransition
    {
        private readonly CanvasGroup _blackImage;
        private readonly float _showTime;
        private readonly float _hideTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="FadeTransition"/> class.
        /// </summary>
        /// <param name="showTime">How long it should take to completely fade-to-black.</param>
        /// <param name="hideTime">How long it should take to completely fade-from-black.</param>
        /// <param name="blackImage">The CanvasGroup that should be faded, obscuring the screen.</param>
        public FadeTransition(float showTime, float hideTime, CanvasGroup blackImage)
        {
            _blackImage = blackImage;
            _showTime = showTime;
            _hideTime = hideTime;
        }

        /// <inheritdoc/>
        protected override IEnumerator ShowEffectCoroutine()
        {
            yield return _blackImage.TweenCanvasGroupAlpha(1, _showTime).Yield();
        }

        /// <inheritdoc/>
        protected override IEnumerator HideEffectCoroutine()
        {
            yield return _blackImage.TweenCanvasGroupAlpha(0, _hideTime).Yield();
        }
    }
}
