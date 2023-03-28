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
        public override void OnBattleStart()
        {
            base.OnBattleStart();

            Controller.Loss.ObserveOnExit()
                .Subscribe(_ => Services.RespawnTracker.Respawn())
                .AddTo(AutoDispose);
        }
    }
}
