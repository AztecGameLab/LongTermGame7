using System.Collections;
using UnityEngine;

namespace Application.StateMachine
{
    public class BattleIntro : BattleState
    {
        public override void OnEnter()
        {
            // todo: add a way for custom battle intro / cutscene stuff?
            // we probably also want to do some common camera movements or animations.
            Debug.Log("Battle intro is a stub right now, but its running!");
            Controller.BattleStateMachine.SetState(Controller.BattleRound);
        }
    }
}

