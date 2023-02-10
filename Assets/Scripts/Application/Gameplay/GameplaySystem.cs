using Application.Core;
using Application.Gameplay.Landmarks;
using System;

namespace Application.Gameplay
{
    public class GameplaySystem : IDisposable
    {
        private LevelLoader _levelLoader = new LevelLoader();
        private LandmarkViewer _landmarkViewer = new LandmarkViewer();
        private LevelDesignUtil _levelDesignUtil = new LevelDesignUtil();
        private BattleController _battleController = new BattleController();
        private OverworldBattleSetup _overworldBattleSetup = new OverworldBattleSetup();
        
        private DisposableBag _disposables;
        
        public void Init()
        {
            _disposables = new DisposableBag(new IDisposable[]
            {
                _landmarkViewer.Init(),
                _levelDesignUtil.Init(),
                _levelLoader.Init(),
                _overworldBattleSetup.Init(_battleController),
            });
            
            RegionDebugger.Init();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
