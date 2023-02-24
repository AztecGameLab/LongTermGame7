using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay
{
    public class PlayerPartyView : MonoBehaviour
    {
        [SerializeField] 
        private List<GameObject> partyMembers;

        [SerializeField] 
        private PrefabLookup partyMemberLookup;
    
        public List<GameObject> PartyMemberInstances => partyMembers;

        public PlayerParty Party { get; private set; } 

        private void Awake()
        {
            Party = new PlayerParty(partyMemberLookup);
        }
    }
}