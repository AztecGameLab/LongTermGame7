namespace Application.Gameplay
{
    using System;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Represents something in the game that can live, die, and take damage.
    /// </summary>
    public class LivingEntity : MonoBehaviour
    {
        private readonly Subject<float> _onDamage = new Subject<float>();
        private readonly Subject<float> _onHeal = new Subject<float>();

        [SerializeField]
        private FloatReactiveProperty health;

        /// <summary>
        /// Gets an observable for each time this entity is damaged.
        /// </summary>
        public IObservable<float> OnDamage => _onDamage;

        /// <summary>
        /// Gets an observable for each time this entity is healed.
        /// </summary>
        public IObservable<float> OnHeal => _onHeal;

        /// <summary>
        /// Gets an observable for each time this entity's health changes.
        /// </summary>
        public IObservable<float> OnHealthChange => health;

        /// <summary>
        /// Remove this entity's health.
        /// </summary>
        /// <param name="amount">How much health to remove.</param>
        public void Damage(float amount)
        {
            health.Value -= amount;
            _onDamage.OnNext(amount);
        }

        /// <summary>
        /// Restore this entity's health.
        /// </summary>
        /// <param name="amount">How much health should be restored.</param>
        public void Heal(float amount)
        {
            health.Value += amount;
            _onHeal.OnNext(amount);
        }
    }
}
