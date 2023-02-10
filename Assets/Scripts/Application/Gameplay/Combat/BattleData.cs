using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The information needed to start a battle.
/// </summary>
public struct BattleData
{
    public List<GameObject> PlayerTeamInstances;
    public List<GameObject> EnemyTeamInstances;
}