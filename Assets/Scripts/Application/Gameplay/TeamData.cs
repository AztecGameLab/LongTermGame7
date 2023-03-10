namespace Application.Gameplay
{
    using Newtonsoft.Json;
    using UniRx;

    /// <summary>
    /// The player's current team information.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TeamData
    {
        /// <summary>
        /// Gets the total unlocked members of the players team.
        /// These are the team members that the player has the option of selecting
        /// to bring into battle.
        /// </summary>
        [JsonProperty]
        public ReactiveCollection<TeamMemberData> UnlockedMembers { get; }
            = new ReactiveCollection<TeamMemberData>();

        /// <summary>
        /// Gets the currently selected members of the players team.
        /// These are the team members that will be brought into combat.
        /// </summary>
        [JsonProperty]
        public ReactiveCollection<TeamMemberData> SelectedMembers { get; }
            = new ReactiveCollection<TeamMemberData>();

        /// <summary>
        /// Gets or sets the member data for the player themself.
        /// </summary>
        [JsonProperty]
        public TeamMemberData Player { get; set; }
    }
}
