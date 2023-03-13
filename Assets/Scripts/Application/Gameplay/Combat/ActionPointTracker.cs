using System;

namespace Application.Gameplay.Combat
{
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Keeps track of how many action points this monster has available.
    /// </summary>
    public class ActionPointTracker : MonoBehaviour
    {
        [SerializeField]
        private IntReactiveProperty remainingActionPoints;

        [SerializeField]
        private IntReactiveProperty maxActionPoints;

        /// <summary>
        /// Gets or sets the maximum action points this monster can hold.
        /// </summary>
        public int MaxActionPoints
        {
            get => maxActionPoints.Value;
            set => maxActionPoints.Value = value;
        }

        /// <summary>
        /// Gets the available action points that can be spent.
        /// </summary>
        public int RemainingActionPoints => remainingActionPoints.Value;

        /// <summary>
        /// Gets an observable that changes each time the remaining points change.
        /// </summary>
        /// <returns>An observable that changes each time the remaining points change.</returns>
        public IObservable<int> ObserveRemainingActionPoints() => remainingActionPoints;

        /// <summary>
        /// Gets an observable that changes each time the maximum points change.
        /// </summary>
        /// <returns>Gets an observable that changes each time the maximum points change.</returns>
        public IObservable<int> ObserveMaxActionPoints() => maxActionPoints;

        /// <summary>
        /// Check if this monster can spend a certain amount of action points.
        /// </summary>
        /// <param name="cost">The cost we are evaluating against.</param>
        /// <returns>True if we have enough points to spend, false if we do not.</returns>
        public bool CanAfford(int cost)
        {
            return RemainingActionPoints - cost >= 0;
        }

        /// <summary>
        /// If this monster can afford a certain amount of action points, subtract them from the pool.
        /// </summary>
        /// <param name="amount">The cost we are trying to apply.</param>
        /// <returns>True if we successfully payed the desired amount, false if the payment didn't go through.</returns>
        public bool TrySpend(int amount)
        {
            bool success = CanAfford(amount);

            if (success)
            {
                remainingActionPoints.Value -= amount;
            }

            return success;
        }

        /// <summary>
        /// Refills the remaining action points to the max.
        /// </summary>
        public void Refill()
        {
            remainingActionPoints.Value = maxActionPoints.Value;
        }
    }
}
