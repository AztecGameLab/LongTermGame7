namespace Application.Vfx
{
    using UnityEngine;

    /// <summary>
    /// Used for testing whether the Fade Transition is working or now.
    /// </summary>
    public class FadeTransitionTester : MonoBehaviour
    {
        [SerializeField]
        private float showTime;

        [SerializeField]
        private float hideTime;

        [SerializeField]
        private CanvasGroup blackImage;

        private FadeTransition _fadeTransition;

        private void Awake()
        {
            _fadeTransition = new FadeTransition(showTime, hideTime, blackImage);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Show Fade"))
            {
                StartCoroutine(_fadeTransition.ShowEffect());
            }

            if (GUILayout.Button("Hide Fade"))
            {
                StartCoroutine(_fadeTransition.HideEffect());
            }
        }
    }
}
