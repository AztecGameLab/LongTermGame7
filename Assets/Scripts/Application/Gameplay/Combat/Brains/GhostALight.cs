using UnityEngine;
namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    public class GhostALight : MonsterBrain
    {
        public float invisibilityRange;
        public float projectileDamage;
        public Collider targetCollider;
        public GameObject projectilePrefab;

        
        protected override IEnumerator MakeDecision(BattleController controller)
        {
            //if the closest person is too close, go invisibility
            if (controller.PlayerTeam.GetClosest(transform.position, out float distance) <= invisibilityRange)
            {
                targetCollider.enabled = false;
            }
            else {          //else, fire a projectile to the closest
                
            }
        }
        
    }
}
