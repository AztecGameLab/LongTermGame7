using UnityEngine;

namespace Application.Core.Events
{
    [CreateAssetMenu]
    public class LoadLevelEvent : Event<LoadLevelData> { }

    public class LoadLevelData
    {
        public string LevelName;
    }
}