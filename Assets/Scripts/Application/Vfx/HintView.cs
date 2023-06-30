namespace Application.Vfx
{
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using UnityEngine;

    /// <summary>
    /// A hint in game that can appear and disappear.
    /// </summary>
    public class HintView : MonoBehaviour
    {
        [SerializeField]
        private new Renderer renderer;

        [SerializeField]
        private float duration;

        [SerializeField]
        private EaseType easing = EaseType.CubicInOut;

        private Vector3 _originalScale;

        /// <summary>
        /// Show a nice animation of this hint appearing.
        /// </summary>
        public void Show()
        {
            // animation for displaying this hint.
            renderer.TweenCancelAll();
            renderer.TweenLocalScale(_originalScale, duration).SetEase(easing);
        }

        /// <summary>
        /// Show a nice animation of this hint disappearing.
        /// </summary>
        public void Hide()
        {
            // remove and destroy this hint
            renderer.TweenCancelAll();
            renderer.TweenLocalScale(Vector3.zero, duration).SetEase(easing);
        }

        private void Awake()
        {
            _originalScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }
    }
}
