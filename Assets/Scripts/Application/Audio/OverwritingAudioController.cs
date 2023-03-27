namespace Application.Audio
{
    using System.Collections.Generic;
    using FMODUnity;
    using UnityEngine;
    using STOP_MODE = FMOD.Studio.STOP_MODE;

    /// <summary>
    /// A controller for overwriting audio triggers.
    /// All overwriting audio triggers are linked through this controller.
    /// </summary>
    public class OverwritingAudioController : MonoBehaviour
    {
        private readonly LinkedList<OverwritingAudioData> _reverbData = new LinkedList<OverwritingAudioData>();

        /// <summary>
        /// Adds some audio data.
        /// </summary>
        /// <param name="data">The data to add.</param>
        public void AddData(OverwritingAudioData data)
        {
            if (_reverbData.Count > 0)
            {
                _reverbData.First.Value.Instance.setParameterByName("Intensity", 0);
            }

            data.Instance = RuntimeManager.CreateInstance(data.reference);
            data.Instance.start();
            _reverbData.AddFirst(data);
        }

        /// <summary>
        /// Removes some audio data.
        /// </summary>
        /// <param name="data">The data to remove.</param>
        public void RemoveData(OverwritingAudioData data)
        {
            data.Instance.stop(STOP_MODE.IMMEDIATE);
            data.Instance.release();
            _reverbData.Remove(data);

            if (_reverbData.Count > 0)
            {
                var first = _reverbData.First.Value;
                first.Instance.setParameterByName("Intensity", 1);
            }
        }
    }
}
