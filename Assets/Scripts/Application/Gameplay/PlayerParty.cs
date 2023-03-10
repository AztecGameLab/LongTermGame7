namespace Application.Gameplay
{
    using System.Collections.Generic;
    using Core.Serialization;
    using UnityEngine;

    /// <summary>
    /// Holds all additional objects that should be taken into battle with the player.
    /// </summary>
    public class PlayerParty : ISerializable
    {
        private readonly PrefabLookup _lookup;
        private List<int> _prefabIds;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerParty"/> class.
        /// </summary>
        /// <param name="lookup">The prefab lookup for spawning in player party prefabs.</param>
        public PlayerParty(PrefabLookup lookup)
        {
            _lookup = lookup;
            _prefabIds = new List<int>();
        }

        /// <inheritdoc/>
        public string Id => "PlayerParty";

        /// <summary>
        /// Returns a list of all party member prefabs.
        /// </summary>
        /// <returns>Prefabs of each member in the player party.</returns>
        public GameObject[] GetPartyMemberPrefabs()
        {
            GameObject[] result = new GameObject[_prefabIds.Count];

            for (int i = 0; i < _prefabIds.Count; i++)
            {
                result[i] = _lookup.GetPrefab(_prefabIds[i]);
            }

            return result;
        }

        /// <summary>
        /// Adds a new member to the party.
        /// </summary>
        /// <param name="prefab">The prefab to add.</param>
        public void AddPartyMemberPrefab(GameObject prefab)
        {
            _prefabIds.Add(_lookup.GetId(prefab));
        }

        /// <summary>
        /// Removes a member from the party.
        /// </summary>
        /// <param name="prefab">The prefab to remove.</param>
        public void RemovePartyMemberPrefab(GameObject prefab)
        {
            _prefabIds.Remove(_lookup.GetId(prefab));
        }

        /// <inheritdoc/>
        public void ReadData(object data)
        {
            _prefabIds = (List<int>)data;
        }

        /// <inheritdoc/>
        public object WriteData()
        {
            return _prefabIds;
        }
    }
}
