using System;
using UnityEngine;

namespace Application.Gameplay
{
    public struct HealthChangeData
    {
        public float Delta;
        public float OldHealth;
        public float NewHealth;
        public Health Health;
    }
    
    public class Health : MonoBehaviour
    {
        public float health;
        
        public event Action<HealthChangeData> OnHealthChange;

        public void Damage(float amount)
        {
            var data = new HealthChangeData();
            data.Delta = -amount;
            data.OldHealth = health;
            data.Health = this;
            
            health -= amount;

            data.NewHealth = health;
            OnHealthChange?.Invoke(data);
        }

        public void Heal(float amount)
        {
            var data = new HealthChangeData();
            data.Delta = amount;
            data.OldHealth = health;
            data.Health = this;

            health += amount;

            data.NewHealth = health;
            OnHealthChange?.Invoke(data);
        }
    }
}
