using System.Collections;

namespace Application.Gameplay.Combat.Hooks
{
    using System;
    using UniRx;
    using Object = UnityEngine.Object;

    /// <summary>
    /// A hook that listens for the player to die, and loses the battle if that happens.
    /// </summary>
    public class KeepPlayerAliveObjective : Hook
    {
        private IDisposable _disposable;

        /// <inheritdoc/>
        public override IEnumerator OnBattleStart()
        {
            yield return base.OnBattleStart();

            var player = Object.FindObjectOfType<PlayerMovement>().GetComponent<LivingEntity>();
            _disposable = player.OnDeath.Subscribe(_ => Controller.TransitionTo(Controller.Loss));
        }

        /// <inheritdoc/>
        public override IEnumerator OnBattleEnd()
        {
            yield return base.OnBattleEnd();
            _disposable?.Dispose();
        }
    }
}
