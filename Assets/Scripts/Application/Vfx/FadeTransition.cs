namespace Application.Vfx
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// A transition effect that fades an image over the screen.
    /// </summary>
    public class FadeTransition : SceneTransition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FadeTransition"/> class.
        /// </summary>
        /// <param name="showTime">How long it should take to completely fade-to-black.</param>
        /// <param name="hideTime">How long it should take to completely fade-from-black.</param>
        /// <param name="blackImage">The CanvasGroup that should be faded, obscuring the screen.</param>
        public FadeTransition(float showTime, float hideTime, CanvasGroup blackImage)
        {
            // TODO: integrate these variables into the logic of the fade transition.
        }

        /// <inheritdoc/>
        public override IEnumerator ShowEffect()
        {
            // TODO: Make a black image on the canvas slowly gain it's transparency.
            yield return null;
        }

        /// <inheritdoc/>
        public override IEnumerator HideEffect()
        {
            // TODO: Make a black image on the canvas slowly lose it's transparency.
            yield return null;
        }
    }
}
