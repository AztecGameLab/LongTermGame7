using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    public abstract class MonsterBrain : MonoBehaviour
    {
        public abstract IEnumerator MakeDecision();
    }
}
