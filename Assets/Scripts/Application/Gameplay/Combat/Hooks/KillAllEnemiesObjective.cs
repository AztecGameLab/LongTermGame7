namespace Application.Gameplay.Combat.Hooks
{
    using Core;
    using UniRx;

    /// <summary>
    /// A hook that listens for all enemies to be killed, and then awards the player
    /// a victory.
    /// </summary>
    public class KillAllEnemiesObjective : Hook
    {
        private float _remainingHealth;
        private CompositeDisposable _disposable;

        /// <inheritdoc/>
        public override void OnBattleStart()
        {
            base.OnBattleStart();
            _disposable = new CompositeDisposable();

            foreach (var enemy in Controller.EnemyTeam)
            {
                if (enemy.TryGetComponent(out LivingEntity health))
                {
                    var disposable = health.OnHealthChange
                        .WithPrevious()
                        .Subscribe(data => HandleEnemyHealthChange(data.Delta()));

                    _disposable.Add(disposable);
                }
            }
        }

        /// <inheritdoc/>
        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            _disposable.Dispose();
        }

        private void HandleEnemyHealthChange(float delta)
        {
            _remainingHealth += delta;

            if (_remainingHealth <= 0)
            {
                Controller.EndBattle();
            }
        }
    }
}
