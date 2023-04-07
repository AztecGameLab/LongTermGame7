namespace Application.Gameplay.Combat.Hooks
{
    public class KeepAllMonstersAliveHook : Hook
    {
        public override void OnBattleUpdate()
        {
            base.OnBattleUpdate();

            if (Controller.PlayerTeam.Count <= 0 && Controller.CurrentState != Controller.Loss)
            {
                Controller.TransitionTo(Controller.Loss);
            }
        }
    }
}
