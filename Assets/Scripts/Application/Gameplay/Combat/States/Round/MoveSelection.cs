namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Actions;
    using UI;
    using UniRx;
    using UnityEngine;
    using Button = UnityEngine.UI.Button;

    /// <summary>
    /// The combat round state where the player is choosing what move a monster should perform.
    /// </summary>
    [Serializable]
    public class MoveSelection : RoundState
    {
        [SerializeField]
        private MoveSelectionUI selectionUI;

        [SerializeField]
        private ActionPointTrackerUI trackerUI;

        /// <summary>
        /// Gets the currently selected action.
        /// </summary>
        // public BattleAction SelectedAction =>
        public BattleAction SelectedAction { get; private set; }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            base.OnEnter();

            var selectedActionSet = Round.PickMonster.SelectedMonster.Value.GetComponent<ActionSet>();
            var actionPointTracker = Round.PickMonster.SelectedMonster.Value.GetComponent<ActionPointTracker>();

            trackerUI.gameObject.SetActive(true);
            trackerUI.BindTo(actionPointTracker);

            selectionUI.gameObject.SetActive(true);
            selectionUI.BindTo(selectedActionSet.Actions);
            DisposeOnExit(selectionUI.ObserveActionHovered().Subscribe(action => trackerUI.SetPredictedCost(action.Cost)));
            DisposeOnExit(selectionUI.ObserveActionSubmitted().Subscribe(OnSelectAction));
            DisposeOnExit(selectionUI.ObserveTurnPassed().Subscribe(_ => Round.TransitionTo(Round.EnemyMoveMonsters)));
            SelectedAction = selectedActionSet.Actions[0];

            foreach (MoveUI move in selectionUI.Moves)
            {
                move.GetComponent<Button>().interactable = actionPointTracker.CanAfford(move.Target.Cost);
            }

            if (actionPointTracker.RemainingActionPoints <= 0)
            {
                Round.TransitionTo(Round.EnemyMoveMonsters);
            }
        }

        /// <inheritdoc/>
        public override void OnExit()
        {
            base.OnExit();
            selectionUI.gameObject.SetActive(false);
            trackerUI.gameObject.SetActive(false);
            trackerUI.SetPredictedCost(0);
        }

        private void OnSelectAction(BattleAction monsterAction)
        {
            SelectedAction = monsterAction;
            monsterAction.User = Round.PickMonster.SelectedMonster.Value;
            monsterAction.Controller = Round.Controller;
            Round.TransitionTo(Round.PrepareAction);
        }
    }
}
