using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The information needed to start an overworld battle.
/// </summary>
public struct OverworldBattleStartData
{
    public List<GameObject> PlayerTeamInstances;
    public List<GameObject> EnemyTeamInstances;
}