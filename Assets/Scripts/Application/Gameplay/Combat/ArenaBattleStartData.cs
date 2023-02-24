using Application.Gameplay.Combat.Deciders;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    /// <summary>
    /// The information needed to start an arena battle.
    /// </summary>
    public struct ArenaBattleStartData
    {
        public List<GameObject> PlayerTeamPrefabs;
        public List<GameObject> EnemyTeamPrefabs;
        public EnemyOrderDecider EnemyOrderDecider;
    }
}