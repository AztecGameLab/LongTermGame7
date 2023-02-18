using Application.Core;
using Application.StateMachine;
using System;

public abstract class BattleState : IState
{
    public BattleController Controller { get; set; }

    private IDisposable _disposable;

    public virtual void OnEnter()
    {
        _disposable = ImGuiUtil.Register(DrawGui);
    }
    
    public virtual void OnExit()
    {
        _disposable?.Dispose();
    }
    
    public virtual void OnTick() {}
    protected virtual void DrawGui() {}
}