using Application.Core;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The information needed to start an overworld battle.
/// </summary>
public struct OverworldBattleStartData
{
    public List<GameObject> PlayerTeamInstances;
    public List<GameObject> EnemyTeamInstances;
}

/// <summary>
/// Listens for the beginning of overworld battles, and prepares the world state
/// for the <see cref="BattleController"/>. 
/// </summary>
public class OverworldBattleSetup : IDisposable
{
    private BattleController _controller;
    private IDisposable _disposable;
    
    public OverworldBattleSetup Init(BattleController controller)
    {
        _controller = controller;
        _disposable = Services.EventBus.AddListener<OverworldBattleStartData>(HandleBattleStart, "Overworld battle setup");
        return this;
    }

    public void Dispose()
    {
        _disposable.Dispose();
    }

    private void HandleBattleStart(OverworldBattleStartData data)
    {
        var battleData = new BattleData
        {
            EnemyTeamInstances = data.EnemyTeamInstances, 
            PlayerTeamInstances = data.PlayerTeamInstances,
        };
        
        _controller.BeginBattle(battleData);
    }
}

/// <summary>
/// The information needed to start a battle.
/// </summary>
public struct BattleData
{
    public List<GameObject> PlayerTeamInstances;
    public List<GameObject> EnemyTeamInstances;
}

/// <summary>
/// The generic controller for all battles.
/// Handles the common turn sequencing and logic, win and lose conditions.
/// </summary>
public class BattleController
{
    public void BeginBattle(BattleData data)
    {
        Debug.Log("Starting battle!");
        
        Debug.Log("Enemy objects:");

        foreach (GameObject enemyTeamInstance in data.EnemyTeamInstances)
        {
            Debug.Log(enemyTeamInstance.name);
        }
        
        Debug.Log("Friendly objects:");

        foreach (GameObject playerTeamInstance in data.PlayerTeamInstances)
        {
            Debug.Log(playerTeamInstance.name);
        }
    }
}
