namespace Application.Gameplay.Combat.UI
{
    using TMPro;
    using UnityEngine;

    /// <inheritdoc/>
    public class MoveUI : View<BattleAction>
    {
        [SerializeReference]
        private BattleAction autoBind;

        [SerializeField]
        private TMP_Text nameDisplay;

        [SerializeField]
        private TMP_Text descriptionDisplay;

        /// <inheritdoc/>
        public override void BindTo(BattleAction target)
        {
            if (target != null)
            {
                nameDisplay.text = target.Name;
                descriptionDisplay.text = target.Description;
            }
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            BindTo(autoBind);
        }
    }
}
