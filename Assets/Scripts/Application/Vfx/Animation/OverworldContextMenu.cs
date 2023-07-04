using UniRx;

namespace Application.Vfx.Animation
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using FSM;
    using TMPro;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    /// <summary>
    /// Display a context menu in-game in the overworld.
    /// </summary>
    public class OverworldContextMenu : MonoBehaviour
    {
        private readonly List<OptionView> _options = new List<OptionView>();

        [SerializeField]
        private TMP_Text titleDisplay;

        [SerializeField]
        private float duration = 0.25f;

        [SerializeField]
        private float delay = 0.1f;

        [SerializeField]
        private Transform optionsParent;

        [SerializeField]
        private OptionView optionPrefab;

        private ITween _tween;
        private float _originalScale;
        private StateMachine _stateMachine;
        private bool _visibleAnimationDone;
        private bool _hiddenAnimationDone;

        /// <summary>
        /// Instantiates a new overworld context menu.
        /// </summary>
        /// <param name="position">The position it should be instantiated at.</param>
        /// <param name="title">The title of the menu.</param>
        /// <param name="options">The options the menu should contain.</param>
        /// <returns>The new instance of the menu.</returns>
        public static OverworldContextMenu Create(Vector3 position, string title, List<Option> options)
        {
            GameObject asset = Addressables.InstantiateAsync("overworld_context_menu").WaitForCompletion();
            OverworldContextMenu instance = asset.GetComponent<OverworldContextMenu>();
            instance.titleDisplay.text = title;
            instance.SetOptions(options);
            instance.transform.position = position;
            return instance;
        }

        /// <summary>
        /// Updates the displayed options for this menu.
        /// </summary>
        /// <param name="options">The new options that should be displayed.</param>
        public void SetOptions(List<Option> options)
        {
            foreach (OptionView option in _options)
            {
                Destroy(option);
            }

            foreach (var option in options)
            {
                OptionView instance = Instantiate(optionPrefab, optionsParent);
                instance.BindTo(option);
                instance.selectionButton.OnClickAsObservable().Subscribe(_ => Hide()).AddTo(this);
                _options.Add(instance);
            }
        }

        /// <summary>
        /// Display the context menu.
        /// </summary>
        public void Show()
        {
            if (_stateMachine.ActiveStateName != "visible")
            {
                _stateMachine.RequestStateChange("visible");
            }
        }

        /// <summary>
        /// Hide the context menu.
        /// </summary>
        public void Hide()
        {
            if (_stateMachine.ActiveStateName != "hidden")
            {
                _stateMachine.RequestStateChange("hidden");
            }
        }

        private void Start()
        {
            _originalScale = transform.localScale.y;

            _stateMachine = new StateMachine();

            _stateMachine.AddState(
                "visible",
                onEnter: state => ShowMenuTask().GetAwaiter().OnCompleted(() => _stateMachine.StateCanExit()),
                canExit: state => _visibleAnimationDone,
                needsExitTime: true);

            _stateMachine.AddState(
                "hidden",
                onEnter: state => HideMenuTask().GetAwaiter().OnCompleted(() => _stateMachine.StateCanExit()),
                canExit: state => _hiddenAnimationDone,
                needsExitTime: true);

            _stateMachine.SetStartState("hidden");
            _stateMachine.Init();
        }

        private void Update()
        {
            _stateMachine.OnLogic();
        }

        private async UniTask ShowMenuTask()
        {
            _visibleAnimationDone = false;
            this.TweenLocalScaleY(_originalScale, duration).SetFrom(0).SetEase(EaseType.CubicInOut);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            foreach (OptionView option in _options)
            {
                option.Show(duration).Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }

            _visibleAnimationDone = true;
        }

        private async UniTask HideMenuTask()
        {
            _hiddenAnimationDone = false;

            foreach (OptionView option in _options)
            {
                option.Hide(duration).Forget();
                await UniTask.Delay(TimeSpan.FromSeconds(delay));
            }

            this.TweenLocalScaleY(0, duration).SetFrom(_originalScale).SetEase(EaseType.CubicInOut);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            _hiddenAnimationDone = true;
        }

        /// <summary>
        /// An individual option of a context menu.
        /// </summary>
        public struct Option
        {
            /// <summary>
            /// The name of this option, prominently displayed.
            /// </summary>
            public string Name;

            /// <summary>
            /// The action to run when this option is selected.
            /// </summary>
            public Action Callback;

            /// <summary>
            /// The icon that can be optionally displayed alongside the name.
            /// </summary>
            public Sprite Icon;
        }
    }
}
