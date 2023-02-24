using Application.Core;
using System;

namespace Application.Gameplay.Combat
{
    public class ArenaBattleSetup : IDisposable
    {
        private BattleController _controller;
        private IDisposable _disposable;
    
        public ArenaBattleSetup Init(BattleController controller)
        {
            _controller = controller;
            _disposable = Services.EventBus.AddListener<ArenaBattleStartData>(HandleArenaBattleStart, "Arena battle setup");
            return this;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    
        private void HandleArenaBattleStart(ArenaBattleStartData data)
        {
            // todo: load new scene, based on region and random selection
        
            // todo: find spawns
            // todo: instantiate prefabs into spawn positions
            // todo: pass a battle start to the controller
        }
    }
}