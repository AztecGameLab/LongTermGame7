using Application.Core;
using Application.Gameplay;
using Application.Gameplay.Combat;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The in-game trigger effect that can begin a combat sequence.
/// </summary>
public class EnterOverworldCombat : TriggerEffect
{
    [SerializeField] private List<GameObject> enemyTeam;
    [SerializeField] private EnemyOrderDecider enemyOrderDecider;
    
    protected override void HandleCollisionEnter(GameObject obj)
    {
        List<GameObject> enemyTeamInstances = enemyTeam;
        List<GameObject> playerTeamInstances = FindObjectOfType<PlayerPartyView>().PartyMemberInstances;

        var battleData = new OverworldBattleStartData
        {
            EnemyTeamInstances = enemyTeamInstances, 
            PlayerTeamInstances = playerTeamInstances,
            EnemyOrderDecider = enemyOrderDecider,
        };
        
        Services.EventBus.Invoke(battleData, $"Overworld Combat Trigger: {gameObject.name}");
    }
}