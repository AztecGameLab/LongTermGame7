namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A container for actions that an entity can use.
    /// </summary>
    public class ActionSet : MonoBehaviour
    {
        [SerializeReference]
        private List<BattleAction> actions;

        /// <summary>
        /// Gets a list of actions that this entity can use.
        /// </summary>
        /// <value>
        /// A list of actions that this entity can use.
        /// </value>
        public IReadOnlyList<BattleAction> Actions => actions;
    }
}
