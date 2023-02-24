using Application.Gameplay.Combat.Deciders;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    /// <summary>
    /// The information needed to start an overworld battle.
    /// </summary>
    public struct OverworldBattleStartData
    {
        public List<GameObject> PlayerTeamInstances;
        public List<GameObject> EnemyTeamInstances;
        public EnemyOrderDecider EnemyOrderDecider;
    }
}