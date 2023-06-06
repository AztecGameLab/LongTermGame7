using TMPro;

namespace Application.Gameplay.Combat.UI
{
    using System;
    using Core;
    using UniRx;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A UI that displays the current remaining action points.
    /// </summary>
    public class ActionPointTrackerUI : UIView<ActionPointTracker>
    {
        [SerializeField]
        private Slider actionPointSlider;

        [SerializeField]
        private Slider predictedCostSlider;

        [SerializeField]
        private TMP_Text remainingPointsText;

        private IDisposable _disposable;
        private float _predictedTarget;
        private float _realTarget;

        /// <inheritdoc/>
        public override void BindTo(ActionPointTracker target)
        {
            base.BindTo(target);
            _disposable?.Dispose();

            actionPointSlider.minValue = 0;
            actionPointSlider.maxValue = target.MaxActionPoints;
            actionPointSlider.value = target.RemainingActionPoints;
            _realTarget = target.RemainingActionPoints;

            predictedCostSlider.minValue = 0;
            predictedCostSlider.maxValue = target.MaxActionPoints;

            var compositeDisposable = new CompositeDisposable();
            // compositeDisposable.Add(target.ObserveRemainingActionPoints().Subscribe(points => actionPointSlider.value = points));
            compositeDisposable.Add(target.ObserveMaxActionPoints().Subscribe(max => actionPointSlider.maxValue = max));
            compositeDisposable.AddTo(this);

            compositeDisposable.Add(target.ObserveRemainingActionPoints()
                .Subscribe(_ => UpdateRemainingPoints(target.RemainingActionPoints, target.MaxActionPoints)));

            compositeDisposable.Add(target.ObserveMaxActionPoints()
                .Subscribe(_ => UpdateRemainingPoints(target.RemainingActionPoints, target.MaxActionPoints)));

            _disposable = compositeDisposable;
        }

        public void SetPredictedCost(int value)
        {
            _predictedTarget = actionPointSlider.value - value;
        }

        private void UpdateRemainingPoints(int current, int max)
        {
            _realTarget = current;

            if (remainingPointsText)
                remainingPointsText.text = $"{current}/{max}";
        }

        private void Update()
        {
            actionPointSlider.value = Mathf.Lerp(actionPointSlider.value, _realTarget, 15 * Time.deltaTime);
            predictedCostSlider.value = Mathf.Lerp(predictedCostSlider.value, _predictedTarget, 15 * Time.deltaTime);
        }
    }
}
