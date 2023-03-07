namespace Application.Gameplay
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// MonoBehavior storage for the current player party.
    /// </summary>
    public class PlayerPartyView : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> partyMembers;

        [SerializeField]
        private PrefabLookup partyMemberLookup;

        /// <summary>
        /// Gets the currently spawned party members.
        /// </summary>
        public IReadOnlyCollection<GameObject> PartyMemberInstances => partyMembers;

        /// <summary>
        /// Gets the party data.
        /// </summary>
        public PlayerParty Party { get; private set; }

        private void Awake()
        {
            Party = new PlayerParty(partyMemberLookup);
        }
    }
}
