namespace Application.Gameplay
{
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// Instantiates an object when this entity dies.
    /// </summary>
    [RequireComponent(typeof(LivingEntity))]
    public class SpawnOnDeath : MonoBehaviour
    {
        [SerializeField]
        private GameObject prefabToSpawn;

        private void Awake()
        {
            var entity = GetComponent<LivingEntity>();
            var t = transform;
            entity.OnDeath.Subscribe(_ => Instantiate(prefabToSpawn, t.position, t.rotation));
        }
    }
}
