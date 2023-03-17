namespace Application.Gameplay
{
    using Combat;
    using Core;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// A GameObject representation of a team member.
    /// </summary>
    public class TeamMemberWorldView : View<TeamMemberData>
    {
        [SerializeField]
        private TextMeshPro nameDisplay;

        /// <inheritdoc/>
        public override void BindTo(TeamMemberData target)
        {
            base.BindTo(target);

            nameDisplay.text = target.Name;

            if (TryGetComponent(out LivingEntity livingEntity))
            {
                livingEntity.Initialize(target.CurrentHealth, target.MaxHealth);
            }

            if (TryGetComponent(out ActionSet actionSet))
            {
                actionSet.Initialize(target.Actions);
            }

            if (TryGetComponent(out ActionPointTracker actionPointTracker))
            {
                actionPointTracker.MaxActionPoints = target.MaxActionPoints;
            }
        }
    }
}
