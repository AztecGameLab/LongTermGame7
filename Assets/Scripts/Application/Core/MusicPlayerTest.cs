namespace Application
{
    using System;
    using FMOD.Studio;
    using FMODUnity;
    using UnityEngine;

    /// <summary>
    /// Used to test the functionality of the music player.
    /// </summary>
    public class MusicPlayerTest : MonoBehaviour
    {
        [SerializeField]
        private EventReference song1;

        [SerializeField]
        private EventReference song2;

        [SerializeField]
        private EventReference song3;

        private IDisposable _song1Handle;
        private IDisposable _song2Handle;
        private IDisposable _song3Handle;

        private MusicPlayer _musicPlayer;

        private static void DrawMusicInfo(EventInstance instance)
        {
            instance.getDescription(out var desc);
            desc.getID(out var id);
            GUILayout.Label($"\tSong: {id.ToString()}");
        }

        private void Awake()
        {
            _musicPlayer = new MusicPlayer();
        }

        private void OnGUI()
        {
            DrawSongUI($"Song 1 {song1.Guid}", song1, ref _song1Handle);
            DrawSongUI($"Song 2 {song2.Guid}", song2, ref _song2Handle);
            DrawSongUI($"Song 3 {song3.Guid}", song3, ref _song3Handle);

            GUILayout.Label($"Currently playing {_musicPlayer.MusicList.Count} song(s).");

            foreach (var music in _musicPlayer.MusicList)
            {
                DrawMusicInfo(music.Instance);
            }
        }

        private void DrawSongUI(string songName, EventReference reference, ref IDisposable handle)
        {
            if (handle == null && GUILayout.Button($"Add {songName}"))
            {
                handle = _musicPlayer.AddMusic(reference);
            }
            else if (handle != null && GUILayout.Button($"Remove {songName}"))
            {
                handle.Dispose();
                handle = null;
            }
            else
            {
                // No buttons have been pressed.
            }
        }
    }
}
