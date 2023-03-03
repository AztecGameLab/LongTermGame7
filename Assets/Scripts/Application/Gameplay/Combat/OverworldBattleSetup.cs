using Application.Core;
using Application.Gameplay.Combat.Hooks;
using System;
using System.Collections.Generic;

namespace Application.Gameplay.Combat
{
    /// <summary>
    /// Listens for the beginning of overworld battles, and prepares the world state
    /// for the <see cref="BattleController"/>. 
    /// </summary>
    [Serializable]
    public class OverworldBattleSetup : IDisposable
    {
        private BattleController _controller;
        private IDisposable _disposable;
    
        public OverworldBattleSetup Init(BattleController controller)
        {
            _controller = controller;
            _disposable = Services.EventBus.AddListener<OverworldBattleStartData>(HandleBattleStart, "Overworld battle setup");
            return this;
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        private void HandleBattleStart(OverworldBattleStartData data)
        {
            var battleData = new BattleData
            {
                EnemyTeamInstances = data.EnemyTeamInstances, 
                PlayerTeamInstances = data.PlayerTeamInstances,
                Hooks = new List<Hook>(new []{new DebuggingHook()}),
            };
        
            _controller.BeginBattle(battleData);
        }
    }
}
