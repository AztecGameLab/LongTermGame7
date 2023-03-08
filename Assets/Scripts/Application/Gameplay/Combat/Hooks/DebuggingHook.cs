using UnityEngine;

namespace Application.Gameplay.Combat.Hooks
{
    public class DebuggingHook : Hook
    {
        public override void OnBattleEnd()
        {
            Debug.Log("Battle End");
        }

        public override void OnBattleStart()
        {
            Debug.Log("Battle Start");
        }

        public override void OnBattleUpdate()
        {
            Debug.Log("Battle Update");
        }
    }
}
