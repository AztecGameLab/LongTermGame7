namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Deciders;
    using Hooks;
    using UnityEngine;

    /// <summary>
    /// The information needed to start an overworld battle.
    /// </summary>
    public readonly struct OverworldBattleStartData
    {
        /// <summary>
        /// The spawned instances of each player team member.
        /// </summary>
        public readonly IReadOnlyCollection<GameObject> PlayerTeamInstances;

        /// <summary>
        /// The spawned instances of each enemy team member.
        /// </summary>
        public readonly IReadOnlyCollection<GameObject> EnemyTeamInstances;

        /// <summary>
        /// The hooks to be loaded in.
        /// </summary>
        public readonly IReadOnlyCollection<Hook> Hooks;

        /// <summary>
        /// The controller for enemy turns.
        /// </summary>
        public readonly EnemyOrderDecider EnemyOrderDecider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverworldBattleStartData"/> struct.
        /// </summary>
        /// <param name="playerTeamInstances">The spawned instances of each player team member.</param>
        /// <param name="enemyTeamInstances">The spawned instances of each enemy team member.</param>
        /// <param name="hooks">The hooks to be loaded in.</param>
        /// <param name="orderDecider">The controller for enemy turns.</param>
        public OverworldBattleStartData(
            IReadOnlyCollection<GameObject> playerTeamInstances,
            IReadOnlyCollection<GameObject> enemyTeamInstances,
            IReadOnlyCollection<Hook> hooks,
            EnemyOrderDecider orderDecider)
        {
            PlayerTeamInstances = playerTeamInstances;
            EnemyTeamInstances = enemyTeamInstances;
            EnemyOrderDecider = orderDecider;
            Hooks = hooks;
        }
    }
}
