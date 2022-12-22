using System;
using Application.Core.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application.Level
{
    public class LevelSystem : IDisposable
    {
        private readonly LevelSettings _settings;

        private IDisposable _loadLevelEventHandler;
    
        public LevelSystem(LevelSettings settings)
        {
            _settings = settings;
            _loadLevelEventHandler = _settings.loadLevelEvent.AddListener(HandleLoadLevelEvent, "LevelSystem");
            
            Debug.Log($"Loaded level system, with \"{settings.name}\".");
        }

        private void HandleLoadLevelEvent(LoadLevelData data)
        {
            Debug.Log($"Loading {data.LevelName}...");
            SceneManager.LoadScene(data.LevelName);
        }
    
        public void Dispose()
        {
            _loadLevelEventHandler.Dispose();
            Debug.Log("Disposed level system.");
        }

        public void RunDemo()
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}