using Application.Core;
using Application.Gameplay.Combat.Deciders;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    public class EnterArenaCombat : TriggerEffect
    {
        [SerializeField] private List<GameObject> enemyTeamPrefabs;
        [SerializeField] private EnemyOrderDecider enemyOrderDecider;
    
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