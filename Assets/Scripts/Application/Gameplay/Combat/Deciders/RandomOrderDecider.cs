using System.Collections;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    [CreateAssetMenu]
    public class RandomOrderDecider : EnemyOrderDecider
    {
        public override IEnumerator ExecuteTurn(BattleController controller)
        {
            GameObject randomEnemy = controller.EnemyTeam[Random.Range(0, controller.EnemyTeam.Count)];
            Debug.Log($"Random order decider chose the enemy {randomEnemy.name}.");
            
            if (randomEnemy.TryGetComponent(out MonsterBrain brain))
            {
                yield return brain.MakeDecision();
            }
        }
    }
}