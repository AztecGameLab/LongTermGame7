namespace Application.Gameplay.Combat.UI
{
    using System;
    using System.Collections.Generic;
    using Actions;
    using Core;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Displays a list of moves that can be selected.
    /// </summary>
    public class MoveSelectionUI : UIView<IReadOnlyReactiveCollection<BattleAction>>
    {
        private readonly Subject<BattleAction> _actionSubmitted = new Subject<BattleAction>();
        private readonly List<MoveUI> _boundMoves = new List<MoveUI>();

        [SerializeField]
        private MoveUI moveUI;

        [SerializeField]
        private Transform layoutParent;

        [SerializeField]
        private Button passTurnButton;

        private ObjectPool<MoveUI> _moveUiPool;
        private CompositeDisposable _disposables;

        /// <summary>
        /// Gets an observable that changes each time the user submits a new action.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<BattleAction> ObserveActionSubmitted() => _actionSubmitted;

        /// <summary>
        /// Gets an observable that changes each time the user passer their turn without selecting an action.
        /// </summary>
        /// <returns>An observable.</returns>
        public IObservable<Unit> ObserveTurnPassed() => passTurnButton.OnClickAsObservable();

        /// <inheritdoc/>
        public override void BindTo(IReadOnlyReactiveCollection<BattleAction> actions)
        {
            base.BindTo(actions);

            // Return all active moves.
            foreach (MoveUI boundMove in _boundMoves)
            {
                _moveUiPool.Release(boundMove);
            }

            _boundMoves.Clear();
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            // Bind all new moves.
            foreach (BattleAction action in actions)
            {
                var instance = _moveUiPool.Get();
                instance.BindTo(action);

                _boundMoves.Add(instance);
                instance.OnPointerClickAsObservable().Subscribe(_ => _actionSubmitted.OnNext(action)).AddTo(_disposables);
                instance.OnSubmitAsObservable().Subscribe(_ => _actionSubmitted.OnNext(action)).AddTo(_disposables);
            }
        }

        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();
            _moveUiPool = new ObjectPool<MoveUI>(() => Instantiate(moveUI, layoutParent));
            _moveUiPool.ActionOnGet += moveUi =>
            {
                moveUi.gameObject.SetActive(true);
            };
            _moveUiPool.ActionOnRelease += moveUi =>
            {
                moveUi.gameObject.SetActive(false);
            };
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _disposables.Dispose();
        }
    }
}
