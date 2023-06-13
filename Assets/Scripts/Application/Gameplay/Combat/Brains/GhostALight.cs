using UnityEngine;
namespace Application.Gameplay.Combat.Brains
{
    using System.Collections;
    public class GhostALight : MonsterBrain
    {
        public float invisibilityRange;
        public float projectileDamage;
        public Collider targetCollider;
        [SerializeField] private GameObject projectilePrefab;
        
        protected override IEnumerator MakeDecision(BattleController controller)
        {
            var closest = controller.PlayerTeam.GetClosest(transform.position, out float distance);
            //if the closest person is too close, go invisibility
            if (closest <= invisibilityRange)
            {
                targetCollider.enabled = false;
            }
            else {          //else, fire a projectile to the closest
                var launchVelocity = ProjectileMotion.GetLaunchVelocity(User.transform.position, closest);
                //spawn a projectile
                //PhysicsComponent physics = Prefab.GetComponent<PhysicsComponent>();
                
                //shoot projectile
                //physics.Velocity = launchVelocity;

                //Apply damage

            }
        }
        
    }
}
