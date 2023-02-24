using Application.Gameplay.Combat;
using ImGuiNET;

namespace Application.StateMachine
{
    public class PickActionsForMonster : RoundState
    {
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
        }

        private ActionSet _selectedActionSet;
        private ActionPointTracker _actionPointTracker;

        private int _currentActionIndex;

        private IAction CurrentAction =>
            _selectedActionSet.actions[_currentActionIndex % _selectedActionSet.actions.Count];

        protected override void DrawGui()
        {
            ImGui.Begin("Decide Monster Actions");
            
            if (ImGui.Button("Next Action"))
                _currentActionIndex++;
            
            if (ImGui.Button($"Choose Action {CurrentAction.Name}"))
                OnSelectAction(CurrentAction);
                
            ImGui.End();
        }

        private void OnSelectAction(IAction monsterAction)
        {
            Round.SelectedAction = monsterAction;
            Round.StateMachine.SetState(Round.PlayAnimation);
        }
    }
}