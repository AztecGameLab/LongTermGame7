namespace Application.Gameplay.Combat.Brains
{
    using System;
    using System.Collections;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// A logical controller on a per-entity basis.
    /// Manages the immediate behavior of an enemy when given an opportunity.
    /// </summary>
    public abstract class MonsterBrain : MonoBehaviour
    {
        /// <summary>
        /// Runs a single unit of decision logic for this enemy.
        /// For example, run towards the player and throw a rock.
        /// Or (for a boss), create some minions and back away.
        /// </summary>
        /// <param name="controller">The battle controller to query for battle info.</param>
        /// <returns>An observable for when this logic finishes.</returns>
        public IObservable<Unit> Run(BattleController controller) => Observable.FromCoroutine(() => MakeDecision(controller));

        /// <summary>
        /// Runs a single unit of decision logic for this enemy.
        /// For example, run towards the player and then throw a rock.
        /// Or (for a boss), create some minions and back away.
        /// </summary>
        /// <param name="controller">The battle controller to query for battle info.</param>
        /// <returns>Unity coroutine information.</returns>
        protected abstract IEnumerator MakeDecision(BattleController controller);
    }
}
