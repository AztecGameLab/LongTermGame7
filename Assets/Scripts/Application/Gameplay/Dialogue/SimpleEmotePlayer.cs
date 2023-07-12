namespace Application.Gameplay.Dialogue
{
    using System.Collections;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using UnityEngine;
    using UnityEngine.Animations;

    /// <summary>
    /// A basic implementation of an EmotePlayer, that simply fades in and repositions emotes.
    /// </summary>
    public class SimpleEmotePlayer : EmotePlayer
    {
        [SerializeField]
        private CanvasGroup emoteCanvasGroup;

        [SerializeField]
        private float duration = 0.5f;

        [SerializeField]
        private float holdTime = 0.5f;

        [SerializeField]
        private EaseType easeType = EaseType.CubicInOut;

        [SerializeField]
        private ParentConstraint parentConstraint;

        [SerializeField]
        private Transform emoteSlideTransform;

        [SerializeField]
        private float slideAmount = 0.5f;

        private WaitForSeconds _holdWaitForSeconds;
        private float _initialY;

        /// <inheritdoc/>
        protected override IEnumerator AnimationCoroutine(GameObject target)
        {
            int source = parentConstraint.AddSource(new ConstraintSource { sourceTransform = target.transform, weight = 1 });

            emoteCanvasGroup.TweenCanvasGroupAlpha(1, duration)
                .SetEase(easeType);

            yield return emoteSlideTransform.TweenLocalPositionY(_initialY, duration)
                .SetFrom(_initialY - slideAmount)
                .SetEase(easeType)
                .Yield();

            yield return _holdWaitForSeconds;

            yield return emoteCanvasGroup.TweenCanvasGroupAlpha(0, duration)
                .SetEase(easeType);

            yield return emoteSlideTransform.TweenLocalPositionY(_initialY + slideAmount, duration)
                .SetFrom(_initialY)
                .SetEase(easeType)
                .Yield();

            parentConstraint.RemoveSource(source);
        }

        private void Awake()
        {
            _holdWaitForSeconds = new WaitForSeconds(holdTime);
            _initialY = emoteSlideTransform.localPosition.y;
        }
    }
}
