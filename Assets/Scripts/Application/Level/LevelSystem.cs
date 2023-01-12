using System;
using Application.Core;
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
            _loadLevelEventHandler = Services.EventBus.AddListener<LoadLevelEvent>(HandleLoadLevelEvent, "LevelSystem");
            
            Debug.Log($"Loaded level system, with \"{settings.name}\".");
        }

        private void HandleLoadLevelEvent(LoadLevelEvent @event)
        {
            Debug.Log($"Loading {@event.LevelName}...");
            SceneManager.LoadScene(@event.LevelName);
        }
    
        public void Dispose()
        {
            _loadLevelEventHandler.Dispose();
            Debug.Log("Disposed level system.");
        }

        public void RunDemo()
        {
            Debug.Log("Running demo...");
            SceneManager.LoadScene("SampleScene");
        }
    }
}