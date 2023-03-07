namespace Application.Gameplay.Combat.States
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The state of a battle where an introductory cutscene can be played.
    /// </summary>
    [Serializable]
    public class BattleIntro : BattleState
    {
        /// <summary>
        /// Sets up the battle intro state.
        /// </summary>
        public void Initialize()
        {
            // Method intentionally left empty.
        }

        /// <inheritdoc/>
        public override void OnEnter()
        {
            // todo: add a way for custom battle intro / cutscene stuff?
            // we probably also want to do some common camera movements or animations.
            Debug.Log("Battle intro is a stub right now, but its running!");
            Controller.TransitionTo(Controller.Round);
        }
    }
}
