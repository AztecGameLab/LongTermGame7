using UnityEngine;

namespace Application.StateMachine
{
    public class PickMonster : RoundState
    {
        public override void OnEnter()
        {
            var partyView = Object.FindObjectOfType<PlayerPartyView>();
            
            // now we can enumerate partyView.PlayerPartyInstances, and go from there to
            // determine the monster being chosen.
            
            // we would probably want to load a UI and quit in response to a button / input, so assume OnSelectMonster is bound to that.
        }

        private void OnSelectMonster(GameObject monster)
        {
            BattleRound.SelectedMonster = monster;
            BattleRound.StateMachine.SetState(BattleRound.PickActions);
        }
    }
}