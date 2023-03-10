using UniRx;

namespace Application.Gameplay.Combat.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// A user-interface that displays a list of battle actions.
    /// </summary>
    public class MoveListUI : UIView<IReadOnlyReactiveCollection<BattleAction>>
    {
        [SerializeField]
        private Transform listTarget;

        [SerializeField]
        private MoveUI moveUI;

        /// <inheritdoc/>
        public override void BindTo(IReadOnlyReactiveCollection<BattleAction> target)
        {
            base.BindTo(target);

            foreach (BattleAction action in target)
            {
                var instance = Instantiate(moveUI, listTarget);
                instance.BindTo(action);
            }
        }
    }
}
