namespace Application.Gameplay.Combat.UI
{
    using System.Collections.ObjectModel;
    using Newtonsoft.Json;

    /// <summary>
    /// Information about a member of the player's team.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TeamMemberData
    {
        /// <summary>
        /// Gets or sets the name of this team's member.
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a description of this team's member.
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// Gets a list of the current actions this member can use in combat.
        /// </summary>
        [JsonProperty]
        public Collection<BattleAction> Actions { get; } = new Collection<BattleAction>();

        /// <summary>
        /// Gets or sets the current maximum health of this member.
        /// </summary>
        [JsonProperty]
        public float MaxHealth { get; set; }

        /// <summary>
        /// Gets or sets the current health of this member.
        /// </summary>
        [JsonProperty]
        public float CurrentHealth { get; set; }
    }
}
