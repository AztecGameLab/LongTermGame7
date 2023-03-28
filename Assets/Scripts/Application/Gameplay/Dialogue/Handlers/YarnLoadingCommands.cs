namespace Application.Gameplay.Dialogue.Handlers
{
    using System;
    using System.Collections.Generic;
    using Core;
    using UnityEngine;
    using Yarn.Unity;

    /// <summary>
    /// Yarn commands for loading gameplay scenes with different strategies.
    /// </summary>
    [Serializable]
    public class YarnLoadingCommands : IYarnCommandHandler
    {
        [SerializeField]
        private DictionaryGeneratorAdvanced<string, string, SceneValuePair<string>> levelIds;

        private Dictionary<string, string> _levelLookup;

        /// <inheritdoc/>
        public void RegisterCommands(DialogueRunner runner)
        {
            _levelLookup = levelIds.GenerateDictionary();

            runner.AddCommandHandler<string>("level-load", HandleLoad);
            runner.AddCommandHandler<string, float, float, float>("level-load-position", HandleLoadPosition);
            runner.AddCommandHandler<string, string>("level-load-entrance", HandleLoadEntrance);
        }

        /// <inheritdoc/>
        public void UnregisterCommands(DialogueRunner runner)
        {
            runner.RemoveCommandHandler("level-load");
        }

        private void HandleLoadEntrance(string levelName, string entranceID)
        {
            var sceneName = _levelLookup[levelName];
            Services.EventBus.Invoke(new LevelChangeEvent { NextScene = sceneName, SpawningStrategy = new EntranceSpawningStrategy(entranceID) }, "Yarn Level Loading");
        }

        private void HandleLoadPosition(string levelName, float x, float y, float z)
        {
            var sceneName = _levelLookup[levelName];
            Services.EventBus.Invoke(new LevelChangeEvent { NextScene = sceneName, SpawningStrategy = new PositionSpawningStrategy(new Vector3(x, y, z)) }, "Yarn Level Loading");
        }

        private void HandleLoad(string levelName)
        {
            var sceneName = _levelLookup[levelName];
            Services.EventBus.Invoke(new LevelChangeEvent { NextScene = sceneName }, "Yarn Level Loading");
        }
    }
}
