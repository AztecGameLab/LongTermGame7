using Application.Core;
using Cinemachine;
using ImGuiNET;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Application.StateMachine
{
    public abstract class RoundState : IState
    {
        public BattleRound BattleRound { get; set; }

        private IDisposable _disposable;

        public virtual void OnEnter()
        {
            _disposable = ImGuiUtil.Register(DrawGui);
        }

        public virtual void OnExit()
        {
            _disposable?.Dispose();
        }
        
        public virtual void OnTick() {}
        protected virtual void DrawGui() {}
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

        protected override void DrawGui()
        {
            ImGui.Begin("Battle Round");
            
            ImGui.Text($"Current State: {StateMachine.CurrentState.GetType().Name}");
            
            if (ImGui.Button("Win Battle"))
                Controller.BattleStateMachine.SetState(Controller.BattleVictory);
            
            if (ImGui.Button("Lose Battle"))
                Controller.BattleStateMachine.SetState(Controller.BattleLoss);
            
            ImGui.End();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            StateMachine.SetState(PickMonster);
        }
        
        public override void OnTick()
        {
            StateMachine.Tick();
        }

        public override void OnExit()
        {
            base.OnExit();
            StateMachine.SetState(null);
        }
    }
}