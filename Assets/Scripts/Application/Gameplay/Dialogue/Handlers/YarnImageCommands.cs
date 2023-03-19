namespace Application.Gameplay.Dialogue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ElRaccoone.Tweens;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using Yarn.Unity;

    /// <summary>
    /// Registers commands for yarn to display static images.
    /// </summary>
    [Serializable]
    public class YarnImageCommands : IYarnCommandHandler
    {
        [SerializeField]
        private AssetReference yarnImageViewAsset;

        [SerializeField]
        private float fadeTime = 1;

        [SerializeField]
        private DictionaryGenerator<string, Sprite> imageTable;

        private YarnImageView _imageView;
        private Dictionary<string, Sprite> _imageTable;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _imageView = yarnImageViewAsset.InstantiateAsync(runner.transform)
                .WaitForCompletion()
                .GetComponent<YarnImageView>();

            _imageTable = imageTable.GenerateDictionary();

            runner.AddCommandHandler<string>("image-show", id => runner.StartCoroutine(ShowImage(id)));
            runner.AddCommandHandler("image-hide", () => runner.StartCoroutine(HideImage()));
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("image-show");
            runner.RemoveCommandHandler("image-hide");
        }

        private IEnumerator ShowImage(string imageId)
        {
            Sprite targetImage = _imageTable[imageId];
            _imageView.Image.sprite = targetImage;
            yield return _imageView.CanvasGroup.TweenCanvasGroupAlpha(1, fadeTime).Yield();
        }

        private IEnumerator HideImage()
        {
            yield return _imageView.CanvasGroup.TweenCanvasGroupAlpha(0, fadeTime).Yield();
            _imageView.Image.sprite = null;
        }
    }
}
