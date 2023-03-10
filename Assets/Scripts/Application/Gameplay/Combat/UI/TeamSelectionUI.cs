namespace Application.Gameplay.Combat.UI
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// A user-interface for managing your current team to take into combat.
    /// </summary>
    public class TeamSelectionUI : View<TeamData>
    {
        [SerializeField]
        private TeamMemberListUI unlockedMembers;

        [SerializeField]
        private TeamMemberListUI selectedMembers;

        /// <inheritdoc/>
        public override void BindTo(TeamData target)
        {
            unlockedMembers.BindTo(target.UnlockedMembers);
            selectedMembers.BindTo(target.SelectedMembers);

            unlockedMembers.ObserveMemberClicked()
                .Subscribe(member => Move(member, target.UnlockedMembers, target.SelectedMembers))
                .AddTo(this);

            selectedMembers.ObserveMemberClicked()
                .Subscribe(member => Move(member, target.SelectedMembers, target.UnlockedMembers))
                .AddTo(this);
        }

        private static void Move<T>(T item, ICollection<T> from, ICollection<T> to)
        {
            from.Remove(item);
            to.Add(item);
        }
    }
}
