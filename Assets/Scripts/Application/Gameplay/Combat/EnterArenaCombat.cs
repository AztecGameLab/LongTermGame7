namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Core;
    using Deciders;
    using Hooks;
    using TriInspector;
    using UnityEngine;

    /// <summary>
    /// The in-game trigger effect that will teleport the player to a combat arena.
    /// </summary>
    public class EnterArenaCombat : TriggerEffect
    {
        [SerializeField]
        private List<GameObject> enemyTeamPrefabs;

        [Required]
        [SerializeField]
        private EnemyOrderDecider enemyOrderDecider;

        [SerializeReference]
        private List<Hook> hooks;

        [Scene]
        [SerializeField]
        private string arenaSceneName;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            // List<GameObject> enemyTeamPrefabs = new List<GameObject>(); // todo: properly get random enemies, or whatever
            List<TeamMemberData> playerTeamData = new List<TeamMemberData>(Services.PlayerTeamData.SelectedMembers)
            {
                Services.PlayerTeamData.Player,
            };

            var battleData = new ArenaBattleStartData(playerTeamData, enemyTeamPrefabs, hooks, arenaSceneName, enemyOrderDecider);
            Services.EventBus.Invoke(battleData, $"Arena Combat Trigger: {gameObject.name}");
        }
    }
}
