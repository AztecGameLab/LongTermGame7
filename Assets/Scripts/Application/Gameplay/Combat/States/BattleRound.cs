﻿namespace Application.Gameplay.Combat.States
{
    using System;
    using Core;
    using ImGuiNET;
    using Round;
    using UI;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The state of a battle where actions are being executed, and the player
    /// is taking turns with the enemies to control the game state.
    /// <remarks>This is the main loop of the battle system, most things happen here.</remarks>
    /// </summary>
    [Serializable]
    public class BattleRound : BattleState, IDebugImGui
    {
        [SerializeField]
        private PickMonster pickMonster;

        [SerializeField]
        private PickActionsForMonster pickActions;

        [SerializeField]
        private PrepareAction prepareAction;

        [SerializeField]
        private PlayActionAnimation playAnimation;

        [SerializeField]
        private EnemyMoveMonsters enemyMoveMonsters;

        [SerializeField]
        private RoundUI roundUI;

        private RoundState[] _states;

        /// <summary>
        /// Gets the state for picking a monster.
        /// </summary>
        public PickMonster PickMonster => pickMonster;

        /// <summary>
        /// Gets the state for picking actions for a monster.
        /// </summary>
        public PickActionsForMonster PickActions => pickActions;

        /// <summary>
        /// Gets the state for preparing an action.
        /// </summary>
        public PrepareAction PrepareAction => prepareAction;

        /// <summary>
        /// Gets the state for playing an action animation.
        /// </summary>
        public PlayActionAnimation PlayActionAnimation => playAnimation;

        /// <summary>
        /// Gets the state for the enemy monsters moving.
        /// </summary>
        public EnemyMoveMonsters EnemyMoveMonsters => enemyMoveMonsters;

        /// <summary>
        /// Gets the number of rounds that have passed in this combat.
        /// </summary>
        public IntReactiveProperty RoundNumber { get; } = new IntReactiveProperty();

        private StateMachine StateMachine { get; set; }

        /// <summary>
        /// Changes the current battle round state.
        /// </summary>
        /// <param name="state">The target state.</param>
        public void TransitionTo(RoundState state)
        {
            StateMachine.SetState(state);
        }

        /// <summary>
        /// Sets up the battle round.
        /// </summary>
        public void Initialize()
        {
            StateMachine = new StateMachine();

            EnemyMoveMonsters.Initialize();
            PickMonster.Initialize();
            PlayActionAnimation.Initialize();
            PrepareAction.Initialize();

            _states = new RoundState[]
            {
                PickMonster, PickActions, PrepareAction, PlayActionAnimation, EnemyMoveMonsters,
            };

            foreach (RoundState roundState in _states)
            {
                roundState.Round = this;
            }

            RegisterDebugImGui(this);
            roundUI.BindTo(this);
        }

        /// <summary>
        /// Advances to the next round.
        /// </summary>
        public void NextRound()
        {
            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundEnd();
            }

            StateMachine.SetState(null);
            RoundNumber.Value++;
            foreach (GameObject playerTeamMember in Controller.PlayerTeam)
            {
                if (playerTeamMember.TryGetComponent(out ActionPointTracker tracker))
                {
                    tracker.Refill();
                }
            }

            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundBegin();
            }

            StateMachine.SetState(pickMonster);
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();

            RoundNumber.Value = 1;
            roundUI.gameObject.SetActive(true);

            foreach (GameObject playerTeamMember in Controller.PlayerTeam)
            {
                if (playerTeamMember.TryGetComponent(out ActionPointTracker tracker))
                {
                    tracker.Refill();
                }
            }

            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundBegin();
            }

            StateMachine.SetState(PickMonster);
        }

        /// <inheritdoc/>
        public override void OnTick()
        {
            StateMachine.Tick();
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            roundUI.gameObject.SetActive(false);

            foreach (RoundState roundState in _states)
            {
                roundState.OnRoundEnd();
            }

            StateMachine.SetState(null);
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Battle Round");

            ImGui.Text($"Current State: {StateMachine.CurrentState.GetType().Name}");

            if (ImGui.Button("Win Battle"))
            {
                Controller.TransitionTo(Controller.Victory);
            }

            if (ImGui.Button("Lose Battle"))
            {
                Controller.TransitionTo(Controller.Loss);
            }

            ImGui.End();
        }
    }
}
