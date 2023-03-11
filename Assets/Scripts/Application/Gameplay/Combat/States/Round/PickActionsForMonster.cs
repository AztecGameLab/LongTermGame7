﻿using Application.Gameplay.Combat.UI;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Actions;
    using Core;
    using ImGuiNET;

    /// <summary>
    /// The combat round state where the player is choosing what move a monster should perform.
    /// </summary>
    [Serializable]
    public class PickActionsForMonster : RoundState, IDebugImGui
    {
        [SerializeField]
        private MoveSelectionUI selectionUI;
        
        private ActionSet _selectedActionSet;
        private ActionPointTracker _actionPointTracker;
        private int _currentActionIndex;
        private IDisposable _disposable;

        /// <summary>
        /// Gets the currently selected action.
        /// </summary>
        /// <value>
        /// The currently selected action.
        /// </value>
        public BattleAction SelectedAction =>
            _selectedActionSet.Actions[_currentActionIndex % _selectedActionSet.Actions.Count];

        /// <summary>
        /// Initializes a new instance of the <see cref="PickActionsForMonster"/> class.
        /// </summary>
        public void Initialize()
        {
            RegisterImGuiDebug(this);
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();

            _selectedActionSet = Round.PickMonster.SelectedMonster.GetComponent<ActionSet>();
            _actionPointTracker = Round.PickMonster.SelectedMonster.GetComponent<ActionPointTracker>();
            
            selectionUI.gameObject.SetActive(true);
            selectionUI.BindTo(_selectedActionSet.Actions);
            _disposable = selectionUI.ObserveActionSubmitted().Subscribe(OnSelectAction);

            if (_actionPointTracker.RemainingActionPoints <= 0)
            {
                Round.TransitionTo(Round.EnemyMoveMonsters);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            selectionUI.gameObject.SetActive(false);
            _disposable?.Dispose();
        }

        /// <inheritdoc/>
        public void RenderImGui()
        {
            ImGui.Begin("Decide Monster Actions");

            if (_actionPointTracker != null)
            {
                ImGui.Text($"Action Points: {_actionPointTracker.RemainingActionPoints}/{_actionPointTracker.MaxActionPoints}");
            }

            if (ImGui.Button("Next Action"))
            {
                _currentActionIndex++;
            }

            if (ImGui.Button($"Choose Action {SelectedAction.Name}"))
            {
                OnSelectAction(SelectedAction);
            }

            ImGui.End();
        }

        private void OnSelectAction(BattleAction monsterAction)
        {
            monsterAction.User = Round.PickMonster.SelectedMonster;
            monsterAction.Controller = Round.Controller;
            Round.TransitionTo(Round.PrepareAction);
        }
    }
}
