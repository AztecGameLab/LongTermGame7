using System.Collections.Generic;
using UnityEngine;

namespace Application.Gameplay.Combat.UI
{
    [CreateAssetMenu]
    public class TeamMemberAuthoring : ScriptableObject
    {
        [SerializeField]
        private string memberName;
        
        [SerializeField]
        private string memberDescription;
        
        [SerializeReference]
        private List<BattleAction> actions;

        [SerializeField]
        private float maxHealth;

        public TeamMemberData GenerateData()
        {
            return new TeamMemberData
            {
                name = memberName,
                actions = actions,
                description = memberDescription,
                currentHealth = maxHealth,
                maxHealth = maxHealth,
            };
        }
    }
}