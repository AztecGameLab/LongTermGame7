using UnityEngine;

namespace Application.Level
{
    [CreateAssetMenu(menuName = ApplicationConstants.AssetMenuName + "/"  + LevelConstants.AssetMenuName + "/Settings")]
    public class LevelSettings : ScriptableObject
    {
        // todo: annotation for easy scene selection
        public string demoSceneName;
    }
}