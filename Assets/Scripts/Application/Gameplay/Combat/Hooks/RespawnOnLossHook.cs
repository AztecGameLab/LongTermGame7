using System.Collections;

namespace Application.Gameplay.Combat.Hooks
{
    using Core;
    using UniRx;

    /// <summary>
    /// Automatically respawn the player when the lose a battle.
    /// </summary>
    public class RespawnOnLossHook : Hook
    {
        /// <inheritdoc/>
        public override IEnumerator OnBattleStart()
        {
            yield return base.OnBattleStart();

            Controller.Loss.ObserveExited()
                .Subscribe(_ => Services.RespawnTracker.Respawn())
                .AddTo(AutoDispose);
        }
    }
}
