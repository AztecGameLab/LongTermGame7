namespace Application
{
    using System;
    using System.Collections.Generic;
    using FMOD.Studio;
    using FMODUnity;
    using FMOD;

    /// <summary>
    /// Manages the playing of music in the game.
    /// </summary>
    public class MusicPlayer
    {
        private readonly LinkedList<ActiveMusic> _musicList = new LinkedList<ActiveMusic>();

        /// <summary>
        /// Gets the currently playing music.
        /// </summary>
        public IReadOnlyCollection<ActiveMusic> MusicList => _musicList;

        /// <summary>
        /// Begins playing a new song. Old music will be paused for this new song.
        /// When this new song is removed or disposed, the old music begins playing again.
        /// </summary>
        /// <param name="song">The new song to play.</param>
        /// <returns>A disposable for stopping the newly created song.</returns>
        public ActiveMusic AddMusic(EventReference song)
        {
            ActiveMusic music = new ActiveMusic(this, RuntimeManager.CreateInstance(song));

            // now that the music is set up, we need to actually manage the logic
            // of figuring out which song should currently be playing. Only the most recently
            // added song should play, but if it is removed the next most recent one starts.
            if (_musicList.First != null) {
                _musicList.First.Value.Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);    // fades out current song if there is one playing
            }
            
            _musicList.AddFirst(music);                                         // adds new song to be played to front of list
            music.Instance.start();                                             // plays new song

            // Note: don't worry about automatically removing a song if it ends! that responsibility
            // should be on the user of the player, and most music in the game loops anyways.

            // 1) Here is how to start / stop a song, along with some other cool and useful operations.
            // More info here! https://www.fmod.com/docs/2.02/api/studio-api-eventinstance.html
            //music.Instance.start();
            //music.Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            //music.Instance.setPaused(true);
            //music.Instance.getDescription(out EventDescription description);
            //description.getLength(out int songLengthInMilliseconds);

            // 2) Here is how to manage a C# Linked List.
            // More info here! https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1?view=net-7.0
            //_musicList.AddFirst(music);
            //_musicList.RemoveFirst();
            //_musicList.Remove(music);
            //ActiveMusic mostRecentMusic = _musicList.First.Value;
            //ActiveMusic oldestMusic = _musicList.Last.Value;

            return music;
        }

        /// <summary>
        /// Removes a song from the player. If it is active, then the currently playing music will change.
        /// </summary>
        /// <param name="music">The music to remove.</param>
        public void RemoveMusic(ActiveMusic music)
        {
            // You'll want to remove the requested music from the internal list, and update
            // the currently playing music if it changed.

            ActiveMusic currentSong = _musicList.First.Value;
            currentSong.Instance.getDescription(out EventDescription currDesc);
            currDesc.getID(out GUID currID);                                    // obtain ID of current song
            music.Instance.getDescription(out EventDescription musDesc);
            musDesc.getID(out GUID musID);                                      // obtain ID of song to be played

            if (currID == musID) {                                              // if the current playing music is the one to be removed
                music.Instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);            // stop playing with a fadeout
                _musicList.Remove(music);                                           // remove song from the list
                if (_musicList.First != null) {                                     // if there is another song to be played, play it
                    _musicList.First.Value.Instance.start();
                }
            } else {
                _musicList.Remove(music);                                       // otherwise, just remove the music.
            }
        }

        // note to adam; dont worry too music about the disposable stuff below, it should already be all set up.
        // Essentially, it allows the user to mess with the music EventInstance if they want, and lets them remove
        // the music from the player by calling "Dispose" on the thing they get back.

        /// <summary>
        /// Represents a currently active piece of music in the game.
        /// </summary>
        public readonly struct ActiveMusic : IDisposable
        {
            private readonly MusicPlayer _parent;

            /// <summary>
            /// Initializes a new instance of the <see cref="ActiveMusic"/> struct.
            /// </summary>
            /// <param name="parent">The music player managing this music.</param>
            /// <param name="instance">The music this object represents.</param>
            public ActiveMusic(MusicPlayer parent, EventInstance instance)
            {
                _parent = parent;
                Instance = instance;
            }

            /// <summary>
            /// Gets the FMOD instance data for this music.
            /// </summary>
            public EventInstance Instance { get; }

            /// <summary>
            /// Removes this music from the player, allowing previously added songs to resume.
            /// </summary>
            public void Dispose()
            {
                _parent.RemoveMusic(this);
            }
        }
    }
}