using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class TeamMemberListUI : View<IReadOnlyReactiveCollection<TeamMemberData>>
    {
        [SerializeField] private TeamMemberUI teamMemberUI;
        [SerializeField] private Transform listTarget;

        public IObservable<TeamMemberData> ObserveMemberClicked() => _memberClicked;

        private Subject<TeamMemberData> _memberClicked = new Subject<TeamMemberData>();
        private Dictionary<TeamMemberData, TeamMemberUI> _boundDataLookup = new Dictionary<TeamMemberData, TeamMemberUI>();

        public IReadOnlyReactiveCollection<TeamMemberData> Members;

        public override void BindTo(IReadOnlyReactiveCollection<TeamMemberData> members)
        {
            Members = members;
            
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
            instance.name = memberData.name;
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