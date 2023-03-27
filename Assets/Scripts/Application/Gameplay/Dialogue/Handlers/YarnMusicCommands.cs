namespace Application.Gameplay.Dialogue.Handlers
{
    using System;
    using System.Collections.Generic;
    using Core;
    using FMODUnity;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Commands for playing music via yarn scripting.
    /// </summary>
    [Serializable]
    public class YarnMusicCommands : IYarnCommandHandler
    {
        [SerializeField]
        private DictionaryGenerator<string, EventReference> musicLookupAuthoring;

        private Dictionary<string, EventReference> _musicLookup;
        private Dictionary<string, IDisposable> _yarnActiveMusic;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _musicLookup = musicLookupAuthoring.GenerateDictionary();
            _yarnActiveMusic = new Dictionary<string, IDisposable>();

            runner.AddCommandHandler<string>("music-play", HandlePlayMusic);
            runner.AddCommandHandler<string>("music-stop", HandleStopMusic);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("music-play");
            runner.RemoveCommandHandler("music-stop");
        }

        private void HandlePlayMusic(string musicId)
        {
            var reference = _musicLookup[musicId];
            var disposable = Services.MusicPlayer.AddMusic(reference);
            _yarnActiveMusic.Add(musicId, disposable);
        }

        private void HandleStopMusic(string musicId)
        {
            if (_yarnActiveMusic.ContainsKey(musicId))
            {
                _yarnActiveMusic[musicId]?.Dispose();
                _yarnActiveMusic.Remove(musicId);
            }
            else
            {
                Debug.LogError($"YARN: Tried to stop music {musicId} that does not exist.");
            }
        }
    }
}
