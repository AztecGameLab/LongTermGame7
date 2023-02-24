using Application.Core.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay
{
    /// <summary>
    /// Holds all additional objects that should be taken into battle with the player.
    /// </summary>
    public class PlayerParty : ISerializable
    {
        private readonly PrefabLookup _lookup;
        private List<int> _prefabIds;

        public PlayerParty(PrefabLookup lookup)
        {
            _lookup = lookup;
            _prefabIds = new List<int>();
        }
    
        public GameObject[] GetPartyMemberPrefabs()
        {
            GameObject[] result = new GameObject[_prefabIds.Count];

            for (int i = 0; i < _prefabIds.Count; i++)
            {
                result[i] = _lookup.GetPrefab(_prefabIds[i]);
            }

            return result;
        }

        public void AddPartyMemberPrefab(GameObject prefab)
        {
            _prefabIds.Add(_lookup.GetId(prefab));
        }

        public void RemovePartyMemberPrefab(GameObject prefab)
        {
            _prefabIds.Remove(_lookup.GetId(prefab));
        }

        public string GetID() => "PlayerParty";

        public void ReadData(object data)
        {
            _prefabIds = (List<int>)data;
        }

        public object WriteData()
        {
            return _prefabIds;
        }
    }
}