namespace Application.Gameplay.Combat.Brains
{
    using System.Collections.Generic;
    using UniRx;
    using UnityEngine;

    public class ProtectorEnemy : MonoBehaviour
    {
        public GameObject shieldPrefab;
        public LivingEntity entity;
        public LivingEntity[] targets;
        public Material shieldMaterial;
        public Color shieldColor;

        private Dictionary<LivingEntity, GameObject> _entitiesToShields
            = new Dictionary<LivingEntity, GameObject>();

        private void Start()
        {
            entity.OnDeath.Subscribe(_ => BreakShields()).AddTo(this);
            AddShields();
        }

        private void AddShield(LivingEntity target)
        {
            target.IsInvincible = true;
            var instance = new GameObject("Shield Effect");
            instance.transform.SetParent(target.transform, false);

            foreach (SpriteRenderer targetRenderer in target.GetComponentsInChildren<SpriteRenderer>())
            {
                var shield = Instantiate(targetRenderer.gameObject, instance.transform).GetComponent<SpriteRenderer>();
                shield.material = shieldMaterial;
                shield.color = shieldColor;
            }

            instance.transform.position += Vector3.back * 0.01f;
            _entitiesToShields.Add(target, instance);
        }

        private void BreakShield(LivingEntity target)
        {
            target.IsInvincible = false;
            var instance = _entitiesToShields[target];
            Destroy(instance);
        }

        private void AddShields()
        {
            foreach (LivingEntity target in targets)
            {
                AddShield(target);
            }
        }

        private void BreakShields()
        {
            foreach (LivingEntity target in targets)
            {
                BreakShield(target);
            }
        }
    }
}
