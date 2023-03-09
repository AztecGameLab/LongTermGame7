using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Application.Gameplay.Combat.UI
{
    public class TeamData
    {
        public List<TeamMember> UnlockedMembers;
        public List<TeamMember> SelectedMembers;
    }

    public class TeamMember
    {
        public string Name;
        public string Description;
        public List<BattleAction> Actions;
        
        public float MaxHealth;
        public float CurrentHealth;
    }

    public class MoveListUI : View<IEnumerable<BattleAction>>
    {
        [SerializeField] private Transform listTarget;
        [SerializeField] private MoveView moveView;
        
        public override void BindTo(IEnumerable<BattleAction> actions)
        {
            foreach (BattleAction action in actions)
            {
                var instance = Instantiate(moveView, listTarget);
                instance.BindTo(action);
            }
        }
    }
        
    public class TeamMemberUI : View<TeamMember>
    {
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private MoveListUI moveListUI;
        
        public override void BindTo(TeamMember target)
        {
            name.text = target.Name;
            description.text = target.Description;

        }
    }
    
    public class TeamSelectionUI : View<TeamData>
    {
        [SerializeField] private TeamMemberUI teamMemberUI;
        [SerializeField] private Transform listTarget;
        
        public override void BindTo(TeamData target)
        {
            foreach (var member in target.UnlockedMembers)
            {
                var instance = Instantiate(teamMemberUI, listTarget);
                instance.BindTo(member);
            }
        }
    }
}