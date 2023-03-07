namespace Application.Gameplay.Combat.States.Round
{
    using Core;
    using ImGuiNET;

    /// <summary>
    /// The combat round state where the player is choosing what move a monster should perform.
    /// </summary>
    public class PickActionsForMonster : RoundState
    {
        private ActionSet _selectedActionSet;
        private ActionPointTracker _actionPointTracker;
        private int _currentActionIndex;

        private BattleAction CurrentAction =>
            _selectedActionSet.Actions[_currentActionIndex % _selectedActionSet.Actions.Count];

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();

            _selectedActionSet = Round.SelectedMonster.GetComponent<ActionSet>();
            _actionPointTracker = Round.SelectedMonster.GetComponent<ActionPointTracker>();

            if (_actionPointTracker.remainingActionPoints <= 0)
            {
                _actionPointTracker.remainingActionPoints = _actionPointTracker.maxActionPoints;
                Round.StateMachine.SetState(Round.EnemyMoveMonsters);
            }

            Services.EventBus.Invoke(new RoundStateEnterEvent<PickActionsForMonster> { State = this }, "Pick Actions For Monster State");
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            Services.EventBus.Invoke(new RoundStateExitEvent<PickActionsForMonster> { State = this }, "Pick Actions For Monster State");
        }

        /// <inheritdoc/>
        protected override void DrawGui()
        {
            ImGui.Begin("Decide Monster Actions");

            if (_actionPointTracker != null)
            {
                ImGui.Text($"Action Points: {_actionPointTracker.remainingActionPoints}/{_actionPointTracker.maxActionPoints}");
            }

            if (ImGui.Button("Next Action"))
            {
                _currentActionIndex++;
            }

            if (ImGui.Button($"Choose Action {CurrentAction.Name}"))
            {
                OnSelectAction(CurrentAction);
            }

            ImGui.End();
        }

        private void OnSelectAction(BattleAction monsterAction)
        {
            monsterAction.User = Round.SelectedMonster;
            Round.SelectedAction = monsterAction;
            Round.StateMachine.SetState(Round.PrepareAction);
        }
    }
}
