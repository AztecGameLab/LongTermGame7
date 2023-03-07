namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// A logical controller on a per-entity basis.
    /// Manages the immediate behavior of an enemy when given an opportunity.
    /// </summary>
    public abstract class MonsterBrain : MonoBehaviour
    {
        /// <summary>
        /// Runs a single unit of decision logic for this enemy.
        /// For example, run towards the player and then throw a rock.
        /// Or (for a boss), create some minions and back away.
        /// </summary>
        /// <returns>Unity coroutine information.</returns>
        public abstract IEnumerator MakeDecision();
    }
}
