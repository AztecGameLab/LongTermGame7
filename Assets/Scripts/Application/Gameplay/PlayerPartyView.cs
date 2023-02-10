using System;
using System.Collections.Generic;
using UnityEngine;

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