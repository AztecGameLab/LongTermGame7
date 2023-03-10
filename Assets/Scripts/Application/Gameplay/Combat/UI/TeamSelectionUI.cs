using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Gameplay.Combat.UI
{
    [Serializable]
    public class TeamData
    {
        public List<TeamMember> unlockedMembers;
        public List<TeamMember> selectedMembers;
    }

    [Serializable]
    public class TeamMember
    {
        public string name;
        public string description;
        
        [SerializeReference]
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
        }
    }
}