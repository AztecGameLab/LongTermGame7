namespace Application.Audio
{
    using Core;
    using FMOD.Studio;
    using FMODUnity;
    using UnityEngine;
    using STOP_MODE = FMOD.Studio.STOP_MODE;

    /// <summary>
    /// A trigger that plays an audio effect while the player is inside.
    /// </summary>
    public class AudioTrigger : TriggerEffect
    {
        [SerializeField]
        private EventReference reverbSnapshot;

        private EventInstance _instance;

        /// <inheritdoc/>
        protected override void HandleCollisionEnter(GameObject obj)
        {
            base.HandleCollisionEnter(obj);
            _instance = RuntimeManager.CreateInstance(reverbSnapshot);
            _instance.start();
        }

        /// <inheritdoc/>
        protected override void HandleCollisionExit(GameObject obj)
        {
            base.HandleCollisionExit(obj);
            _instance.stop(STOP_MODE.ALLOWFADEOUT);
            _instance.release();
        }

        private void OnDestroy()
        {
            _instance.stop(STOP_MODE.ALLOWFADEOUT);
            _instance.release();
        }
    }
}
