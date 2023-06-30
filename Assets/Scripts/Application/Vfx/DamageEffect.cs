namespace Application.Vfx
{
    using System.Collections.Generic;
    using ElRaccoone.Tweens;
    using ElRaccoone.Tweens.Core;
    using FMODUnity;
    using Gameplay;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Causes an entity to flash when they take damage.
    /// </summary>
    [RequireComponent(typeof(LivingEntity))]
    public class DamageEffect : MonoBehaviour
    {
        private readonly List<ITween> _tweens = new List<ITween>();

        [SerializeField]
        private Renderer[] targetRenderers;

        [SerializeField]
        private Transform targetTransform;

        [SerializeField]
        private float duration = 0.5f;

        [SerializeField]
        private float scaleAmount = 1.25f;

        [SerializeField]
        private Color flashColor;

        [SerializeField]
        private EventReference hitSound;

        private void Awake()
        {
            var entity = GetComponent<LivingEntity>();
            entity.OnDamage.Subscribe(_ => FlashRed()).AddTo(this);
        }

        private void FlashRed()
        {
            foreach (ITween tween in _tweens)
            {
                tween.Cancel();
            }

            _tweens.Clear();
            RuntimeManager.PlayOneShot(hitSound);

            foreach (Renderer target in targetRenderers)
                _tweens.Add(target.TweenSpriteRendererColor(Color.white, duration).SetFrom(flashColor));

            _tweens.Add(targetTransform.TweenLocalScale(Vector3.one, duration).SetFrom(Vector3.one * scaleAmount));
        }
    }
}
