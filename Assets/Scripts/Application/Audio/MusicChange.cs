namespace Application.Audio
{
    using Core;
    using FMODUnity;
    using UnityEngine;

    public class MusicChange : MonoBehaviour
    {
        public EventReference music;

        private static MusicPlayer.ActiveMusic _activeMusic;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _activeMusic = default;
        }

        private void Start()
        {
            if (_activeMusic.Instance.GetGuid() != music.Guid)
            {
                if (_activeMusic.Instance.isValid())
                    _activeMusic.Dispose();

                _activeMusic = Services.MusicPlayer.AddMusic(music);
            }
        }
    }
}
