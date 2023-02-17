using Application.StateMachine;

public abstract class BattleState : IState
{
    public BattleController Controller { get; set; }
    
    public virtual void OnEnter() {}
    public virtual void OnTick() {}
    public virtual void OnExit() {}
}