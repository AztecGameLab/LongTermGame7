using Application.Core;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Gameplay.Combat.UI
{
    public class ActionPointTrackerUI : UIView<ActionPointTracker>
    {
        [SerializeField]
        private Slider actionPointSlider;

        private IDisposable _disposable;
        
        public override void BindTo(ActionPointTracker target)
        {
            base.BindTo(target);
            _disposable?.Dispose();

            actionPointSlider.minValue = 0;
            actionPointSlider.maxValue = target.MaxActionPoints;
            actionPointSlider.value = target.RemainingActionPoints;

            var compositeDisposable = new CompositeDisposable();
            compositeDisposable.Add(target.ObserveRemainingActionPoints().Subscribe(points => actionPointSlider.value = points));
            compositeDisposable.Add(target.ObserveMaxActionPoints().Subscribe(max => actionPointSlider.maxValue = max));
            compositeDisposable.AddTo(this);

            _disposable = compositeDisposable;
        }
    }
}
