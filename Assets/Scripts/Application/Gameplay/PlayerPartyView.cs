using System.Collections.Generic;
using UnityEngine;

public class PlayerPartyView : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> partyMembers;
    
    public List<GameObject> PartyMemberInstances => partyMembers;
}