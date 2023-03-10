namespace Application.Gameplay.Combat.UI
{
    using System;
    using System.Collections.Generic;
    using UniRx;
    using UniRx.Triggers;
    using UnityEngine;

    /// <summary>
    /// A user-interface for displaying a collection of team members.
    /// </summary>
    public class TeamMemberListUI : UIView<IReadOnlyReactiveCollection<TeamMemberData>>
    {
        private readonly Subject<TeamMemberData> _memberClicked
            = new Subject<TeamMemberData>();

        private readonly Dictionary<TeamMemberData, TeamMemberUI> _boundDataLookup
            = new Dictionary<TeamMemberData, TeamMemberUI>();

        [SerializeField]
        private TeamMemberUI teamMemberUI;

        [SerializeField]
        private Transform listTarget;

        /// <summary>
        /// An observable that emits every time a displayed member is clicked on.
        /// </summary>
        /// <returns>The aforementioned observable.</returns>
        public IObservable<TeamMemberData> ObserveMemberClicked() => _memberClicked;

        /// <inheritdoc/>
        public override void BindTo(IReadOnlyReactiveCollection<TeamMemberData> members)
        {
            base.BindTo(members);

            // Initially load members.
            foreach (var member in members)
            {
                CreateMember(member);
            }

            // Update visuals whenever the list changes.
            members.ObserveAdd().Select(addEvent => addEvent.Value).Subscribe(CreateMember).AddTo(this);
            members.ObserveRemove().Select(removeEvent => removeEvent.Value).Subscribe(RemoveMember).AddTo(this);
        }

        private void CreateMember(TeamMemberData memberData)
        {
            var instance = Instantiate(teamMemberUI, listTarget);
            instance.name = memberData.Name;
            instance.BindTo(memberData);
            instance.OnPointerClickAsObservable().Subscribe(_ => _memberClicked.OnNext(memberData)).AddTo(this);

            _boundDataLookup.Add(memberData, instance);
        }

        private void RemoveMember(TeamMemberData memberData)
        {
            Destroy(_boundDataLookup[memberData].gameObject);
            _boundDataLookup.Remove(memberData);
        }
    }
}
