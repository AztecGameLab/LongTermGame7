using System;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Application.Gameplay.Combat.UI
{
    public class TeamMemberUI : View<TeamMemberData>
    {
        [SerializeField] private TMP_Text name;
        [SerializeField] private TMP_Text description;
        [SerializeField] private MoveListUI moveListUI;

        public override void BindTo(TeamMemberData target)
        {
            name.text = target.name;
            description.text = target.description;
            moveListUI.BindTo(target.actions);
        }
    }
}