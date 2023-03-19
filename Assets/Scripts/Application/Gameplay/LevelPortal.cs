namespace Application.Gameplay
{
    using UnityEngine;

    /// <summary>
    /// A simplified interface for connecting levels.
    /// </summary>
    public class LevelPortal : MonoBehaviour
    {
        [SerializeField]
        private string portalId;

        [SerializeField]
        private string targetScene;

        private void Start()
        {
            LevelEntrance entrance = GetComponentInChildren<LevelEntrance>();
            LevelExit exit = GetComponentInChildren<LevelExit>();

            entrance.EntranceID = portalId;
            exit.TargetScene = targetScene;
            exit.TargetID = portalId;
        }
    }
}
