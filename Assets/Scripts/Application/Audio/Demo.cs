using System;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Application.Audio
{
    public class Demo : MonoBehaviour
    {
        public EventReference reference;
        public float value;

        private EventInstance _instance;
        
        private void Start()
        {
            // RuntimeManager.PlayOneShot(reference);
            
            _instance = RuntimeManager.CreateInstance(reference);
            _instance.start();
        }

        private void Update()
        {
            _instance.setParameterByName("Intensity", value);
        }

        private void OnDestroy()
        {
            _instance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}