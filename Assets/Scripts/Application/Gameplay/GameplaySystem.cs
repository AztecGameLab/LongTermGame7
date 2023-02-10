using Application.Core;
using Application.Gameplay.Landmarks;
using System;
using Object = UnityEngine.Object;

namespace Application.Gameplay
{
    public class GameplaySystem : IDisposable
    {
        private LevelLoader _levelLoader = new LevelLoader();
        
        // ImGui debugging utilities
        private LandmarkViewer _landmarkViewer = new LandmarkViewer();
        private LevelDesignUtil _levelDesignUtil = new LevelDesignUtil();

        // Combat-related systems
        private OverworldBattleSetup _overworldBattleSetup = new OverworldBattleSetup();
        private ArenaBattleSetup _arenaBattleSetup = new ArenaBattleSetup();
        private BattleController _battleController;
        
        private DisposableBag _disposables;
        
        public void Init()
        {
            _battleController = Object.FindObjectOfType<BattleController>();
            
            _disposables = new DisposableBag(new IDisposable[]
            {
                _landmarkViewer.Init(),
                _levelDesignUtil.Init(),
                _levelLoader.Init(),
                _overworldBattleSetup.Init(_battleController),
                _arenaBattleSetup.Init(_battleController),
            });
            
            RegionDebugger.Init();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
