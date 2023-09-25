﻿namespace Application.Audio
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using FMOD;
    using FMOD.Studio;
    using FMODUnity;
    using UniRx;
    using UnityEngine;

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

            if (_musicList.Count > 0)
            {
                musicToPlay.Instance.setVolume(0);
            }

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
            var currentMusicId = currentMusic.Instance.GetGuid();

            musicToRemove.SetPlaying(false);
            _musicList.Remove(musicToRemove);

            // If there is still music in the list after removing this one, we want to check to see if
            // it needs to be started or stopped.
            if (_musicList.Count > 0)
            {
                ActiveMusic newMusic = _musicList.First.Value;
                var newMusicId = newMusic.Instance.GetGuid();

                if (currentMusicId != newMusicId)
                {
                    _musicList.First?.Value.SetPlaying(true);
                }
            }
        }

        /// <summary>
        /// Represents a currently active piece of music in the game.
        /// </summary>
        public struct ActiveMusic : IDisposable
        {
            private readonly bool _pauseForInterrupts;
            private readonly MusicPlayer _parent;
            private IDisposable _volumeAnimation;

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
                _volumeAnimation = null;

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

            /// <inheritdoc/>
            public override bool Equals(object obj)
            {
                if (obj is ActiveMusic music)
                {
                    return Instance.handle == music.Instance.handle;
                }

                return false;
            }

            /// <inheritdoc/>
            public override int GetHashCode()
            {
                return Instance.handle.GetHashCode();
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
                    if (value)
                    {
                        Instance.setPaused(false);
                        _volumeAnimation?.Dispose();
                        _volumeAnimation = UpdateVolume(1).ToObservable().Subscribe();
                    }
                    else
                    {
                        var instance = Instance; // need to trap local copy for the lambda
                        _volumeAnimation?.Dispose();
                        _volumeAnimation = UpdateVolume(0).ToObservable().Subscribe(_ => instance.setPaused(true));
                    }
                }
                else
                {
                    if (value)
                    {
                        Instance.start();
                        _volumeAnimation?.Dispose();
                        _volumeAnimation = UpdateVolume(1).ToObservable().Subscribe();
                    }
                    else
                    {
                        var instance = Instance; // need to trap local copy for the lambda
                        _volumeAnimation?.Dispose();
                        _volumeAnimation = UpdateVolume(0).ToObservable().Subscribe(_ => instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT));
                    }
                }
            }

            private IEnumerator UpdateVolume(float to)
            {
                float elapsedTime = 0;
                const float duration = 1;
                Instance.getVolume(out float currentVolume);

                while (elapsedTime < duration)
                {
                    Instance.setVolume(Mathf.Lerp(currentVolume, to, elapsedTime / duration));
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                Instance.setVolume(to);
            }
        }
    }

    public static class EventInstanceExtensions
    {
        public static GUID GetGuid(this EventInstance instance)
        {
            instance.getDescription(out var desc);
            desc.getID(out var guid);
            return guid;
        }
    }
}
