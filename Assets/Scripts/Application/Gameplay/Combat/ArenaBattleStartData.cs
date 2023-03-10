using Application.Gameplay.Combat.UI;

namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Deciders;
    using Hooks;
    using UnityEngine;

    /// <summary>
    /// The information needed to start an arena battle.
    /// </summary>
    public readonly struct ArenaBattleStartData
    {
        /// <summary>
        /// The prefabs used to spawn the player team.
        /// </summary>
        public readonly IReadOnlyCollection<TeamMemberData> PlayerTeamData;

        /// <summary>
        /// The prefabs used to spawn the enemy team.
        /// </summary>
        public readonly IReadOnlyCollection<GameObject> EnemyTeamPrefabs;

        /// <summary>
        /// The hooks to be added into the battle.
        /// </summary>
        public readonly IReadOnlyCollection<Hook> Hooks;

        /// <summary>
        /// The decider in charge of executing enemy turns.
        /// </summary>
        public readonly EnemyOrderDecider EnemyOrderDecider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArenaBattleStartData"/> struct.
        /// </summary>
        /// <param name="playerTeamData">The prefabs to spawn for the players.</param>
        /// <param name="enemyTeamPrefabs">The prefabs to spawn for the enemies.</param>
        /// <param name="hooks">The hooks to be added into the battle.</param>
        /// <param name="enemyOrderDecider">The decider in charge of executing enemy turns.</param>
        public ArenaBattleStartData(
            IReadOnlyCollection<TeamMemberData> playerTeamData,
            IReadOnlyCollection<GameObject> enemyTeamPrefabs,
            IReadOnlyCollection<Hook> hooks,
            EnemyOrderDecider enemyOrderDecider)
        {
            PlayerTeamData = playerTeamData;
            EnemyTeamPrefabs = enemyTeamPrefabs;
            Hooks = hooks;
            EnemyOrderDecider = enemyOrderDecider;
        }
    }
}
