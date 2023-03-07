namespace Application.Gameplay.Combat.Hooks
{
    /// <summary>
    /// A hook that listens for all enemies to be killed, and then awards the player
    /// a victory.
    /// </summary>
    public class KillAllEnemiesObjective : Hook
    {
        private float _remainingHealth;

        /// <inheritdoc/>
        public override void OnBattleStart()
        {
            foreach (var enemy in Controller.EnemyTeam)
            {
                if (enemy.TryGetComponent(out Health health))
                {
                    health.OnHealthChange += HandleEnemyHealthChange;
                }
            }
        }

        private void HandleEnemyHealthChange(HealthChangeData data)
        {
            _remainingHealth += data.Delta;

            if (_remainingHealth <= 0)
            {
                Controller.EndBattle();
            }
        }
    }
}