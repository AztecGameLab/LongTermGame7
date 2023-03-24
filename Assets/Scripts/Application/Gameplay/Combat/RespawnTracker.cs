using Application.Core;
using Application.Gameplay.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yarn.Unity;
using Object = UnityEngine.Object;

namespace Application.Gameplay.Combat
{
    //todo: this system needs a lot of cleaning
    [Serializable]
    public class RespawnTracker
    {
        private class RespawnPointSpawningStrategy : ISpawningStrategy
        {
            private readonly string _respawnPointId;

            public RespawnPointSpawningStrategy(string respawnPointId)
            {
                _respawnPointId = respawnPointId;
            }

            public Vector3 GetSpawnPosition()
            {
                var respawnPoint = Object.FindObjectsOfType<RespawnPoint>().FirstOrDefault(respondPoint => respondPoint.Id == _respawnPointId);

                if (respawnPoint != null)
                {
                    return respawnPoint.transform.position;
                }

                return default;
            }
        }

        [Serializable]
        public struct RespawnData
        {
            public string SceneName;
            public string RespawnPointId;
        }

        [SerializeField]
        private DictionaryGenerator<string, RespawnData> respawnLookup;

        private Dictionary<string, RespawnData> _respawnLookup;
        private RespawnData _currentRespawnPoint;

        public void Init()
        {
            _respawnLookup = respawnLookup.GenerateDictionary();
        }

        public void SetRespawnPoint(string respawnPointId)
        {
            _currentRespawnPoint = _respawnLookup[respawnPointId];
        }

        public void Respawn()
        {
            var message = new LevelChangeEvent
            {
                NextScene = _currentRespawnPoint.SceneName,
                SpawningStrategy = new RespawnPointSpawningStrategy(_currentRespawnPoint.RespawnPointId),
            };

            Services.EventBus.Invoke(message, "Respawn Tracker");
        }
    }

    public class YarnRespawnCommands : IYarnCommandHandler
    {
        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            runner.AddCommandHandler<string>("respawn-change", HandleRespawnChange);
        }

        private void HandleRespawnChange(string respawnPointId)
        {
            Services.RespawnTracker.SetRespawnPoint(respawnPointId);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("respawn-change");
        }
    }
}