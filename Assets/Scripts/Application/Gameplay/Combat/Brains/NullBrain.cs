using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    public class NullBrain : MonsterBrain
    {
        public override IEnumerator MakeDecision()
        {
            Debug.Log($"Monster {gameObject.name} did absolutely nothing: head empty...");
            yield return null;
        }
    }
}