namespace Levels.__TESTING_LEVELS__.Real_Demo
{
    using System.Collections;
    using ElRaccoone.Tweens;
    using UnityEngine;

    public class ClickToClearPopup : Popup
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float appearDuration = 1;

        public override IEnumerator Show()
        {
            yield return group.TweenCanvasGroupAlpha(1, appearDuration);
            group.interactable = true;
            group.blocksRaycasts = true;

            while (!Input.anyKey)
            {
                yield return null;
            }

            group.interactable = false;
            group.blocksRaycasts = false;
            yield return group.TweenCanvasGroupAlpha(0, appearDuration);
        }
    }
}