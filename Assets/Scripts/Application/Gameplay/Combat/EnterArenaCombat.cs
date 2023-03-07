﻿namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Core;
    using Deciders;
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

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            // List<GameObject> enemyTeamPrefabs = new List<GameObject>(); // todo: properly get random enemies, or whatever
            List<GameObject> playerTeamPrefabs = FindObjectOfType<PlayerPartyView>().PartyMemberInstances;

            var battleData = new ArenaBattleStartData
            {
                EnemyTeamPrefabs = enemyTeamPrefabs,
                PlayerTeamPrefabs = playerTeamPrefabs,
                EnemyOrderDecider = enemyOrderDecider,
            };

            Services.EventBus.Invoke(battleData, $"Arena Combat Trigger: {gameObject.name}");
        }
    }
}
