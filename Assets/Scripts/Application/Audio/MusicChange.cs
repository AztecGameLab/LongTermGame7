namespace Application.Audio
{
    using Core;
    using FMODUnity;
    using UnityEngine;

    public class MusicChange : MonoBehaviour
    {
        public EventReference music;

        private static EventReference _activeMusic;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            _activeMusic = default;
        }

        private void Start()
        {
            if (_activeMusic.Guid != music.Guid)
            {
                foreach (MusicPlayer.ActiveMusic activeMusic in Services.MusicPlayer.MusicList)
                {
                    activeMusic.Dispose();
                }

                Services.MusicPlayer.AddMusic(music);
                _activeMusic = music;
            }
        }
    }
}
