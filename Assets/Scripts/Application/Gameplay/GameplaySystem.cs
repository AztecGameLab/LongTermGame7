using Application.Core;
using Application.Gameplay.Combat;
using Application.Gameplay.Landmarks;
using Application.Gameplay.Regions;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Application.Gameplay
{
    [Serializable]
    public class GameplaySystem : IDisposable
    {
        private LevelLoader _levelLoader = new LevelLoader();
        
        // ImGui debugging utilities
        private LandmarkViewer _landmarkViewer = new LandmarkViewer();
        private LevelDesignUtil _levelDesignUtil = new LevelDesignUtil();

        // Combat-related systems
        [SerializeField] private OverworldBattleSetup overworldBattleSetup;
        [SerializeField] private ArenaBattleSetup arenaBattleSetup;
        
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
                overworldBattleSetup.Init(_battleController),
                arenaBattleSetup.Init(_battleController),
            });
            
            RegionDebugger.Init();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
