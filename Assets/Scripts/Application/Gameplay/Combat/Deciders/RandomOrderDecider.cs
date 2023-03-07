namespace Application.Gameplay.Combat.Deciders
{
    using System.Collections;
    using Brains;
    using UnityEngine;

    /// <summary>
    /// Randomly deciders the order for monsters to move.
    /// Works well in purely random encounters as a fallback.
    /// </summary>
    [CreateAssetMenu]
    public class RandomOrderDecider : EnemyOrderDecider
    {
        /// <inheritdoc/>
        protected override IEnumerator ExecuteTurn(BattleController controller)
        {
            if (controller == null)
            {
                yield break;
            }

            GameObject randomEnemy = controller.EnemyTeam[Random.Range(0, controller.EnemyTeam.Count)];
            Debug.Log($"Random order decider chose the enemy {randomEnemy.name}.");

            if (randomEnemy.TryGetComponent(out MonsterBrain brain))
            {
                yield return brain.MakeDecision();
            }
        }
    }
}
