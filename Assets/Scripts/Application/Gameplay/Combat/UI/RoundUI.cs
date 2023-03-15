namespace Application.Gameplay.Combat.UI
{
    using System;
    using Core;
    using States;
    using TMPro;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// A UI for viewing information about the current battle round.
    /// </summary>
    public class RoundUI : UIView<BattleRound>
    {
        [SerializeField]
        private TMP_Text roundNumberText;

        private IDisposable _disposable;

        /// <inheritdoc/>
        public override void BindTo(BattleRound round)
        {
            base.BindTo(round);

            _disposable?.Dispose();
            _disposable = round.RoundNumber.Subscribe(roundNumber =>
            {
                roundNumberText.text = $"Round {roundNumber}";
            });
            _disposable.AddTo(this);
        }
    }
}
