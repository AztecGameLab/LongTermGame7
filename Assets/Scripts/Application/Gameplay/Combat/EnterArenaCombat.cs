using Application.Core;
using Application.Gameplay;
using Application.Gameplay.Combat;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnterArenaCombat : TriggerEffect
{
    [SerializeField] private List<GameObject> enemyTeamPrefabs;
    [SerializeField] private EnemyOrderDecider enemyOrderDecider;
    
    protected override void HandleCollisionEnter(GameObject obj)
    {
        // List<GameObject> enemyTeamPrefabs = new List<GameObject>(); // todo: properly get random enemies, or whatever 
        List<GameObject> playerTeamPrefabs = FindObjectOfType<PlayerPartyView>().Party.GetPartyMemberPrefabs().ToList();

        var battleData = new ArenaBattleStartData
        {
            EnemyTeamPrefabs = enemyTeamPrefabs, 
            PlayerTeamPrefabs = playerTeamPrefabs,
            EnemyOrderDecider = enemyOrderDecider,
        };
        
        Services.EventBus.Invoke(battleData, $"Arena Combat Trigger: {gameObject.name}");
    }
}