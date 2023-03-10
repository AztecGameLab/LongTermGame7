namespace Application.Gameplay.Combat.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// An authoring utility for player team members, AKA monsters.
    /// Provides an easy way to define the member data in the editor,
    /// and quickly assign it to different places in the game.
    /// </summary>
    [CreateAssetMenu]
    public class TeamMemberAuthoring : ScriptableObject
    {
        [SerializeField]
        private string memberName;

        [SerializeField]
        private string memberDescription;

        [SerializeReference]
        private List<BattleAction> actions;

        [SerializeField]
        private float maxHealth;

        /// <summary>
        /// Creates a new team member based off this authoring data.
        /// </summary>
        /// <returns>A new team member with the editor-defined data.</returns>
        public TeamMemberData GenerateData()
        {
            var result = new TeamMemberData
            {
                Name = memberName,
                Description = memberDescription,
                CurrentHealth = maxHealth,
                MaxHealth = maxHealth,
            };

            foreach (BattleAction battleAction in actions)
            {
                result.Actions.Add(battleAction);
            }

            return result;
        }
    }
}
