namespace Application.Gameplay
{
    using Combat.Actions;
    using Newtonsoft.Json;
    using UniRx;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;

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
        public IReactiveCollection<BattleAction> Actions { get; } = new ReactiveCollection<BattleAction>();

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

        /// <summary>
        /// Gets or sets the maximum action points of this member.
        /// </summary>
        [JsonProperty]
        public int MaxActionPoints { get; set; }

        /// <summary>
        /// Gets or sets the addressable asset path to a world view for this member.
        /// </summary>
        [JsonProperty]
        public string WorldViewAssetPath { get; set; }

        public TeamMemberWorldView CreateWorldView()
        {
            return CreateWorldView(Vector3.zero, Quaternion.identity);
        }

        /// <summary>
        /// Instantiates a GameObject representation of this team member.
        /// </summary>
        /// <returns>An instance of this team member.</returns>
        public TeamMemberWorldView CreateWorldView(Vector3 position, Quaternion rotation)
        {
            AsyncOperationHandle<GameObject> test = Addressables.InstantiateAsync(WorldViewAssetPath, position, rotation);
            var result = test.WaitForCompletion().GetComponentInChildren<TeamMemberWorldView>(true);
            result.BindTo(this);
            return result;
        }
    }
}
