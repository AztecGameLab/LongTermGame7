namespace Application.Gameplay.Combat.UI
{
    /// <summary>
    /// A GameObject representation of a team member.
    /// </summary>
    public class TeamMemberWorldView : View<TeamMemberData>
    {
        /// <inheritdoc/>
        public override void BindTo(TeamMemberData target)
        {
            base.BindTo(target);

            if (TryGetComponent(out LivingEntity livingEntity))
            {
                livingEntity.Initialize(target.CurrentHealth, target.MaxHealth);
            }

            if (TryGetComponent(out ActionSet actionSet))
            {
                actionSet.Initialize(target.Actions);
            }
        }
    }
}
