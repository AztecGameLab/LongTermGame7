namespace Application.Gameplay
{
    using UniRx;
    using UnityEngine;

    [RequireComponent(typeof(LivingEntity))]
    public class DestroyOnDeath : MonoBehaviour
    {
        private void Awake()
        {
            var entity = GetComponent<LivingEntity>();
            entity.OnDeath.Subscribe(_ => Destroy(gameObject));
        }
    }
}
