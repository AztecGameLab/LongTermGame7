namespace Application.Gameplay.Combat.Hooks
{
    using System;

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
        /// Called once when the battle begins.
        /// </summary>
        public virtual void OnBattleStart()
        {
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
        }
    }
}
