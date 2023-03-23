using System;

namespace Application.Audio
{
    using UnityEngine;

    /// <summary>
    /// An audio trigger where only one source can be played at a time.
    /// </summary>
    public class OverwritingAudioTrigger : MonoBehaviour
    {
        [SerializeField]
        private OverwritingAudioData data;

        [SerializeField]
        private OverwritingAudioController controller;

        [SerializeField]
        private bool isGlobal;

        private void Start()
        {
            if (isGlobal)
            {
                controller.AddData(data);
            }
        }

        private void OnDestroy()
        {
            controller.RemoveData(data);
        }

        private void OnTriggerEnter()
        {
            if (isGlobal)
            {
                controller.AddData(data);
            }
        }

        private void OnTriggerExit()
        {
            if (!isGlobal)
            {
                controller.RemoveData(data);
            }
        }
    }
}
