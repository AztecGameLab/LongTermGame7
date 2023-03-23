namespace Application.Gameplay
{
    using System;
    using Combat;
    using Core.Utility;
    using Landmarks;
    using Regions;
    using UniRx;
    using UnityEngine;

    /// <summary>
    /// The system settings for gameplay.
    /// </summary>
    [Serializable]
    public class GameplaySystem : MonoBehaviour
    {
        [SerializeField]
        private BattleController battleController;


        // ImGui debugging utilities
        private LandmarkViewer _landmarkViewer = new LandmarkViewer();
        private LevelDesignUtil _levelDesignUtil = new LevelDesignUtil();

        // Combat-related systems
        [SerializeField]
        private OverworldBattleSetup overworldBattleSetup;

        [SerializeField]
        private ArenaBattleSetup arenaBattleSetup;

        private CompositeDisposable _disposables;

        private void Awake()
        {
            _disposables = new CompositeDisposable(
                _landmarkViewer.Init(),
                _levelDesignUtil.Init(),
                overworldBattleSetup.Init(battleController),
                arenaBattleSetup.Init(battleController));

            RegionDebugger.Init();
        }

        private void OnDestroy()
        {
            _disposables.Dispose();
        }
    }
}
