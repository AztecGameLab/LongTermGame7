namespace Application.Gameplay
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using Yarn.Unity;

    /// <summary>
    /// Provides yarn commands to fading the screen.
    /// </summary>
    public class FadeIntoBlack : MonoBehaviour
    {
        [SerializeField]
        private Image blackOutSquare;

        /// <summary>
        /// Causes screen to fade into black. When yarn spinner uses "false, fade out of black.
        /// </summary>
        /// <param name="fadeToBlack">Should the screen fade in, or out.</param>
        /// <param name="fadeSpeed">How long should it take to fade.</param>
        /// <returns>Unity coroutine.</returns>
        // [YarnCommand("fade_camera")]
        public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 1)
        {
            Color objectColor = blackOutSquare.color;
            float fadeAmount;

            if (fadeToBlack)
            {
                while (blackOutSquare.color.a < 1)
                {
                    fadeAmount = objectColor.a + (1.0f / fadeSpeed * Time.deltaTime);
                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    blackOutSquare.color = objectColor;
                    yield return null;
                }
            }

            // If we are fading OUT of black
            else
            {
                while (blackOutSquare.color.a > 0)
                {
                    fadeAmount = objectColor.a - (1.0f / fadeSpeed * Time.deltaTime);
                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    blackOutSquare.color = objectColor;
                    yield return null;
                }
            }
        }

        private Coroutine StartFadeOut(float fadeSpeed = 1)
        {
            return StartCoroutine(FadeBlackOutSquare(true, fadeSpeed));
        }

        private Coroutine StartFadeIn(float fadeSpeed = 1)
        {
            return StartCoroutine(FadeBlackOutSquare(false, fadeSpeed));
        }

        private void Awake()
        {
            var dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.AddCommandHandler<float>("cam-fade-out", StartFadeOut);
            dialogueRunner.AddCommandHandler<float>("cam-fade-in", StartFadeIn);
        }

        private void OnDestroy()
        {
            var dialogueRunner = FindObjectOfType<DialogueRunner>();
            dialogueRunner.RemoveCommandHandler("cam-fade-out");
            dialogueRunner.RemoveCommandHandler("cam-fade-in");
        }
    }
}
