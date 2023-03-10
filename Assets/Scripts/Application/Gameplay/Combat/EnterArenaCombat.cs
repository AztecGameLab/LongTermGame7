namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Core;
    using Deciders;
    using Hooks;
    using UnityEngine;

    /// <summary>
    /// The in-game trigger effect that will teleport the player to a combat arena.
    /// </summary>
    public class EnterArenaCombat : TriggerEffect
    {
        [SerializeField]
        private List<GameObject> enemyTeamPrefabs;

        [SerializeField]
        private EnemyOrderDecider enemyOrderDecider;

        [SerializeReference]
        private List<Hook> hooks;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            // List<GameObject> enemyTeamPrefabs = new List<GameObject>(); // todo: properly get random enemies, or whatever
            List<TeamMemberData> playerTeamData = new List<TeamMemberData>(Services.PlayerTeamData.SelectedMembers)
            {
                Services.PlayerTeamData.Player,
            };

            var battleData = new ArenaBattleStartData(playerTeamData, enemyTeamPrefabs, hooks, enemyOrderDecider);
            Services.EventBus.Invoke(battleData, $"Arena Combat Trigger: {gameObject.name}");
        }
    }
}
