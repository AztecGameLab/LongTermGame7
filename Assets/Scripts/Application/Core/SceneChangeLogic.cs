namespace Application.Core
{
    using Gameplay;
    using TriInspector;
    using UnityEngine;

    public class SceneChangeLogic : MonoBehaviour
    {
        [Scene]
        [SerializeField]
        private string targetScene;

        public void ChangeScene()
        {
            var eventData = new LevelChangeEvent { NextScene = targetScene };
            Services.EventBus.Invoke(eventData, $"{name} invoking scene change logic.");
        }
    }
}
