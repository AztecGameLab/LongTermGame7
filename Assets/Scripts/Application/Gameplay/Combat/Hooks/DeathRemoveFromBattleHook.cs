namespace Application.Gameplay.Combat.Hooks
{
    using System.Collections.Generic;
    using UniRx;
    using UniRx.Diagnostics;
    using UnityEngine;

    /// <summary>
    /// A hook that listens for monsters to die, and then removes them from battle.
    /// </summary>
    public class DeathRemoveFromBattleHook : Hook
    {
        private CompositeDisposable _disposable;

        /// <inheritdoc/>
        public override void OnBattleStart()
        {
            base.OnBattleStart();

            _disposable = new CompositeDisposable();
            SetupDeathHandlers(Controller.PlayerTeam);
            SetupDeathHandlers(Controller.EnemyTeam);
            _disposable.AddTo(Controller);
        }

        /// <inheritdoc/>
        public override void OnBattleEnd()
        {
            base.OnBattleEnd();
            _disposable.Dispose();
        }

        private void SetupDeathHandlers(ICollection<GameObject> objects)
        {
            foreach (GameObject gameObject in objects)
            {
                if (gameObject.TryGetComponent(out LivingEntity entity))
                {
                    _disposable.Add(entity.OnDeath.Subscribe(_ => objects.Remove(gameObject)));
                }
            }
        }
    }
}
