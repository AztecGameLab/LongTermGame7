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
    public class GameplaySystem : IDisposable
    {
        [SerializeField]
        private BattleController battleController;

        private LevelLoader _levelLoader = new LevelLoader();

        // ImGui debugging utilities
        private LandmarkViewer _landmarkViewer = new LandmarkViewer();
        private LevelDesignUtil _levelDesignUtil = new LevelDesignUtil();

        // Combat-related systems
        [SerializeField]
        private OverworldBattleSetup overworldBattleSetup;

        [SerializeField]
        private ArenaBattleSetup arenaBattleSetup;

        private CompositeDisposable _disposables;

        private GameplayLauncher _launcher = new GameplayLauncher();

        /// <summary>
        /// Sets up the gameplay settings.
        /// </summary>
        public void Init()
        {
            _disposables = new CompositeDisposable(
                _landmarkViewer.Init(),
                _levelDesignUtil.Init(),
                _levelLoader.Init(),
                overworldBattleSetup.Init(battleController),
                arenaBattleSetup.Init(battleController));

            _launcher.Initialize();
            _launcher.AddTo(_disposables);

            RegionDebugger.Init();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
