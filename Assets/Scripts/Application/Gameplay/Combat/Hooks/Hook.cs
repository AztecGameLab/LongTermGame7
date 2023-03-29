namespace Application.Gameplay.Combat.Hooks
{
    using System;
    using UniRx;

    /// <summary>
    /// A logical event that is monitored during a battle.
    /// For example, a hook might be a victory / loss condition.
    /// </summary>
    [Serializable]
    public abstract class Hook
    {
        /// <summary>
        /// Gets or sets a parent controller of this hook.
        /// </summary>
        public BattleController Controller { get; set; }

        /// <summary>
        /// Gets a disposable that will be cleaned up when the battle ends.
        /// </summary>
        protected CompositeDisposable AutoDispose { get; private set; }

        /// <summary>
        /// Called once when the battle begins.
        /// </summary>
        public virtual void OnBattleStart()
        {
            AutoDispose = new CompositeDisposable();
        }

        /// <summary>
        /// Called frequently while the battle is running.
        /// </summary>
        public virtual void OnBattleUpdate()
        {
        }

        /// <summary>
        /// Called once when the battle ends.
        /// </summary>
        public virtual void OnBattleEnd()
        {
            AutoDispose?.Dispose();
        }
    }
}
