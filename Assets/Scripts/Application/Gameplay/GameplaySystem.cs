namespace Application.Gameplay
{
    using System;
    using Combat;
    using Core;
    using Landmarks;
    using Regions;
    using UniRx;
    using UnityEngine;
    using Object = UnityEngine.Object;

    /// <summary>
    /// The system settings for gameplay.
    /// </summary>
    [Serializable]
    public class GameplaySystem : IDisposable
    {
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

        /// <summary>
        /// Sets up the gameplay settings.
        /// </summary>
        public void Init()
        {
            var battleController = Object.FindObjectOfType<BattleController>();

            _disposables = new CompositeDisposable(
                _landmarkViewer.Init(),
                _levelDesignUtil.Init(),
                _levelLoader.Init(),
                overworldBattleSetup.Init(battleController),
                arenaBattleSetup.Init(battleController));

            RegionDebugger.Init();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
