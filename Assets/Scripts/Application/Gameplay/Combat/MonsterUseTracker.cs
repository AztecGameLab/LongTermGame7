using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    public class MonsterUseTracker : MonoBehaviour
    {
        public bool HasBeenUsed { get; }
    }

    // public class MoveSet : MonoBehaviour
    // {
    //     public List<Move> Moves { get; } = new List<Move>();
    // }
    //
    // public interface IMove
    // {
    //     public abstract IEnumerator Use(BattleController controller);
    //     public abstract MoveView View { get; }
    // }
    //
    // public abstract class MoveView : MonoBehaviour
    // {
    //     public event Action OnSelect;
    // }
    //
    // public class HealAllFriendlies : ScriptableObject
    // {
    //     public override IEnumerator Use(BattleController controller)
    //     {
    //         foreach (GameObject gameObject in controller.PlayerTeam)
    //         {
    //             if (gameObject.TryGetComponent(out Health health))
    //             {
    //                 health.Heal(1);
    //             }
    //         }
    //
    //         yield return null;
    //     }
    //
    //     public override MoveView View { get; }
    // }
}
