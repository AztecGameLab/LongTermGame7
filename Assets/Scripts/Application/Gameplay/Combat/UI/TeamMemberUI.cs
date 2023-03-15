namespace Application.Gameplay.Combat.UI
{
    using Core;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// A user-interface for viewing information about a team member.
    /// </summary>
    public class TeamMemberUI : UIView<TeamMemberData>
    {
        [SerializeField]
        private TMP_Text memberName;

        [SerializeField]
        private TMP_Text memberDescription;

        [SerializeField]
        private MoveListUI moveListUI;

        /// <inheritdoc/>
        public override void BindTo(TeamMemberData target)
        {
            base.BindTo(target);

            memberName.text = target.Name;
            memberDescription.text = target.Description;
            moveListUI.BindTo(target.Actions);
        }
    }
}
