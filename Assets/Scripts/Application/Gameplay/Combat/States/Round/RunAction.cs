namespace Application.StateMachine
{
    public class PlayActionAnimation : RoundState
    {
        public override void OnEnter()
        {
            BattleRound.SelectedAction.Run();
            
            // choose another action when this animation finished, assume the method below is called correctly
        }

        private void OnActionEnd()
        {
            BattleRound.StateMachine.SetState(BattleRound.PickActions);
        }
    }
}