namespace Application.Gameplay.Combat.Hooks
{
    /// <summary>
    /// A hook that listens for all enemies to be killed, and then awards the player
    /// a victory.
    /// </summary>
    public class KillAllEnemiesObjective : Hook
    {
        /// <inheritdoc/>
        public override void OnBattleUpdate()
        {
            base.OnBattleUpdate();

            if (Controller.EnemyTeam.Count <= 0 && Controller.CurrentState != Controller.Victory)
            {
                Controller.TransitionTo(Controller.Victory);
            }
        }
    }
}
