using Application.Core;
using Application.Gameplay.Combat;
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
        public BattleRound Round { get; set; }

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
        protected virtual void DrawGizmos() {}
        
        public virtual void OnRoundBegin() {}
        public virtual void OnRoundEnd() {}
    }
    
    public class BattleRound : BattleState
    {
        public StateMachine StateMachine { get; private set; }

        public PickMonster PickMonster {get; private set;}
        public PickActionsForMonster PickActions {get; private set;}
        public PrepareAction PrepareAction { get; private set; }
        public PlayActionAnimation PlayAnimation {get; private set;}
        public EnemyMoveMonsters EnemyMoveMonsters { get; private set; }

        public GameObject SelectedMonster { get; set; }
        public BattleAction SelectedAction { get; set; }

        private RoundState[] _states;

        public BattleRound()
        {
            StateMachine = new StateMachine();

            _states = new RoundState[]
            {
                PickMonster = new PickMonster(),
                PickActions = new PickActionsForMonster(),
                PrepareAction = new PrepareAction(),
                PlayAnimation = new PlayActionAnimation(),
                EnemyMoveMonsters = new EnemyMoveMonsters(),
            };

            foreach (RoundState roundState in _states)
            {
                roundState.Round = this;
            }
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

            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundBegin();
            }
            
            StateMachine.SetState(PickMonster);
        }
        
        public override void OnTick()
        {
            StateMachine.Tick();
        }

        public override void OnExit()
        {
            base.OnExit();

            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundEnd();
            }
            
            StateMachine.SetState(null);
        }
    }
}