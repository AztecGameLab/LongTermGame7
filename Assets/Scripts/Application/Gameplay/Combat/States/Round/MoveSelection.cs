namespace Application.Gameplay.Combat.States.Round
{
    using System;
    using Actions;
    using UI;
    using UniRx;
    using UnityEngine;

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

        private IDisposable _disposable;

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
            _disposable = selectionUI.ObserveActionSubmitted().Subscribe(OnSelectAction);
            SelectedAction = selectedActionSet.Actions[0];

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
            _disposable?.Dispose();
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
