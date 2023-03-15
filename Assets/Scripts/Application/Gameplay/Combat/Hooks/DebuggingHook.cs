namespace Application.Gameplay.Combat.Hooks
{
    using UnityEngine;

    /// <summary>
    /// A simple hook for testing.
    /// </summary>
    public class DebuggingHook : Hook
    {
        /// <inheritdoc/>
        public override void OnBattleEnd()
        {
            Debug.Log("Battle End");
        }

        /// <inheritdoc/>
        public override void OnBattleStart()
        {
            Debug.Log("Battle Start");
        }

        /// <inheritdoc/>
        public override void OnBattleUpdate()
        {
            // Method intentionally left empty.
        }
    }
}
