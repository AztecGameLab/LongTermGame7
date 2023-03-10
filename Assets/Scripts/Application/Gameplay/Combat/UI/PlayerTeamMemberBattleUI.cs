namespace Application.Gameplay.Combat.UI
{
    using UnityEngine;

    /// <summary>
    /// A view for player GameObjects during battle.
    /// </summary>
    public class PlayerTeamMemberBattleUI : UIView<GameObject>
    {
        [SerializeField]
        private HealthBar healthBar;

        /// <inheritdoc/>
        public override void BindTo(GameObject target)
        {
            base.BindTo(target);

            if (target == null)
            {
                return;
            }

            if (target.TryGetComponent(out LivingEntity livingEntity))
            {
                healthBar.gameObject.SetActive(true);
                healthBar.BindTo(livingEntity);
            }
        }
    }
}
