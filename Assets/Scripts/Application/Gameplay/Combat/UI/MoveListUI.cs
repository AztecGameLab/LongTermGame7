namespace Application.Gameplay.Combat.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A user-interface that displays a list of battle actions.
    /// </summary>
    public class MoveListUI : View<IEnumerable<BattleAction>>
    {
        [SerializeField]
        private Transform listTarget;

        [SerializeField]
        private MoveUI moveUI;

        /// <inheritdoc/>
        public override void BindTo(IEnumerable<BattleAction> target)
        {
            foreach (BattleAction action in target)
            {
                var instance = Instantiate(moveUI, listTarget);
                instance.BindTo(action);
            }
        }
    }
}
