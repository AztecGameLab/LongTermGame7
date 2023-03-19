namespace Application.Gameplay
{
    using System;
    using System.Collections;
    using Dialogue;
    using UnityEngine;
    using UnityEngine.UI;
    using Yarn.Unity;

    using Object = UnityEngine.Object;

    /// <summary>
    /// Provides yarn commands to fading the screen.
    /// </summary>
    [Serializable]
    public class FadeIntoBlack : IYarnCommandHandler
    {
        [SerializeField]
        private Image blackOutSquarePrefab;

        private MonoBehaviour _runner;
        private Image _blackOutSquare;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _runner = runner;
            _blackOutSquare = Object.Instantiate(blackOutSquarePrefab, runner.transform);

            runner.AddCommandHandler<float>("cam-fade-out", StartFadeOut);
            runner.AddCommandHandler<float>("cam-fade-in", StartFadeIn);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("cam-fade-out");
            runner.RemoveCommandHandler("cam-fade-in");
        }

        /// <summary>
        /// Causes screen to fade into black. When yarn spinner uses "false, fade out of black.
        /// </summary>
        /// <param name="fadeToBlack">Should the screen fade in, or out.</param>
        /// <param name="fadeSpeed">How long should it take to fade.</param>
        /// <returns>Unity coroutine.</returns>
        // [YarnCommand("fade_camera")]
        public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = 1)
        {
            Color objectColor = _blackOutSquare.color;
            float fadeAmount;

            if (fadeToBlack)
            {
                while (_blackOutSquare.color.a < 1)
                {
                    fadeAmount = objectColor.a + (1.0f / fadeSpeed * Time.deltaTime);
                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    _blackOutSquare.color = objectColor;
                    yield return null;
                }
            }

            // If we are fading OUT of black
            else
            {
                while (_blackOutSquare.color.a > 0)
                {
                    fadeAmount = objectColor.a - (1.0f / fadeSpeed * Time.deltaTime);
                    objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                    _blackOutSquare.color = objectColor;
                    yield return null;
                }
            }
        }

        private Coroutine StartFadeOut(float fadeSpeed = 1)
        {
            return _runner.StartCoroutine(FadeBlackOutSquare(true, fadeSpeed));
        }

        private Coroutine StartFadeIn(float fadeSpeed = 1)
        {
            return _runner.StartCoroutine(FadeBlackOutSquare(false, fadeSpeed));
        }
    }
}
