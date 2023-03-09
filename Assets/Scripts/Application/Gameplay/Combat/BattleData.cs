namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using Deciders;
    using Hooks;
    using UnityEngine;

    /// <summary>
    /// The information needed to start a battle.
    /// </summary>
    public readonly struct BattleData
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
        /// The hooks that should be loaded into the combat.
        /// </summary>
        public readonly IReadOnlyCollection<Hook> Hooks;

        /// <summary>
        /// The script in charge of running enemy turns.
        /// </summary>
        public readonly EnemyOrderDecider Decider;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleData"/> struct.
        /// </summary>
        /// <param name="playerTeamInstances">The spawned instances of each player team member.</param>
        /// <param name="enemyTeamInstances">The spawned instances of each enemy team member.</param>
        /// <param name="hooks">The hooks that should be loaded into the combat.</param>
        /// <param name="enemyOrderDecider">The script in charge of running enemy turns.</param>
        public BattleData(
            IReadOnlyCollection<GameObject> playerTeamInstances,
            IReadOnlyCollection<GameObject> enemyTeamInstances,
            IReadOnlyCollection<Hook> hooks,
            EnemyOrderDecider enemyOrderDecider)
        {
            PlayerTeamInstances = playerTeamInstances;
            EnemyTeamInstances = enemyTeamInstances;
            Hooks = hooks;
            Decider = enemyOrderDecider;
        }
    }
}
