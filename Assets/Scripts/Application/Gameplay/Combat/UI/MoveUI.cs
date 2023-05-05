namespace Application.Gameplay.Combat.UI
{
    using Actions;
    using Core;
    using TMPro;
    using UnityEngine;

    /// <inheritdoc/>
    public class MoveUI : UIView<BattleAction>
    {
        [SerializeReference]
        private BattleAction autoBind;

        [SerializeField]
        private TMP_Text nameDisplay;

        [SerializeField]
        private TMP_Text descriptionDisplay;

        [SerializeField]
        private TMP_Text costDisplay;

        /// <inheritdoc/>
        public override void BindTo(BattleAction target)
        {
            base.BindTo(target);

            if (target != null)
            {
                nameDisplay.text = target.Name;
                descriptionDisplay.text = target.Description;

                if (costDisplay)
                {
                    if (target.Cost != 0)
                        costDisplay.text = $"Cost: {target.Cost}";

                    else costDisplay.text = string.Empty;
                }
            }
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            BindTo(autoBind);
        }
    }
}
