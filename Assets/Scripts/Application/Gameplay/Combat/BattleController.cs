using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The generic controller for all battles.
/// Handles the common turn sequencing and logic, win and lose conditions.
/// </summary>
public class BattleController
{
    public List<GameObject> PlayerTeam;
    public List<GameObject> EnemyTeam;
    
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

    public void EndBattle()
    {
        // todo: we may have to pass more information on the ending of battle, e.g. win vs. loss and whatnot
        Debug.Log("Ending battle!");
    }
}
