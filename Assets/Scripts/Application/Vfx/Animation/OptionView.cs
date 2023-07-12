using Application.Core.Utility;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using System;

namespace Application.Vfx.Animation
{
    using Core;
    using TMPro;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Displays an individual option for an overworld context menu.
    /// </summary>
    public class OptionView : View<OverworldContextMenu.Option>
    {
        public Image icon;
        public TMP_Text nameDisplay;
        public Button selectionButton;
        public CanvasGroup canvasGroup;

        private ITween _tween;

        /// <inheritdoc/>
        public override void BindTo(OverworldContextMenu.Option target)
        {
            if (target.Icon == null)
            {
                icon.gameObject.SetActive(false);
            }
            else
            {
                icon.sprite = target.Icon;
            }

            nameDisplay.text = target.Name;
            selectionButton.OnClickAsObservable().Subscribe(_ => target.Callback.Invoke()).AddTo(this);
        }

        public async UniTask Show(float duration)
        {
            canvasGroup.TweenCanvasGroupAlpha(1, duration).Override(ref _tween);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }

        public async UniTask Hide(float duration)
        {
            canvasGroup.TweenCanvasGroupAlpha(0, duration).Override(ref _tween);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
        }
    }
}
