namespace Application.Audio
{
    using System;
    using FMOD.Studio;
    using FMODUnity;

    /// <summary>
    /// Some data that can be played and overwritten.
    /// </summary>
    [Serializable]
    public class OverwritingAudioData
    {
        /// <summary>
        /// The reference to the instance that should be created.
        /// </summary>
        public EventReference reference;

        /// <summary>
        /// The currently playing instance of this event.
        /// </summary>
        public EventInstance Instance;
    }
}
