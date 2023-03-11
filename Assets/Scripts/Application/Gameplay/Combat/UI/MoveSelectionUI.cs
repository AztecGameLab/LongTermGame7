using Application.Core;
using Application.Gameplay.Combat.Actions;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Application.Gameplay.Combat.UI
{
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

        private ObjectPool<MoveUI> _moveUiPool;
        private CompositeDisposable _disposables;

        public IObservable<BattleAction> ObserveActionSubmitted() => _actionSubmitted;

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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _disposables.Dispose();
        }

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
                var d = instance.OnSubmitAsObservable().Subscribe(_ => _actionSubmitted.OnNext(action));
                _disposables.Add(d);
            }
        }
    }
}
