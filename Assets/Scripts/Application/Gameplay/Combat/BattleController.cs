using TMPro;
using UnityEngine;

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
