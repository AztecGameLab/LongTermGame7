namespace Application.Core
{
    using Application.Core.ScriptableObjects;
    using UnityEngine;

    /// <summary>
    /// Script that should run when a scene loads to set GameData values
    /// </summary>
    public class OnSceneLoad : MonoBehaviour
    {
        [SerializeField]
        private GameData.Region _thisRegion = GameData.Region.Undefined;

        // Start is called before the first frame update
        void Start()
        {
            if (_thisRegion == GameData.Region.Undefined)
            {
                Debug.LogWarning("This region is undefined! Please change this in this scene's OnSceneLoad.");
            }

            GameData.CurrentRegion = _thisRegion;
        }
    }
}