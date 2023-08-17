using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Application.Gameplay;
using UniRx;

public class KillEnemy : MonoBehaviour
{
    [SerializeField] private LivingEntity entity;

    public void Start()
    {
        entity.OnDeath.Subscribe(Lloyd => HandleEntityDeath());
    }

    private void HandleEntityDeath()
    {
        Destroy(gameObject);
    }

}