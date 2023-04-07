using ElRaccoone.Tweens.Core;

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

        private YarnImageView _imageViewA;
        private YarnImageView _imageViewB;
        private Dictionary<string, Sprite> _imageTable;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _imageViewA = yarnImageViewAsset.InstantiateAsync(runner.transform)
                .WaitForCompletion()
                .GetComponent<YarnImageView>();

            _imageViewB = yarnImageViewAsset.InstantiateAsync(runner.transform)
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

        private YarnImageView OldView => _viewTracker ? _imageViewA : _imageViewB;

        private YarnImageView CurrentView => _viewTracker ? _imageViewB : _imageViewA;

        private bool _viewTracker;

        private IEnumerator ShowImage(string imageId)
        {
            if (OldView.Image != null)
            {
                OldView.CanvasGroup.TweenCanvasGroupAlpha(0, fadeTime).SetEase(EaseType.CubicIn);
                OldView.CanvasGroup.interactable = false;
                OldView.CanvasGroup.blocksRaycasts = false;
            }

            Sprite targetImage = _imageTable[imageId];
            CurrentView.Image.sprite = targetImage;
            CurrentView.CanvasGroup.interactable = true;
            CurrentView.CanvasGroup.blocksRaycasts = true;
            yield return CurrentView.CanvasGroup.TweenCanvasGroupAlpha(1, fadeTime).SetEase(EaseType.CubicOut).Yield();
            _viewTracker = !_viewTracker;
        }

        private IEnumerator HideImage()
        {
            yield return OldView.CanvasGroup.TweenCanvasGroupAlpha(0, fadeTime).SetEase(EaseType.CubicIn).Yield();
            OldView.Image.sprite = null;
            OldView.CanvasGroup.interactable = false;
            OldView.CanvasGroup.blocksRaycasts = false;
        }
    }
}