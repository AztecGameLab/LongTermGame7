using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Gameplay.Combat.UI
{
    [Serializable]
    public class TeamData
    {
        public ReactiveCollection<TeamMemberData> unlockedMembers = new ReactiveCollection<TeamMemberData>();
        public ReactiveCollection<TeamMemberData> selectedMembers = new ReactiveCollection<TeamMemberData>();
    }

    public class TeamMemberData
    {
        public string name;
        public string description;
        
        public List<BattleAction> actions;
        
        public float maxHealth;
        public float currentHealth;
    }

    public class TeamSelectionUI : View<TeamData>
    {
        [SerializeField] private TeamMemberListUI unlockedMembers;
        [SerializeField] private TeamMemberListUI selectedMembers;
        
        public override void BindTo(TeamData target)
        {
            unlockedMembers.BindTo(target.unlockedMembers);
            selectedMembers.BindTo(target.selectedMembers);

            unlockedMembers.ObserveMemberClicked()
                .Subscribe(member => Move(member, target.unlockedMembers, target.selectedMembers))
                .AddTo(this);
            
            selectedMembers.ObserveMemberClicked()
                .Subscribe(member => Move(member, target.selectedMembers, target.unlockedMembers))
                .AddTo(this);
        }

        private void Move<T>(T item, ICollection<T> from, ICollection<T> to)
        {
            from.Remove(item);
            to.Add(item);
        }
    }
}