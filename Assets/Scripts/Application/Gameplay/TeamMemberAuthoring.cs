namespace Application.Gameplay
{
    using System.Collections.Generic;
    using Combat.Actions;
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
        private string memberName = "Monster Name";

        [SerializeField]
        private string memberDescription = "Monster Description";

        [SerializeReference]
        private List<BattleAction> actions;

        [SerializeField]
        private float maxHealth = 5;

        [SerializeField]
        private string worldViewAssetPath;

        [SerializeField]
        private int maxActionPoints = 5;

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
                MaxActionPoints = maxActionPoints,
                WorldViewAssetPath = worldViewAssetPath,
            };

            foreach (BattleAction battleAction in actions)
            {
                result.Actions.Add(battleAction);
            }

            return result;
        }
    }
}
