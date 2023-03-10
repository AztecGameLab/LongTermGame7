using TMPro;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    public class TeamMemberUI : View<TeamMember>
    {
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private MoveListUI moveListUI;
        
        public override void BindTo(TeamMember target)
        {
            name.text = target.name;
            description.text = target.description;
            moveListUI.BindTo(target.actions);
        }
    }
}