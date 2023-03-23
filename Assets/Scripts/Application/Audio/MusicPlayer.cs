namespace Application.Audio
{
    using System;
    using System.Collections.Generic;
    using FMOD;
    using FMOD.Studio;
    using FMODUnity;

    /// <summary>
    /// Manages the playing of music in the game.
    /// </summary>
    public class MusicPlayer
    {
        private readonly LinkedList<ActiveMusic> _musicList = new LinkedList<ActiveMusic>();

        /// <summary>
        /// Gets the currently playing music.
        /// </summary>
        /// <value>
        /// The currently playing music.
        /// </value>
        public IReadOnlyCollection<ActiveMusic> MusicList => _musicList;

        /// <summary>
        /// Begins playing a new song. Old music will be paused for this new song.
        /// When this new song is removed or disposed, the old music begins playing again.
        /// </summary>
        /// <param name="musicEvent">The new song to play.</param>
        /// <param name="pauseForInterrupts">How should this music handle being interrupted? If true, the
        /// music will pause when something else is played on top of it, and resume when it becomes active
        /// again. If false, the music will completely stop and restart when interrupted.</param>
        /// <returns>A disposable for stopping the newly created song.</returns>
        public ActiveMusic AddMusic(EventReference musicEvent, bool pauseForInterrupts = true)
        {
            ActiveMusic musicToPlay = new ActiveMusic(this, RuntimeManager.CreateInstance(musicEvent), pauseForInterrupts);

            // Stop the currently playing music if it exists.
            _musicList.First?.Value.SetPlaying(false);
            _musicList.AddFirst(musicToPlay);
            musicToPlay.SetPlaying(true);
            return musicToPlay;
        }

        /// <summary>
        /// Removes a song from the player. If it is active, then the currently playing music will change.
        /// </summary>
        /// <param name="musicToRemove">The music to remove.</param>
        public void RemoveMusic(ActiveMusic musicToRemove)
        {
            ActiveMusic currentMusic = _musicList.First.Value;
            var currentMusicId = GetGuid(currentMusic.Instance);

            musicToRemove.Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _musicList.Remove(musicToRemove);

            // If there is still music in the list after removing this one, we want to check to see if
            // it needs to be started or stopped.
            if (_musicList.Count > 0)
            {
                ActiveMusic newMusic = _musicList.First.Value;
                var newMusicId = GetGuid(newMusic.Instance);

                if (currentMusicId != newMusicId)
                {
                    _musicList.First?.Value.SetPlaying(true);
                }
            }
        }

        private static GUID GetGuid(EventInstance instance)
        {
            instance.getDescription(out var desc);
            desc.getID(out var guid);
            return guid;
        }

        /// <summary>
        /// Represents a currently active piece of music in the game.
        /// </summary>
        public readonly struct ActiveMusic : IDisposable
        {
            private readonly bool _pauseForInterrupts;
            private readonly MusicPlayer _parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActiveMusic"/> struct.
            /// </summary>
            /// <param name="parent">The music player managing this music.</param>
            /// <param name="instance">The music this object represents.</param>
            /// <param name="pauseForInterrupts">How should this music handle being interrupted? If true, the
            /// music will pause when something else is played on top of it, and resume when it becomes active
            /// again. If false, the music will completely stop and restart when interrupted.</param>
            public ActiveMusic(MusicPlayer parent, EventInstance instance, bool pauseForInterrupts)
            {
                _parent = parent;
                Instance = instance;
                _pauseForInterrupts = pauseForInterrupts;

                if (pauseForInterrupts)
                {
                    Instance.start();
                    Instance.setPaused(true);
                }
            }

            /// <summary>
            /// Gets the FMOD instance data for this music.
            /// </summary>
            /// <value>
            /// The FMOD instance data for this music.
            /// </value>
            public EventInstance Instance { get; }

            /// <summary>
            /// Removes this music from the player, allowing previously added songs to resume.
            /// </summary>
            public void Dispose()
            {
                _parent.RemoveMusic(this);
            }

            /// <summary>
            /// Changes the current playing status of this instance. The actual
            /// behavior may change based on this music's pauseForInterrupts setting.
            /// </summary>
            /// <param name="value">Should this music be playing.</param>
            public void SetPlaying(bool value)
            {
                if (_pauseForInterrupts)
                {
                    Instance.setPaused(!value);
                }
                else
                {
                    if (value)
                    {
                        Instance.start();
                    }
                    else
                    {
                        Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    }
                }
            }
        }
    }
}
