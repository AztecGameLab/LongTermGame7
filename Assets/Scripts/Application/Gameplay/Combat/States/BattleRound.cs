using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Application.StateMachine
{
    public abstract class RoundState : IState
    {
        public BattleRound BattleRound { get; set; }
        
        public virtual void OnEnter() {}
        public virtual void OnTick() {}
        public virtual void OnExit() {}
    }
    
    public class BattleRound : BattleState
    {
        public StateMachine StateMachine { get; private set; }

        public PickMonster PickMonster {get; private set;}
        public PickActionsForMonster PickActions {get; private set;}
        public PlayActionAnimation PlayAnimation {get; private set;}
        public EnemyMoveMonsters EnemyMoveMonsters { get; private set; }

        public GameObject SelectedMonster { get; set; }
        public MonsterAction SelectedAction { get; set; }

        public BattleRound()
        {
            StateMachine = new StateMachine();
            
            PickMonster = new PickMonster { BattleRound = this };
            PickActions = new PickActionsForMonster { BattleRound = this };
            PlayAnimation = new PlayActionAnimation { BattleRound = this };
            EnemyMoveMonsters = new EnemyMoveMonsters { BattleRound = this };
        }
        
        public override void OnEnter()
        {
            StateMachine.SetState(PickMonster);
        }
        
        public override void OnTick()
        {
            StateMachine.Tick();
        }

        public override void OnExit()
        {
            StateMachine.SetState(null);
        }
    }
}
