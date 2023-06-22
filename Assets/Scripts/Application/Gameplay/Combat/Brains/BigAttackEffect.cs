using Cinemachine;
using System;
using UnityEngine;

namespace Application.Gameplay.Combat.Brains
{
    [Serializable]
    public class BigAttackEffect
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private CinemachineImpulseSource screenShake;

        public void Play(Vector3 position)
        {
            particles.transform.position = position;
            particles.Play();

            screenShake.GenerateImpulseAt(position, Vector3.one);
        }
    }
}