﻿namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// The most basic monster brain you can have.
    /// Literally, does nothing but log to the console.
    /// </summary>
    public class NullBrain : MonsterBrain
    {
        /// <inheritdoc/>
        protected override IEnumerator MakeDecision(BattleController controller)
        {
            Debug.Log($"Monster {gameObject.name} did absolutely nothing: head empty...");
            yield return null;
        }
    }
}
