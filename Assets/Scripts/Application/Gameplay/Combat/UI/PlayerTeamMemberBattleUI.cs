using System;

namespace Application.Gameplay.Combat.UI
{
    using Core;
    using UnityEngine;

    /// <summary>
    /// A view for player GameObjects during battle.
    /// </summary>
    public class PlayerTeamMemberBattleUI : UIView<GameObject>
    {
        [SerializeField]
        private HealthBar healthBar;

        [SerializeField]
        private GameObject autoBind;

        protected override void Start()
        {
            base.Start();
            BindTo(autoBind);
        }

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
