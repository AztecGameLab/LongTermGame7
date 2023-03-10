namespace Application.Gameplay.Combat
{
    using System.Collections.Generic;
    using UniRx;
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
        public IReadOnlyReactiveCollection<BattleAction> Actions { get; private set; }

        /// <summary>
        /// Sets up this action set with initial values.
        /// </summary>
        /// <param name="targetActions">The actions to use.</param>
        public void Initialize(IReadOnlyReactiveCollection<BattleAction> targetActions)
        {
            Actions = targetActions;
        }

        private void Awake()
        {
            Actions = actions.ToReactiveCollection();
        }
    }
}
