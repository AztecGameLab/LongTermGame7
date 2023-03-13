using Application.Core;
using Application.Gameplay.Combat.States;
using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class RoundUI : UIView<BattleRound>
    {
        [SerializeField] private TMP_Text roundNumberText;

        private IDisposable _disposable;
        
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