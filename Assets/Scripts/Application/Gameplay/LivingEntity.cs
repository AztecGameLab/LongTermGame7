namespace Application.Gameplay
{
    using System;
    using UniRx;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// Represents something in the game that can live, die, and take damage.
    /// </summary>
    public class LivingEntity : MonoBehaviour
    {
        private readonly Subject<float> _onDamage = new Subject<float>();
        private readonly Subject<float> _onHeal = new Subject<float>();

        [SerializeField]
        private FloatReactiveProperty health;

        [SerializeField]
        private FloatReactiveProperty maxHealth;

        [SerializeField]
        private BoolReactiveProperty isInvincible;

        [SerializeField]
        private UnityEvent onDeath;

        /// <summary>
        /// Gets an observable for each time this entity is damaged.
        /// Passes the remaining health of the entity.
        /// </summary>
        public IObservable<float> OnDamage => _onDamage;

        /// <summary>
        /// Gets an observable for each time this entity is healed.
        /// </summary>
        public IObservable<float> OnHeal => _onHeal;

        /// <summary>
        /// Gets an observable for each time this entity is healed.
        /// </summary>
        public IObservable<bool> InvincibilityChanged => isInvincible;

        /// <summary>
        /// Gets an observable for each time this entity's health drops below zero.
        /// </summary>
        public IObservable<Unit> OnDeath => OnHealthChange
            .Where(currentHealth => currentHealth <= 0)
            .Select(_ => Unit.Default);

        /// <summary>
        /// Gets an observable for each time this entity's health changes.
        /// </summary>
        public IObservable<float> OnHealthChange => health;

        /// <summary>
        /// Gets an observable for each time this entity's maximum health changes.
        /// </summary>
        public IObservable<float> OnMaxHealthChange => maxHealth;

        /// <summary>
        /// Gets how much health this entity has remaining.
        /// </summary>
        public float Health => health.Value;

        /// <summary>
        /// Gets or sets the maximum amount of health this entity might have.
        /// </summary>
        public float MaxHealth
        {
            get => maxHealth.Value;
            set => maxHealth.Value = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this entity can be damaged.
        /// </summary>
        public bool IsInvincible
        {
            get => isInvincible.Value;
            set => isInvincible.Value = value;
        }

        /// <summary>
        /// Sets up the initial values for this entity.
        /// </summary>
        /// <param name="targetHealth">How much health to have.</param>
        /// <param name="targetHealthMax">How much max health to have.</param>
        public void Initialize(float targetHealth, float targetHealthMax)
        {
            health.Value = targetHealth;
            maxHealth.Value = targetHealthMax;
        }

        /// <summary>
        /// Remove this entity's health.
        /// </summary>
        /// <param name="amount">How much health to remove.</param>
        public void Damage(float amount)
        {
            if (!IsInvincible)
            {
                float newHealth = Mathf.Max(0, health.Value - amount);
                health.Value = newHealth;
                _onDamage.OnNext(newHealth);
            }
        }

        /// <summary>
        /// Restore this entity's health.
        /// </summary>
        /// <param name="amount">How much health should be restored.</param>
        public void Heal(float amount)
        {
            float newHealth = Mathf.Min(maxHealth.Value, health.Value + amount);
            health.Value = newHealth;
            _onHeal.OnNext(newHealth);
        }

        /// <summary>
        /// Increase max health by 1.
        /// </summary>
        /// <param name="type">item found, to be determined how much increase based on item.</param>
        public void StrengthenRM(string type)
        {
            // float increaseHealth = 0;
            //
            // if (type == "red herb item")
            // {
            //    //smallest number. Will Change value later.
            //    increaseHealth = 10;
            // }
            // else if (type == "red mushroom item")
            // {
            //     //mid number. Will Change value later
            //     increaseHealth = 10;
            // }
            // else if (type == "red elixir item")
            // {
            //     //big number. Will change value later
            //     increaseHealth = 10;
            // }
            //
            // //increase maxHealth by amount
            maxHealth.Value += 1;
        }

        /// <summary>
        /// Deals enough damage to kill this entity.
        /// </summary>
        public void Kill()
        {
            health.Value = 0;
        }

        private void Awake()
        {
            OnDeath.Subscribe(_ => onDeath.Invoke()).AddTo(this);
        }
    }
}
