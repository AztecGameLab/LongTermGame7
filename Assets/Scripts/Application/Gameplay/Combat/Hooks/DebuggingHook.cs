using System.Collections;

namespace Application.Gameplay.Combat.Hooks
{
    using UnityEngine;

    /// <summary>
    /// A simple hook for testing.
    /// </summary>
    public class DebuggingHook : Hook
    {
        /// <inheritdoc/>
        public override IEnumerator OnBattleEnd()
        {
            yield return base.OnBattleEnd();
            Debug.Log("Battle End");
        }

        /// <inheritdoc/>
        public override IEnumerator OnBattleStart()
        {
            yield return base.OnBattleStart();
            Debug.Log("Battle Start");
        }
    }
}
