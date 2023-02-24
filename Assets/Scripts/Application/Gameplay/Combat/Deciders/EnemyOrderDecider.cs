using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat.Deciders
{
    public abstract class EnemyOrderDecider : ScriptableObject
    {
        public abstract IEnumerator ExecuteTurn(BattleController controller);
    }
}