using Application.Gameplay.Combat.Deciders;
using Application.Gameplay.Combat.Hooks;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat
{
    /// <summary>
    /// The information needed to start a battle.
    /// </summary>
    public struct BattleData
    {
        public List<GameObject> PlayerTeamInstances;
        public List<GameObject> EnemyTeamInstances;
        public List<Hook> Hooks;

        public EnemyOrderDecider Decider;
    }
}