namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Core;
    using Deciders;
    using Hooks;
    using UnityEngine;

    /// <summary>
    /// The in-game trigger effect that can begin a combat sequence.
    /// </summary>
    public class EnterOverworldCombat : TriggerEffect
    {
        [SerializeField]
        private List<GameObject> enemyTeam;

        [SerializeField]
        private EnemyOrderDecider enemyOrderDecider;

        [SerializeReference]
        private List<Hook> hooks;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            List<GameObject> enemyTeamInstances = enemyTeam;
            IReadOnlyCollection<GameObject> playerTeamInstances = FindObjectOfType<PlayerPartyView>().PartyMemberInstances;

            var battleData = new OverworldBattleStartData(playerTeamInstances, enemyTeamInstances, hooks, enemyOrderDecider);
            Services.EventBus.Invoke(battleData, $"Overworld Combat Trigger: {gameObject.name}");
        }
    }
}
