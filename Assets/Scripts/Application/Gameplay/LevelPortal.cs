namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// A simplified interface for connecting levels.
    /// </summary>
    public class LevelPortal : MonoBehaviour
    {
        [SerializeField]
        public string portalId;

        [SerializeField]
        public string targetScene;

        private void Awake()
        {
            LevelEntrance entrance = GetComponentInChildren<LevelEntrance>();
            LevelExit exit = GetComponentInChildren<LevelExit>();

            entrance.EntranceID = portalId;
            exit.TargetScene = targetScene;
            exit.TargetID = portalId;
        }
    }
}
