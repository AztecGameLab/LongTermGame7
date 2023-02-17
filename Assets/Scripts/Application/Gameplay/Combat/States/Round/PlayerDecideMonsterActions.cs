namespace Application.StateMachine
{
    public class PickActionsForMonster : RoundState
    {
        public override void OnEnter()
        {
            var targetMonster = BattleRound.SelectedMonster;
            
            // now that we have the target monster, we can find out what it wants to do with some GetComponent calls
            
            // this is also the spot where we check and handle the case where there are no action points left to spend
            if (false) // defaulted to false for now 
            {
                BattleRound.StateMachine.SetState(BattleRound.EnemyMoveMonsters);
            }
            
            // we also probably want to load in a UI for this as well, same thing with the OnSelectAction call.
        }

        private void OnSelectAction(MonsterAction monsterAction)
        {
            BattleRound.SelectedAction = monsterAction;
            BattleRound.StateMachine.SetState(BattleRound.PlayAnimation);
        }
    }
}