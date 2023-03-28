namespace Application.Gameplay.Combat
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core;
    using Dialogue;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// Keeps track of the currently selected respawn point.
    /// </summary>
    [Serializable]
    public class RespawnTracker : IDisposable
    {
        [SerializeField]
        private DictionaryGenerator<string, RespawnData> respawnLookup;

        private Dictionary<string, RespawnData> _respawnLookup;
        private RespawnData _currentRespawnPoint;

        /// <summary>
        /// Sets up this respawn tracker.
        /// </summary>
        public void Init()
        {
            _respawnLookup = respawnLookup.GenerateDictionary();
            Services.Serializer.Lookup("respawn", out _currentRespawnPoint);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Services.Serializer.Store("respawn", _currentRespawnPoint);
        }

        /// <summary>
        /// Changes the active respawn point.
        /// </summary>
        /// <param name="respawnPointId">The ID of the respawn point to load.</param>
        public void SetRespawnPoint(string respawnPointId)
        {
            _currentRespawnPoint = _respawnLookup[respawnPointId];
        }

        /// <summary>
        /// Respawn the player to the current respawn point.
        /// </summary>
        public void Respawn()
        {
            var message = new LevelChangeEvent
            {
                NextScene = _currentRespawnPoint.sceneName,
                SpawningStrategy = new RespawnPointSpawningStrategy(_currentRespawnPoint.respawnPointId),
            };

            Services.EventBus.Invoke(message, "Respawn Tracker");
        }

        /// <summary>
        /// Information about a respawn point.
        /// </summary>
        [Serializable]
        public struct RespawnData
        {
            /// <summary>
            /// The name of the scene that contains this point.
            /// </summary>
            public string sceneName;

            /// <summary>
            /// The ID of the point in the scene.
            /// </summary>
            public string respawnPointId;
        }

        private sealed class RespawnPointSpawningStrategy : ISpawningStrategy
        {
            private readonly string _respawnPointId;

            public RespawnPointSpawningStrategy(string respawnPointId)
            {
                _respawnPointId = respawnPointId;
            }

            public Vector3 CalculateSpawnPosition()
            {
                var respawnPoint = Object.FindObjectsOfType<RespawnPoint>().FirstOrDefault(respondPoint => respondPoint.Id == _respawnPointId);

                if (respawnPoint != null)
                {
                    return respawnPoint.transform.position;
                }

                return default;
            }
        }
    }
}
