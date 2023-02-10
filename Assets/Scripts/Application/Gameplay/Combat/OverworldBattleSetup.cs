using Application.Core;
using System;


/// <summary>
/// Listens for the beginning of overworld battles, and prepares the world state
/// for the <see cref="BattleController"/>. 
/// </summary>
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
        };
        
        _controller.BeginBattle(battleData);
    }
}
