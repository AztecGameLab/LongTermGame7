namespace Application.Gameplay.Combat.Deciders
{
    using System;
    using System.Collections;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// An abstraction of the logic that controls the enemy's turn.
    /// For example, one battle may require simple random decisions,
    /// and another (like a boss fight) may have a procedural strategy.
    /// </summary>
    public abstract class EnemyOrderDecider : ScriptableObject
    {
        /// <summary>
        /// Run and observe the execution of the enemy turn.
        /// </summary>
        /// <param name="controller">The parent controller to use when deciding turn behavior.</param>
        /// <returns>An observable that completes when the enemy turn finishes executing.</returns>
        public IObservable<Unit> Run(BattleController controller) => ExecuteTurn(controller).ToObservable();

        /// <summary>
        /// Performs all of the per-turn logic for a set of enemies.
        /// </summary>
        /// <param name="controller">The parent controller to use when deciding turn behavior.</param>
        /// <returns>Unity coroutine data.</returns>
        protected abstract IEnumerator ExecuteTurn(BattleController controller);
    }
}
