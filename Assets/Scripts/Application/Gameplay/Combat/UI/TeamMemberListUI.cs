using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class TeamMemberListUI : View<IEnumerable<TeamMember>>
    {
        [SerializeField] private TeamMemberUI teamMemberUI;
        [SerializeField] private Transform listTarget;
        
        public override void BindTo(IEnumerable<TeamMember> target)
        {
            foreach (var member in target)
            {
                var instance = Instantiate(teamMemberUI, listTarget);
                instance.BindTo(member);
            }
        }
    }
}