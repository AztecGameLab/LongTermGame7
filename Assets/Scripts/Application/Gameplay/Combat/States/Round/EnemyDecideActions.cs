namespace Application.StateMachine
{
    public class EnemyMoveMonsters : RoundState
    {
        public override void OnEnter()
        {
            var controller = BattleRound.Controller;
            controller.StartCoroutine(controller.Decider.ExecuteTurn(controller));
            
            // subscribe to when the decider finishes its stuff, and call below method
        }

        private void OnDeciderFinish()
        {
            BattleRound.StateMachine.SetState(BattleRound.PickMonster);
        }
    }
}