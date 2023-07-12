using Cinemachine;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Combat.Brains
{
    [Serializable]
    public class BigAttackEffect
    {
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private CinemachineImpulseSource screenShake;

        private ParticleSystem _particleInstance;

        public void Initialize()
        {
            _particleInstance = Object.Instantiate(particles);
        }

        public void Play(Vector3 position)
        {
            _particleInstance.transform.position = position;
            _particleInstance.Play();

            screenShake.GenerateImpulseAt(position, Vector3.one);
        }
    }
}
