namespace Application.Gameplay.Combat.UI
{
    using System;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A health bar to mirror an entity's health.
    /// </summary>
    public class HealthBar : View<LivingEntity>
    {
        [SerializeField]
        private LivingEntity autoBind;

        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Image healthImageFill;

        [SerializeField]
        private float animationSpeed;

        [SerializeField]
        private Gradient healthGradient;

        private float _currentHealthPercent;
        private IDisposable _targetDisposable;

        /// <inheritdoc/>
        public override void BindTo(LivingEntity target)
        {
            if (target != null)
            {
                _targetDisposable?.Dispose();

                _targetDisposable = target.OnHealthChange
                    .Merge(target.OnMaxHealthChange)
                    .Subscribe(_ => RecalculateHealthPercent(target));

                _targetDisposable.AddTo(this);
                _targetDisposable.AddTo(target);
            }
        }

        private void Start()
        {
            BindTo(autoBind);
        }

        private void Update()
        {
            float t = animationSpeed * Time.deltaTime;
            healthSlider.value = Mathf.Lerp(healthSlider.value, _currentHealthPercent, t);
            healthImageFill.color = healthGradient.Evaluate(healthSlider.value);
        }

        private void RecalculateHealthPercent(LivingEntity target)
        {
            if (target.MaxHealth > 0)
            {
                _currentHealthPercent = Mathf.Clamp01(target.Health / target.MaxHealth);
            }
        }
    }
}
