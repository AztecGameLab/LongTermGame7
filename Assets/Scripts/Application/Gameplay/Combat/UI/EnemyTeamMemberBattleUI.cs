﻿namespace Application.Gameplay.Combat.UI
{
    using UnityEngine;

    /// <summary>
    /// A view for enemy GameObjects during battle.
    /// </summary>
    public class EnemyTeamMemberBattleUI : View<GameObject>
    {
        [SerializeField]
        private HealthBar healthBar;

        /// <inheritdoc/>
        public override void BindTo(GameObject target)
        {
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