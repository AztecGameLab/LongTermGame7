using System.Collections.Generic;

namespace Application.Vfx
{
    using System;
    using System.Threading.Tasks;
    using Core;
    using Core.Utility;
    using Cysharp.Threading.Tasks;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using JetBrains.Annotations;
    using TriInspector;
    using UnityEngine;

    public class MindControlEffect : MonoBehaviour
    {
        [SerializeField]
        private Gradient outlineColor;

        [SerializeField]
        private Gradient tint;

        [SerializeField]
        private bool applyOnStart = true;

        [SerializeField]
        private float duration = 1;

        [SerializeField]
        private float pulseSpeed;

        [SerializeField] private float shakeMagnitude = 0.5f;
        [SerializeField] private float shakeFrequency = 5;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private OutlineInstance _outline;
        private bool _hasOutline;
        private ITween _tween;
        private Color _originalColor = Color.white;
        private float _randomOffset;
        private float _elapsedTime;

        public static MindControlEffect AddTo(GameObject target, Gradient outlineColor, Gradient tint, SpriteRenderer renderer = null, float pulseSpeed = 1, bool applyOnStart = true, float duration = 1)
        {
            var instance = target.AddComponent<MindControlEffect>();
            instance.outlineColor = outlineColor;
            instance.tint = tint;
            instance.spriteRenderer = renderer == null ? target.GetComponentInChildren<SpriteRenderer>() : renderer;
            instance.pulseSpeed = pulseSpeed;
            instance.applyOnStart = applyOnStart;
            instance.duration = duration;
            return instance;
        }

        public async UniTask ApplyEffect()
        {
            float t = (Mathf.Sin(_randomOffset + pulseSpeed) + 1) / 2;
            _outline = Services.Outliner.AddOutline(spriteRenderer.gameObject, outlineColor.Evaluate(t), duration);
            spriteRenderer.TweenSpriteRendererColor(tint.Evaluate(t), duration).Override(ref _tween);
            var shakeInstance = Services.ShakeApplier.ApplyShake(spriteRenderer.gameObject, shakeFrequency, shakeMagnitude);
            await Task.Delay(TimeSpan.FromSeconds(duration));
            shakeInstance.Dispose();
            _hasOutline = true;
            _elapsedTime = 0;
        }

        public async UniTask RemoveEffect()
        {
            _hasOutline = false;
            Services.Outliner.RemoveOutline(spriteRenderer.gameObject, duration);
            spriteRenderer.TweenSpriteRendererColor(_originalColor, duration).Override(ref _tween);
            await Task.Delay(TimeSpan.FromSeconds(duration));
        }

        private void Start()
        {
            _randomOffset = UnityEngine.Random.Range(0.0f, 1.0f);
            _originalColor = spriteRenderer.color;

            if (applyOnStart)
            {
                ApplyEffect().Forget();
            }
        }

        private void Update()
        {
            if (_hasOutline)
            {
                float t = (Mathf.Sin(_randomOffset + (_elapsedTime * pulseSpeed)) + 1) / 2;
                _outline.Color = outlineColor.Evaluate(t);
                spriteRenderer.color = tint.Evaluate(t);
                _elapsedTime += Time.deltaTime;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"has outline: {_hasOutline}");
            GUILayout.Label($"color: {_originalColor}");
            GUILayout.Label($"time: {_elapsedTime}");
        }

        [Button]
        [UsedImplicitly]
        [ShowInPlayMode]
        private void InspectorApplyEffect()
        {
            ApplyEffect().Forget();
        }

        [Button]
        [UsedImplicitly]
        [ShowInPlayMode]
        private void InspectorRemoveEffect()
        {
            RemoveEffect().Forget();
        }
    }
}
